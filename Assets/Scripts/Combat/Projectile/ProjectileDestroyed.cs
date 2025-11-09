using DG.Tweening;
using UnityEngine;

public class ProjectileDestroyed : ProjectileBlocker
{
    public float destructTime;

    public override bool ShouldDestroy(Projectile projectile)
    {
        return CMath.LayerOverlapsMask(gameObject.layer, projectile.targetMask);
    }

    public override void OnBlock(Projectile projectile)
    {
        transform.DOScale(0, destructTime).OnComplete(() => Destroy(gameObject, destructTime));
        base.OnBlock(projectile);
        enabled = false;
    }
}
