using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EnemyHitbox))]
public class HopperEnemy : MonoBehaviour
{
    public float hopDist, hopHeight;
    public Vector2 validJumpTestOffset;
    public float validJumpTestRadius;
    //public float jump

    private bool facingLeft;

    private Rigidbody2D rb;
    private EnemyHitbox hitbox;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<EnemyHitbox>();

       if (Random.Range(0, 2) == 0)
            facingLeft = true;
    }

    void Update()
    {
        
    }

    public Vector2 CurrentTestJumpPos()
    {
        Vector2 offset = hopDist * Vector2.right;
        if (facingLeft)
            offset.x *= -1;
        return offset += (Vector2)transform.position;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(CurrentTestJumpPos(),
            validJumpTestRadius);
    }
}
