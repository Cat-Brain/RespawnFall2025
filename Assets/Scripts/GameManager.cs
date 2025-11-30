using com.cyborgAssets.inspectorButtonPro;
using DG.Tweening.Core.Easing;
using LDtkUnity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public enum GameState
{
    IN_GAME, LOSE_SCREEN, WIN_SCREEN, UPGRADE_SCREEN, MAIN_MENU, SETTINGS_MENU, PAUSE_MENU, INVENTORY
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

    [Header("Debug Variables")]
    public bool hasEnabledCurrentMenu = false;
    public int[] pathProgress;
    public LevelType[,] paths;
    public LevelType levelType;
    public List<int> pathTaken;
    public bool inCombat;

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

    public void LoadNextLevel(int path)
    {
        if (path == 0)
        {
            InitPaths();
            pathTaken.Add(path);
            generationManager.SpawnLevel(0, LevelType.COMBAT);
            return;
        }
        pathTaken.Add(path);
        path--;
        pathProgress[path]++;
        generationManager.SpawnLevel(pathProgress[path] + 1, paths[path, pathProgress[path] - 1]);
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


    public void PlayerWin()
    {
        if (gameState != GameState.IN_GAME)
            return;

        playerManager.moveStun = -1;
        playerManager.SetDirection(EntityDirection.NEUTRAL);

        LoadState(GameState.WIN_SCREEN, winSceneName);
    }

    public void PlayerLose()
    {
        if (gameState != GameState.IN_GAME)
            return;

        AudioManager.instance.PlaySoundFXClip(deathClip, transform, 1.0f);

        LoadState(GameState.LOSE_SCREEN, mainMenuSceneName);
    }
}
