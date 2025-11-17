using UnityEngine;

public class PlayerTimer : MonoBehaviour
{
    public BarDisplay timerDisplay;

    public float refillTime;
    public float remainingTime;

    private PlayerManager playerManager;

    void Awake()
    {
        playerManager = GetComponent<PlayerManager>();

        RefreshTime();
    }

    void Update()
    {
        DecreaseTime(Time.deltaTime);
        if (timerDisplay == null)
            timerDisplay = FindFirstObjectByType<BarDisplay>();
        timerDisplay.UpdateRemaining(remainingTime, refillTime);
    }

    public void RefreshTime()
    {
        remainingTime = refillTime;
    }

    public void DecreaseTime(float amount)
    {
        remainingTime -= amount;
        if (remainingTime <= 0)
        {
            remainingTime = 0;
            playerManager.gameManager.PlayerLose();
        }
    }
}
