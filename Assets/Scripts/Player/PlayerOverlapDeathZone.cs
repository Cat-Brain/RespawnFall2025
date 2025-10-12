using UnityEngine;

public class PlayerOverlapDeathZone : MonoBehaviour
{
    public PlayerManager playerManager;

    public string deathZoneTag;
    public float lostLifeRate;
    public float stunRate, maxStun;

    void OnTriggerStay2D(Collider2D trigger)
    {
        if (trigger.CompareTag(deathZoneTag))
        {
            playerManager.gameManager.deathZone.TryAddStun(stunRate * Time.fixedDeltaTime, maxStun);
            playerManager.playerTimer.DecreaseTime(lostLifeRate * Time.fixedDeltaTime);
        }
    }
}
