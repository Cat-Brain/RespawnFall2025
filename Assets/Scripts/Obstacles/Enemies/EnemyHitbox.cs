using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyHitbox : MonoBehaviour
{
    public Hit hit;
    public float knockback;
    public string playerTag;

    protected Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (enabled && collision.collider.CompareTag(playerTag) &&
            collision.collider.TryGetComponent(out Health health))
            OnHit(health, collision.collider);
    }

    public void OnTriggerEnter2D(Collider2D hitCollider)
    {
        if (enabled && hitCollider.CompareTag(playerTag) &&
            hitCollider.TryGetComponent(out Health health))
            OnHit(health, hitCollider);
    }

    public virtual void OnHit(Health health, Collider2D collider)
    {
        health.ApplyHit(hit);
        if (collider.attachedRigidbody != null && rb.linearVelocity != Vector2.zero)
            collider.attachedRigidbody.AddForce(rb.linearVelocity.normalized * knockback, ForceMode2D.Impulse);
    }
}
