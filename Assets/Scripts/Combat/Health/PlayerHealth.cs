using UnityEngine;

public class PlayerHealth : Health
{
    public BarDisplay timerDisplay;
    public PlayerManager playerManager;

    new public void Awake()
    {
        base.Awake();
        RefreshTime();
    }

    protected override void OnDeath()
    {
        playerManager.gameManager.PlayerLose();
    }

    protected override void OnHealthChange(int newValue, Hit? hit)
    {
        base.OnHealthChange(newValue, hit);
        RefreshTime();
    }

    public void RefreshTime()
    {
        if (timerDisplay == null)
            timerDisplay = FindAnyObjectByType<BarDisplay>(FindObjectsInactive.Include);
        timerDisplay.UpdateRemaining(health, maxHealth);
    }
}
