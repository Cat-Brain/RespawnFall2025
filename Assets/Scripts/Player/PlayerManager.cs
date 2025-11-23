using UnityEngine;
using UnityEngine.InputSystem;

public enum HitResult
{
    HIT, ABSORBED, BLOCKED
}

[RequireComponent(typeof(PlayerMove))]
[RequireComponent(typeof(PlayerGravity))]
[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerManager : MonoBehaviour
{
    public PlayerCamera playerCamera;
    public FlipToDirection playerFlip, followerFlip;

    public InputActionReference moveAction, jumpAction;

    public float groundedRadius, groundedDistance;
    public LayerMask groundedMask;

    public Vector2 recoilMultiplier;

    public EntityDirection direction;

    public string winZoneTag;

    [HideInInspector] public float stunInvulnerability = 0, moveStun = 0; // Applies to both horizontal movement and jumping
    
    [HideInInspector] public GameManager gameManager;

    [HideInInspector] public PlayerMove playerMove;
    [HideInInspector] public PlayerGravity playerGravity;
    [HideInInspector] public PlayerAnimator playerAnimator;

    private Rigidbody2D rb;

    void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        gameManager.playerManager = this;

        playerMove = GetComponent<PlayerMove>();
        playerGravity = GetComponent<PlayerGravity>();
        playerAnimator = GetComponent<PlayerAnimator>();

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        stunInvulnerability = Mathf.Max(0, stunInvulnerability - Time.deltaTime);
        if (moveStun != -1)
            moveStun = Mathf.Max(0, moveStun - Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.CompareTag(winZoneTag))
            gameManager.PlayerWin();
    }

    public bool IsGrounded()
    {
        int tempLayer = gameObject.layer;
        gameObject.layer = 2; // Ignore Raycasts

        bool successful = Physics2D.CircleCast(transform.position, groundedRadius, Vector2.down, groundedDistance, groundedMask);

        gameObject.layer = tempLayer;

        return successful;
    }

    public HitResult TryHit(Vector2 knockbackForce, float duration, float invulnerability)
    {
        rb.linearVelocity += knockbackForce;

        if (stunInvulnerability > 0)
            return HitResult.ABSORBED;

        stunInvulnerability = invulnerability;
        moveStun += duration;
        return HitResult.HIT;
    }

    // If the player does not have i frames then stun them and returns true. Else returns false
    public bool TryAddStun(float duration, float invulnerability)
    {
        if (stunInvulnerability > 0)
            return false;

        stunInvulnerability = invulnerability;
        moveStun += duration;
        return true;
    }

    // The direction should be pointing where the player is being pushed. This will be the inverse of the direction aimed at
    public void ApplyRecoil(Vector2 direction, float force)
    {
        direction = direction.normalized;
        rb.linearVelocityX += direction.x * force * recoilMultiplier.y;
        if (direction.y > 0)
        {
            float recoilVelocity = direction.y * force * recoilMultiplier.y;
            if (recoilVelocity < rb.linearVelocityY)
                return;
            rb.linearVelocityY = recoilVelocity;
            playerMove.tapJumpTimer = 0;
        }
    }

    public void SetDirection(EntityDirection direction)
    {
        this.direction = direction;
        playerFlip.direction = direction;
        followerFlip.direction = direction;
    }

    public bool Stunned()
    {
        return moveStun > 0 || moveStun == -1;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded() ? Color.green : Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + Vector2.down * groundedDistance, groundedRadius);
    }
}
