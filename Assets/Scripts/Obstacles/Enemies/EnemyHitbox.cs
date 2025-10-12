using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyHitbox : MonoBehaviour
{
    public float knockback, stunDuration, invulnerability;
    public string playerTag;

    protected Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (enabled && collision.collider.CompareTag(playerTag) &&
            collision.collider.TryGetComponent(out PlayerManager playerManager))
            OnHit(playerManager);   
    }

    public void OnTriggerEnter2D(Collider2D hitCollider)
    {
        if (enabled && hitCollider.CompareTag(playerTag) &&
            hitCollider.TryGetComponent(out PlayerManager playerManager))
            OnHit(playerManager);
    }

    public virtual void OnHit(PlayerManager playerManager)
    {
        playerManager.TryHit(rb.linearVelocity.normalized * knockback, stunDuration, invulnerability);
    }
}
