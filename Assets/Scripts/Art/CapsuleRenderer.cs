using DG.Tweening;
using UnityEngine;

public class CapsuleRenderer : MonoBehaviour
{
    public Color color;

    public Tween DOColor(Color color, float duration)
    {
        return DOTween.To(() => this.color, value => this.color = value, color, duration);
    }
}
