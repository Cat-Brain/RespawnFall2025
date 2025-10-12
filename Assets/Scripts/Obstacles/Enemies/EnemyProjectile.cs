using UnityEngine;

public class EnemyProjectile : EnemyHitbox
{
    public float speed;
    
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
        this.enabled = false;
    }
}
