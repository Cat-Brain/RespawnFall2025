using UnityEngine;

public class ProjectileBlocker : MonoBehaviour
{
    public virtual bool ShouldDestroy(ProjectileInst projectile) { return true; }

    public virtual void OnBlock(ProjectileInst projectile)
    {
        projectile.Destruct();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out ProjectileInst projectile) &&
            projectile.enabled && ShouldDestroy(projectile))
            OnBlock(projectile);
    }
}
