using UnityEngine;

[RequireComponent(typeof(HealthInst))]
public class ProjectileTarget : ProjectileBlocker
{
    [HideInInspector] public HealthInst health;

    public void Awake()
    {
        health = GetComponent<HealthInst>();
    }

    public override bool ShouldDestroy(ProjectileInst projectile)
    {
        return CMath.LayerOverlapsMask(gameObject.layer, projectile.data.targetMask);
    }

    public override void OnBlock(ProjectileInst projectile)
    {
        SFXManager.Instance.Play(SFXManager.Instance.hitIce);
        Hit hit = projectile.data.hit;
        hit.position = projectile.transform.position;
        health.ApplyHit(hit);
        base.OnBlock(projectile);
    }
}
