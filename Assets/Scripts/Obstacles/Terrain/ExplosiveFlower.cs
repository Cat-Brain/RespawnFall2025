using UnityEngine;

public class ExplosiveFlower : ProjectileDestroyed
{
    public float explosionRadius, explosionForce;
    [Range(0, 1)] public float verticalBoostCorrection;
    public LayerMask explosionMask;
    public AudioClip flowerPopClip;

    public override void OnBlock(ProjectileInst projectile)
    {
        // Implement effect here!
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(
            transform.position, explosionRadius, explosionMask))
            if (collider.attachedRigidbody)
            {
                Vector2 dir = ((Vector2)(collider.transform.position - transform.position)).normalized;
                if (collider.transform.position == transform.position || (dir.y > 0 && Mathf.Abs(dir.x) < (1 - verticalBoostCorrection)))
                    dir = Vector2.up;
                collider.attachedRigidbody.linearVelocity = dir * explosionForce;
                if (collider.TryGetComponent(out PlayerManager playerManager))
                {
                    playerManager.playerMove.tapJumpTimer = 0;
                    playerManager.playerGravity.inQuickFall = false;
                }
            }

        AudioManager.instance.PlaySoundFXClip(flowerPopClip, transform, 0.2f);
        base.OnBlock(projectile);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
