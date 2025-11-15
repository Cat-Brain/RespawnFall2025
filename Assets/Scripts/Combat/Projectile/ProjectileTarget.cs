using UnityEngine;

public class ProjectileTarget : ProjectileBlocker
{
    public Health health;

    public override bool ShouldDestroy(ProjectileInst projectile)
    {
        return CMath.LayerOverlapsMask(gameObject.layer, projectile.data.targetMask);
    }

    public override void OnBlock(ProjectileInst projectile)
    {
        Hit hit = projectile.data.hit;
        hit.position = projectile.transform.position;
        health.ApplyHit(hit);
        base.OnBlock(projectile);
    }
}
