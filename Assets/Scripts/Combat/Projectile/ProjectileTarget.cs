public class ProjectileTarget : ProjectileBlocker
{
    public Health health;

    public override void OnBlock(Projectile projectile)
    {
        health.ApplyHit(projectile.hit);
        base.OnBlock(projectile);
    }
}
