using UnityEngine;

public class PlayerTimer : MonoBehaviour
{
    public TimerDisplay timerDisplay;

    public float refillTime;
    public float remainingTime;

    private PlayerManager playerManager;

    void Awake()
    {
        playerManager = GetComponent<PlayerManager>();

        RefreshTime();
    }

    void Start()
    {
        timerDisplay = FindFirstObjectByType<TimerDisplay>();
    }

    void Update()
    {
        DecreaseTime(Time.deltaTime);
        timerDisplay.UpdateTimer(remainingTime, refillTime);
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
