using UnityEngine;

public class ProjectileTarget : ProjectileBlocker
{
    public Health health;

    public override bool ShouldDestroy(Projectile projectile)
    {
        return CMath.LayerOverlapsMask(gameObject.layer, projectile.targetMask);
    }

    public override void OnBlock(Projectile projectile)
    {
        health.ApplyHit(projectile.hit);
        base.OnBlock(projectile);
    }
}
