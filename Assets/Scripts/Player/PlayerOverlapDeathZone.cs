using UnityEngine;

public class PlayerOverlapDeathZone : MonoBehaviour
{
    public PlayerManager playerManager;

    public float lostLifeRate;
    public float stunRate, maxStun;

    void Update()
    {
        if (transform.position.y <= playerManager.gameManager.deathZone.DeadlyHeight())
        {
            playerManager.gameManager.deathZone.TryAddStun(stunRate * Time.deltaTime, maxStun);
            playerManager.playerTimer.DecreaseTime(lostLifeRate * Time.deltaTime);
        }
    }
}
