using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public InputActionReference moveAction, jumpAction;

    public float groundedRadius, groundedDistance;
    public LayerMask groundedMask;

    public Vector2 recoilMultiplier;

    [HideInInspector] public float stunInvulnerability, moveStun; // Applies to both horizontal movement and jumping

    [HideInInspector] public PlayerMove playerMove;
    [HideInInspector] public PlayerGravity playerGravity;

    private Rigidbody2D rb;

    void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        playerGravity = GetComponent<PlayerGravity>();

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        stunInvulnerability = Mathf.Max(0, stunInvulnerability - Time.deltaTime);
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
    public void ApplyRecoil(Vector2 direction)
    {
        direction = direction.normalized;
        direction.y = Mathf.Min(0, direction.y);

        rb.linearVelocity += direction * recoilMultiplier;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded() ? Color.green : Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + Vector2.down * groundedDistance, groundedRadius);
    }
}
