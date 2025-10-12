using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EnemyHitbox))]
public class HopperEnemy : MonoBehaviour
{
    public FlipToDirection flip;

    public GameObject projectilePrefab;

    public float hopDist, hopHeight;
    public float floorTestOffset;
    public float validJumpTestRadius;
    public float jumpCooldownTime, projectileFireTime, projectileCooldownTime;
    public float projectileRange, projectileRadius;
    public LayerMask platformMask, wallMask;

    private bool facingLeft;
    private float jumpTimer = 0, jumpCooldownTimer = 0,
        projectileFireTimer = 0, projectileCooldownTimer = 0;

    private Rigidbody2D rb;
    private EnemyHitbox hitbox;

    private Transform player;

    private Vector2 jumpStartPos;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<EnemyHitbox>();

        player = FindFirstObjectByType<PlayerManager>().transform;

       if (Random.Range(0, 2) == 0)
            facingLeft = true;
    }

    void FixedUpdate()
    {
        if (jumpTimer > 0 && jumpTimer < Time.fixedDeltaTime)
        {
            jumpCooldownTimer = jumpCooldownTime;
            Reorient();
        }
        jumpTimer = Mathf.Max(0, jumpTimer - Time.fixedDeltaTime);
        jumpCooldownTimer = Mathf.Max(0, jumpCooldownTimer - Time.fixedDeltaTime);
        projectileFireTimer = Mathf.Max(0, projectileFireTimer - Time.fixedDeltaTime);
        projectileCooldownTimer = Mathf.Max(0, projectileCooldownTimer - Time.fixedDeltaTime);

        if (jumpTimer > 0 || jumpCooldownTimer > 0 || projectileFireTimer > 0)
            return;

        if (TryShootPlayer())
            return;

        ActivateJump();
    }

    public bool Reorient()
    {
        if (Physics2D.OverlapCircle(FloorTestPos(), validJumpTestRadius, platformMask) &&
            !Physics2D.OverlapCircle(WallTestPos(), validJumpTestRadius, platformMask))
            return false;

        SetDir(!facingLeft);
        return true;
    }

    public bool TryShootPlayer()
    {
        Vector2 offset = (player.position - transform.position);
        if (offset == Vector2.zero)
            return false;
        float distance = offset.magnitude;
        Vector2 direction = offset / distance;
        if (projectileCooldownTimer > 0 ||
            offset.sqrMagnitude > projectileRange * projectileRange ||
            Physics2D.CircleCast(transform.position, projectileRadius, direction, distance, wallMask))
            return false;

        projectileFireTimer = projectileFireTime;
        projectileCooldownTimer = projectileCooldownTime;
        EnemyProjectile projectile = Instantiate(projectilePrefab, transform.position,
            Quaternion.identity).GetComponent<EnemyProjectile>();

        projectile.direction = direction;
        projectile.Init();

        return true;
    }

    public Vector2 WallTestPos()
    {
        Vector2 offset = hopDist * Vector2.right;
        if (facingLeft)
            offset.x *= -1;
        return offset + (Vector2)transform.position;
    }

    public Vector2 FloorTestPos()
    {
        Vector2 offset = hopDist * Vector2.right;
        if (facingLeft)
            offset.x *= -1;
        offset.y -= floorTestOffset;
        return offset + (Vector2)transform.position;
    }

    public float JumpDuration()
    {
        return Mathf.Sqrt(-8 * hopHeight / Physics2D.gravity.y);
    }

    public float JumpForce()
    {
        return Mathf.Sqrt(-2 * hopHeight * Physics2D.gravity.y);
    }

    public Vector2 JumpVelocity()
    {
        return new Vector2((facingLeft ? -1 : 1) * hopDist / JumpDuration(), JumpForce());
    }

    public void ActivateJump()
    {
        jumpTimer = JumpDuration();
        Reorient();
        rb.linearVelocity = JumpVelocity();
    }

    public void SetDir(bool left)
    {
        if (left)
        {
            facingLeft = true;
            flip.direction = EntityDirection.LEFT;
            return;
        }
        facingLeft = false;
        flip.direction = EntityDirection.RIGHT;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(WallTestPos(),
            validJumpTestRadius);
        Gizmos.DrawWireSphere(FloorTestPos(),
            validJumpTestRadius);
        Gizmos.DrawWireSphere(transform.position, projectileRange);
    }
}
