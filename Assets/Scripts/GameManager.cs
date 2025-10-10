using UnityEngine;

public enum GameState
{
    IN_GAME, LOSE_ANIMATION
}

public class GameManager : MonoBehaviour
{
    public PlayerManager playerManager;

    public GameState gameState;

    public void PlayerLose()
    {
        if (gameState != GameState.IN_GAME)
            return;

        gameState = GameState.LOSE_ANIMATION;
        playerManager.moveStun = -1;

        Debug.Log("Pretend that there's cool on lose stuff here");
    }
}
