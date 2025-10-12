using UnityEngine;

public class ExplosiveFlower : EnemyHealth
{
    public float explosionRadius, explosionForce;
    public LayerMask explosionMask;
    public AudioClip flowerPopClip;

    public override void Death()
    {
        Debug.Log("Test");
        // Implement effect here!
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(
            transform.position, explosionRadius, explosionMask))
            if (collider.attachedRigidbody)
                collider.attachedRigidbody.linearVelocity +=
                    ((Vector2)(collider.transform.position - transform.position)).normalized
                    * explosionForce;

        base.Death();
        AudioManager.instance.PlaySoundFXClip(flowerPopClip, transform, 0.2f);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
