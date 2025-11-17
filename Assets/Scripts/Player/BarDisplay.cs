using UnityEngine;

public class BarDisplay : MonoBehaviour
{
    public RectTransform harpFadeMask;

    private float harpFadeMaskStartHeight;

    void Awake()
    {
        harpFadeMaskStartHeight = harpFadeMask.sizeDelta.y;
    }

    public void UpdateRemaining(float fraction)
    {
        harpFadeMask.sizeDelta = new Vector2(harpFadeMask.sizeDelta.x, harpFadeMaskStartHeight * fraction);
    }

    public void UpdateRemaining(float remaining, float total)
    {
        UpdateRemaining(remaining / total);
    }
}
