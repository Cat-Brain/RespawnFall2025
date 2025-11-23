using DG.Tweening.Core.Easing;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerHealth", menuName = "Health/PlayerHealth")]
public class PlayerHealth : Health
{
    [HideInInspector] public BarDisplay timerDisplay;
    [HideInInspector] public PlayerManager playerManager;

    public override void Init()
    {
        base.Init();
        RefreshTime();
    }

    public override void OnDeath()
    {
        if (playerManager == null)
            playerManager = inst.GetComponent<PlayerManager>();
        playerManager.gameManager.PlayerLose();
    }

    /*public override void OnHealthChange(int newValue, Hit? hit)
    {
        base.OnHealthChange(newValue, hit);
        RefreshTime();
    }*/

    public void RefreshTime()
    {
        if (timerDisplay == null)
            timerDisplay = FindAnyObjectByType<BarDisplay>(FindObjectsInactive.Include);
        timerDisplay.UpdateRemaining(inst.health, maxHealth);
    }
}
