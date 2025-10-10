using TMPro;
using UnityEngine;

public class TimerDisplay : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    public void UpdateTimer(float remaining, float total)
    {
        timerText.text = Mathf.FloorToInt(remaining).ToString();
    }
}
