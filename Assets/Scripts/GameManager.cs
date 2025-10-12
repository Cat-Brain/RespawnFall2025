using UnityEngine;

public enum GameState
{
    IN_GAME, LOSE_ANIMATION, WIN_SCREEN
}

public class GameManager : MonoBehaviour
{
    public PlayerManager playerManager;
    public DeathZoneMove deathZone;

    public GameState gameState;
    public AudioClip deathClip;
    

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

        AudioManager.instance.PlaySoundFXClip(deathClip, transform, 1.0f);
        Debug.Log("Pretend that there's cool on lose stuff here");
    }
}
