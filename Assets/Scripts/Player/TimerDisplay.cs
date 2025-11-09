using UnityEngine;

public class TimerDisplay : MonoBehaviour
{
    public RectTransform harpFadeMask;

    private float harpFadeMaskStartHeight;

    void Awake()
    {
        harpFadeMaskStartHeight = harpFadeMask.sizeDelta.y;
    }

    public void UpdateTimer(float remaining, float total)
    {
        float remainingFraction = remaining / total;
        harpFadeMask.sizeDelta = new Vector2(harpFadeMask.sizeDelta.x, harpFadeMaskStartHeight * remainingFraction);
    }
}
