using UnityEngine;
using UnityEngine.Events;

public enum GameState
{
    IN_GAME, LOSE_ANIMATION, WIN_SCREEN, UPGRADE_SCREEN, MAIN_MENU, SETTINGS_MENU, PAUSE_MENU
}

public class GameManager : MonoBehaviour
{
    public GenerationManager generationManager;
    public DisplayStringController stringController;
    public SceneLoadManager sceneLoadManager;
    public GameObject inGameDisplay, loseDisplay, winDisplay, upgradeDisplay, mainMenuDisplay, settingsDisplay, pauseDisplay;

    public string mainMenuSceneName, inGameSceneName;

    public PlayerManager playerManager;
    public DeathZoneMove deathZone;

    public bool shouldGenerateTerrainOnLoad;

    public GameState gameState;

    void Awake()
    {
        if (gameState == GameState.IN_GAME && shouldGenerateTerrainOnLoad)
            generationManager.Init();
    }

    void Update()
    {
        inGameDisplay.SetActive(gameState == GameState.IN_GAME);
        loseDisplay.SetActive(gameState == GameState.LOSE_ANIMATION);
        winDisplay.SetActive(gameState == GameState.WIN_SCREEN);
        upgradeDisplay.SetActive(gameState == GameState.UPGRADE_SCREEN);
        mainMenuDisplay.SetActive(gameState == GameState.MAIN_MENU);
        settingsDisplay.SetActive(gameState == GameState.SETTINGS_MENU);
        pauseDisplay.SetActive(gameState == GameState.PAUSE_MENU);
    }

    public void PlayerWin()
    {
        if (gameState != GameState.IN_GAME)
            return;

        gameState = GameState.WIN_SCREEN;
        playerManager.moveStun = -1;
        playerManager.SetDirection(EntityDirection.NEUTRAL);

        Debug.Log("Pretend that there's cool on win stuff here");
    }

    public void PlayerLose()
    {
        if (gameState != GameState.IN_GAME)
            return;

        gameState = GameState.LOSE_ANIMATION;
        playerManager.moveStun = -1;
        playerManager.SetDirection(EntityDirection.NEUTRAL);

        Debug.Log("Pretend that there's cool on lose stuff here");
    }

    #region Menu Swappers

    public void SwitchToInGame()
    {
        if (gameState != GameState.MAIN_MENU && gameState != GameState.PAUSE_MENU)
            return;

        if (gameState == GameState.PAUSE_MENU)
        {
            Time.timeScale = 1;
            gameState = GameState.IN_GAME;
            return;
        }

        UnityEvent onFadeEvent = new();
        onFadeEvent.AddListener(() => { gameState = GameState.IN_GAME; generationManager.Init(); });
        sceneLoadManager.StartLoad(inGameSceneName, onFadeEvent);
    }

    public void SwitchToSettings()
    {
        if (gameState != GameState.MAIN_MENU)
            return;

        gameState = GameState.SETTINGS_MENU;
    }

    public void SwitchToMainMenu()
    {
        if (gameState != GameState.SETTINGS_MENU && gameState != GameState.PAUSE_MENU)
            return;

        if (gameState == GameState.SETTINGS_MENU)
        {
            gameState = GameState.MAIN_MENU;
            return;
        }

        UnityEvent onFadeEvent = new();
        onFadeEvent.AddListener(() => { gameState = GameState.SETTINGS_MENU; });
        sceneLoadManager.StartLoad(mainMenuSceneName, onFadeEvent);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    #endregion
}
