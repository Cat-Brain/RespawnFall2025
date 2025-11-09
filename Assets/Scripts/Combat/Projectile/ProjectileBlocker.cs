using UnityEngine;

public class ProjectileBlocker : MonoBehaviour
{
    public virtual bool ShouldDestroy(Projectile projectile) { return true; }

    public virtual void OnBlock(Projectile projectile)
    {
        projectile.Destruct();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out Projectile projectile) &&
            projectile.enabled && ShouldDestroy(projectile))
            OnBlock(projectile);
    }
}
