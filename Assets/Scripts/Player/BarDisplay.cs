using UnityEngine;

public class BarDisplay : MonoBehaviour
{
    public RectTransform rectTransform;

    public Vector2 baseDim;

    public void UpdateRemaining(float fraction)
    {
        rectTransform.sizeDelta = new Vector2(Mathf.Ceil(fraction * baseDim.x), baseDim.y);
    }

    public void UpdateRemaining(float remaining, float total)
    {
        UpdateRemaining(remaining / total);
    }
}
