using UnityEngine;

public class EnemyProjectile : EnemyHitbox
{
    public float speed;
    public LayerMask stoppedByMask;
    
    [HideInInspector] public Vector2 direction;

    public void Init()
    {
        if (!rb)
            rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = direction * speed;
    }

    public override void OnHit(PlayerManager playerManager)
    {
        base.OnHit(playerManager);
        Destroy(gameObject);
        enabled = false;
    }

    new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (CMath.LayerOverlapsMask(collision.gameObject.layer, stoppedByMask))
            Destroy(gameObject);
    }
}
