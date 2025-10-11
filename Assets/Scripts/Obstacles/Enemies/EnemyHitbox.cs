using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyHitbox : MonoBehaviour
{
    public float knockback, stunDuration, invulnerability;
    public string playerTag;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D hitCollider)
    {
        if (enabled && hitCollider.CompareTag(playerTag) && hitCollider.TryGetComponent(out PlayerManager playerManager))
            playerManager.TryHit(rb.linearVelocity.normalized * knockback, stunDuration, invulnerability);
    }
}
