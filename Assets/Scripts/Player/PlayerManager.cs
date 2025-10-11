using UnityEngine;
using UnityEngine.InputSystem;

public enum EntityDirection
{
    LEFT, RIGHT
}

public enum HitResult
{
    HIT, ABSORBED, BLOCKED
}

public class PlayerManager : MonoBehaviour
{
    public InputActionReference moveAction, jumpAction, blockAction;

    public float groundedRadius, groundedDistance;
    public LayerMask groundedMask;

    public Vector2 recoilMultiplier;

    public EntityDirection direction;

    /*[HideInInspector]*/ public float blockInvulnerability = 0, stunInvulnerability = 0, moveStun = 0; // Applies to both horizontal movement and jumping

    [HideInInspector] public PlayerMove playerMove;
    [HideInInspector] public PlayerGravity playerGravity;
    [HideInInspector] public PlayerBlock playerBlock;

    private Rigidbody2D rb;

    void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        playerGravity = GetComponent<PlayerGravity>();
        playerBlock = GetComponent<PlayerBlock>();

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        blockInvulnerability = Mathf.Max(0, blockInvulnerability - Time.deltaTime);
        stunInvulnerability = Mathf.Max(0, stunInvulnerability - Time.deltaTime);
        if (moveStun != -1)
            moveStun = Mathf.Max(0, moveStun - Time.deltaTime);
    }

    public bool IsGrounded()
    {
        int tempLayer = gameObject.layer;
        gameObject.layer = 2; // Ignore Raycasts

        bool successful = Physics2D.CircleCast(transform.position, groundedRadius, Vector2.down, groundedDistance, groundedMask);

        gameObject.layer = tempLayer;

        return successful;
    }

    public HitResult TryHit(float duration, float invulnerability)
    {
        if (blockInvulnerability > 0)
            return HitResult.BLOCKED;
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
        direction.y = Mathf.Max(0, direction.y);

        rb.linearVelocity += direction * recoilMultiplier * force;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded() ? Color.green : Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + Vector2.down * groundedDistance, groundedRadius);
    }
}
