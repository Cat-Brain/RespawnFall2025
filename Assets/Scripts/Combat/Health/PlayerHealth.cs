using DG.Tweening.Core.Easing;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerHealth", menuName = "Health/PlayerHealth")]
public class PlayerHealth : Health
{
    [HideInInspector] public BarDisplay healthDisplay;
    [HideInInspector] public PlayerManager playerManager;

    public override void Init()
    {
        base.Init();
        RefreshHealth(inst.health);
    }

    public override void OnDeath()
    {
        if (playerManager == null)
            playerManager = inst.GetComponent<PlayerManager>();
        playerManager.gameManager.PlayerLose();
    }

    public override void OnHealthChange(int newValue, Hit? hit)
    {
        RefreshHealth(newValue);
    }

    public void RefreshHealth(int currentHealth)
    {
        if (healthDisplay == null)
            healthDisplay = FindAnyObjectByType<BarDisplay>(FindObjectsInactive.Include);
        if (healthDisplay)
            healthDisplay.UpdateRemaining(currentHealth, maxHealth);
    }
}
