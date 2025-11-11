using com.cyborgAssets.inspectorButtonPro;
using DG.Tweening.Core.Easing;
using System.Collections.Generic;
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
    public DisplayStringController stringController;
    public SceneLoadManager sceneLoadManager;
    public List<Menu> menus;

    public string mainMenuSceneName, inGameSceneName, winSceneName;

    public InputActionReference pauseAction;

    public PlayerManager playerManager;
    public DeathZoneMove deathZone;

    public bool shouldGenerateTerrainOnLoad;

    public GameState gameState;
    public AudioClip deathClip;

    public bool hasEnabledCurrentMenu = false;
    

    void Awake()
    {
        if (gameState == GameState.IN_GAME && shouldGenerateTerrainOnLoad)
            generationManager.Init();
        //pauseAction.action.started += SwitchToPause; 
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            SetCurrentMenusActive(true);
            SetNotCurrentMenusActive(false);
        }
    }

    private void LateUpdate()
    {
        if (!hasEnabledCurrentMenu)
        {
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

    public void SetStateInGame() => SetState(GameState.IN_GAME);
    public void SetStateLoseScreen() => SetState(GameState.LOSE_SCREEN);
    public void SetStateWinScreen() => SetState(GameState.WIN_SCREEN);
    public void SetStateUpgradeScreen() => SetState(GameState.UPGRADE_SCREEN);
    public void SetStateMainMenu() => SetState(GameState.MAIN_MENU);
    public void SetStateSettingsMenu() => SetState(GameState.SETTINGS_MENU);
    public void SetStatePauseMenu() => SetState(GameState.PAUSE_MENU);
    public void SetStateInventory() => SetState(GameState.INVENTORY);

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

    public void LoadStateInGame(string sceneName) => LoadState(GameState.IN_GAME, sceneName);
    public void LoadStateLoseScreen(string sceneName) => LoadState(GameState.LOSE_SCREEN, sceneName);
    public void LoadStateWinScreen(string sceneName) => LoadState(GameState.WIN_SCREEN, sceneName);
    public void LoadStateUpgradeScreen(string sceneName) => LoadState(GameState.UPGRADE_SCREEN, sceneName);
    public void LoadStateMainMenu(string sceneName) => LoadState(GameState.MAIN_MENU, sceneName);
    public void LoadStateSettingsMenu(string sceneName) => LoadState(GameState.SETTINGS_MENU, sceneName);
    public void LoadStatePauseMenu(string sceneName) => LoadState(GameState.PAUSE_MENU, sceneName);
    public void LoadStateInventory(string sceneName) => LoadState(GameState.INVENTORY, sceneName);

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }


    public void PlayerWin()
    {
        if (gameState != GameState.IN_GAME)
            return;

        playerManager.moveStun = -1;
        playerManager.SetDirection(EntityDirection.NEUTRAL);

        LoadStateWinScreen(winSceneName);
    }

    public void PlayerLose()
    {
        if (gameState != GameState.IN_GAME)
            return;

        AudioManager.instance.PlaySoundFXClip(deathClip, transform, 1.0f);

        LoadStateLoseScreen(mainMenuSceneName);
    }
}
