using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public InputActionReference moveAction, jumpAction;

    public float groundedRadius, groundedDistance;
    public LayerMask groundedMask;

    [HideInInspector] public PlayerMove playerMove;
    [HideInInspector] public PlayerGravity playerGravity;

    void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        playerGravity = GetComponent<PlayerGravity>();
    }
    public bool IsGrounded()
    {
        int tempLayer = gameObject.layer;
        gameObject.layer = 2; // Ignore Raycasts

        bool successful = Physics2D.CircleCast(transform.position, groundedRadius, Vector2.down, groundedDistance, groundedMask);

        gameObject.layer = tempLayer;

        return successful;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded() ? Color.green : Color.red;
        CGizmos.DrawCircle((Vector2)transform.position + Vector2.down * groundedDistance, groundedRadius);
    }
}
