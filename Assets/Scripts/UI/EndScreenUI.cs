using TMPro;
using UnityEngine;

public class EndScreenUI : MonoBehaviour
{
    public GameManager gameManager;

    public TextMeshProUGUI runResultText, difficultyText, timeText, pointsText;

    public string baseRunResultText, baseDifficultyText, baseTimeText, basePointsText;

    public void Awake()
    {
        baseRunResultText = runResultText.text;
        baseDifficultyText = difficultyText.text;
        baseTimeText = timeText.text;
        basePointsText = pointsText.text;
    }

    public void UpdateEndScreen(string result)
    {
        runResultText.text = baseRunResultText + result;
        difficultyText.text = baseDifficultyText + gameManager.difficulty.ToString();
        float totalTime = gameManager.endTime - gameManager.startTime;
        timeText.text = baseTimeText + (totalTime < 60 ? totalTime.ToString("0.00") :
            ((int)totalTime / 60).ToString() + " : " + CMath.Mod(totalTime, 60).ToString("0.00"));
        pointsText.text = basePointsText + gameManager.points.ToString();
    }

    public void OnDeath()
    {
        UpdateEndScreen("Failure");
    }

    public void OnWin()
    {
        UpdateEndScreen("Victorious");
    }

    public void OnRestart()
    {
        UpdateEndScreen("Restarted");
    }
}
