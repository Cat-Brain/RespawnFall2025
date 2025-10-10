using UnityEngine;

public class PlayerTimer : MonoBehaviour
{
    public GameManager gameManager;
    public TimerDisplay timerDisplay;

    public float refillTime;
    public float remainingTime;

    void Awake()
    {
        remainingTime = refillTime;
    }

    void Update()
    {
        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0)
        {
            remainingTime = 0;
            gameManager.PlayerLose();
        }

        timerDisplay.UpdateTimer(remainingTime, refillTime);
    }
}
