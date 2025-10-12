using UnityEngine;

public enum GameState
{
    IN_GAME, LOSE_ANIMATION, WIN_SCREEN
}

public class GameManager : MonoBehaviour
{
    public GenerationManager generationManager;
    public PlayerManager playerManager;

    public DeathZoneMove deathZone;

    public bool shouldGenerateTerrainOnLoad;

    public GameState gameState;

    void Awake()
    {
        if (shouldGenerateTerrainOnLoad)
            generationManager.Init();
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

        Debug.Log("Pretend that there's cool on lose stuff here");
    }
}
