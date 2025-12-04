using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public enum GameState
{
    IN_GAME, END_SCREEN, UNUSED, UPGRADE_SCREEN, MAIN_MENU, SETTINGS_MENU, PAUSE_MENU, INVENTORY
}

[ExecuteInEditMode]
public class GameManager : MonoBehaviour
{
    public GenerationManager generationManager;
    public InventoryController inventory;
    public DisplayStringController stringController;
    public SceneLoadManager sceneLoadManager;
    public List<Menu> menus;

    public string mainMenuSceneName, inGameSceneName, winSceneName;

    public InputActionReference pauseAction;

    public PlayerManager playerManager;

    public int pathCount;
    public LevelType[] basePath, pathEnd;
    public string[] levelTypeStrings;
    public Color[] levelTypeColors;
    
    public GameState gameState;
    public AudioClip deathClip;
    public GameObject lobby;

    [Header("Debug Variables")]
    public bool hasEnabledCurrentMenu = false;
    public int[] pathProgress;
    public LevelType[,] paths;
    public LevelType levelType;
    public List<int> pathTaken;
    public bool inCombat, endScreenLanded = false;
    public float startTime = 0, endTime = 0;

    void Update()
    {
        if (!Application.isPlaying)
        {
            SetNotCurrentMenusActive(false);
            SetCurrentMenusActive(true);
        }
    }

    private void LateUpdate()
    {
        if (!hasEnabledCurrentMenu && Application.isPlaying)
        {
            SetNotCurrentMenusActive(false);
            SetCurrentMenusActive(true);
            hasEnabledCurrentMenu = true;
        }
    }

    public void SetCurrentMenusActive(bool active)
    {
        menus.ForEach((menu) => { if (menu.state == gameState) menu.SetActive(active); });
    }

    public void SetNotCurrentMenusActive(bool active)
    {
        menus.ForEach((menu) => { if (menu.state != gameState) menu.SetActive(active); });
    }

    public void SetState(GameState state)
    {
        if (gameState == state)
            return;

        SetCurrentMenusActive(false);
        gameState = state;
        hasEnabledCurrentMenu = false;
    }

    public void LoadState(GameState state, string sceneName)
    {
        if (gameState == state)
            return;

        SetCurrentMenusActive(false);

        UnityEvent onFadeEvent = new();
        onFadeEvent.AddListener(() =>
        {
            gameState = state;
            hasEnabledCurrentMenu = false;
        });

        sceneLoadManager.sceneToLoad = sceneName;
        sceneLoadManager.StartLoad(onFadeEvent);
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    public void BeginGame()
    {
        startTime = Time.time;
        InitPaths();
    }

    public void LoadNextLevel(int path)
    {
        if (path == 0)
        {
            BeginGame();
            pathTaken.Add(path);
            generationManager.SpawnLevel(0, LevelType.COMBAT);
            return;
        }
        pathTaken.Add(path);
        path--;
        pathProgress[path]++;
        generationManager.SpawnLevel(pathProgress[path], paths[path, pathProgress[path] - 1]);
    }

    public string GetLevelExitText(int path)
    {
        if (path == 0)
            return levelTypeStrings[(int)LevelType.COMBAT] + " 1";
        return levelTypeStrings[(int)paths[path - 1, pathProgress[path - 1]]] + ' ' + (pathProgress[path - 1] + 2);
    }

    public Color GetLevelExitColor(int path)
    {
        if (path == 0)
            return levelTypeColors[(int)LevelType.COMBAT];
        return levelTypeColors[(int)paths[path - 1, pathProgress[path - 1]]];
    }

    public Color GetLevelEntryColor()
    {
        int path = pathTaken[^1];
        if (path == 0)
            return levelTypeColors[(int)LevelType.COMBAT];
        return levelTypeColors[(int)paths[path - 1, pathProgress[path - 1] - 1]];
    }

    public void InitPaths()
    {
        pathTaken = new ();
        pathProgress = new int[pathCount];
        paths = new LevelType[pathCount, basePath.Length + pathEnd.Length];

        for (int i = 0; i < pathCount; i++)
        {
            LevelType[] shuffledPath = basePath.Randomize().ToArray();
            for (int j = 0; j < basePath.Length; j++)
                paths[i, j] = shuffledPath[j];
            for (int j = 0; j < pathEnd.Length; j++)
                paths[i, j + basePath.Length] = pathEnd[j];
        }
    }

    public bool CanExitLevel()
    {
        return !inCombat && inventory.bufferItems.Count == 0;
    }

    public void LevelTransitionComplete()
    {
        lobby.SetActive(false);
        generationManager.UnloadInactiveLevels();
    }


    public void PlayerWin()
    {
        if (gameState != GameState.IN_GAME)
            return;

        ToEndScreen();
    }

    public void PlayerLose()
    {
        if (gameState != GameState.IN_GAME)
            return;

        ToEndScreen();
    }

    public void PlayerReset()
    {
        if (!playerManager.active)
            return;

        SetTimeScale(1);

        ToEndScreen();
    }

    public void ToEndScreen()
    {
        playerManager.End();
        endScreenLanded = false;
        lobby.SetActive(true);
        generationManager.LoadAllLevels();
        SetState(GameState.END_SCREEN);
    }

    public void EndLand()
    {
        if (endScreenLanded)
            return;

        endScreenLanded = true;
        generationManager.DespawnLevels();
        inCombat = false;
    }

    public void ExitEndScreen()
    {
        if (!endScreenLanded)
            return;

        playerManager.Begin();
        SetState(GameState.IN_GAME);
    }
}
