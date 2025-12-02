using DG.Tweening;
using UnityEngine;

public class TweenScaleOnTick : OnTickEffect
{
    public Vector2 scale;
    public float tweenTime;

    public override void OnTick(TickEntity tickEntity)
    {
        transform.DOScale(scale, tweenTime);
    }
}
