using DG.Tweening;
using UnityEngine;

public class TweenToTagOnTick : OnTickEffect
{
    public string desiredTag;
    public float tweenTime;

    public override void OnTick(TickEntity tickEntity)
    {
        Transform foundTransform = GameObject.FindWithTag(desiredTag).transform;
        Tweener tweener = transform.DOMove(foundTransform.position, tweenTime);
        
        tweener.OnUpdate(() => tweener.ChangeEndValue(foundTransform.position, tweener.Duration() - tweener.Elapsed(), true));
    }
}
