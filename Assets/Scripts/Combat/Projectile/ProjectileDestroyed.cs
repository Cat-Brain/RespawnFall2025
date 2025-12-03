using DG.Tweening;
using UnityEngine;

public class ProjectileDestroyed : ProjectileBlocker
{
    public float destructTime;

    public override bool ShouldDestroy(ProjectileInst projectile)
    {
        return CMath.LayerOverlapsMask(gameObject.layer, projectile.data.targetMask);
    }

    public override void OnBlock(ProjectileInst projectile)
    {
        transform.DOScale(0, destructTime).OnComplete(() => Destroy(gameObject, destructTime));
        base.OnBlock(projectile);
        enabled = false;
    }
}
