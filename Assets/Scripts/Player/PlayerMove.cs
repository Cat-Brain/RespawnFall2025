using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float accel, speed;

    public float jumpForce;

    [Tooltip("Time that a player can press the jump button early and still have it register")]
    public float jumpBufferTime;
    [Tooltip("Time that a player can press the jump button late after leaving ground and still jump")]
    public float cayoteTime;
    [Tooltip("Time that a player cannot jump after having done a jump (removes buggy unfun tech)")]
    public float jumpSpamTime;
    private float jumpBufferTimer = 0, cayoteTimer = 0, jumpSpamTimer = 0;

    public PhysicsMaterial2D movePhysicsMaterial, stunnedPhysicsMaterial;

    private Rigidbody2D rb;
    private PlayerManager playerManager;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerManager = GetComponent<PlayerManager>();

        playerManager.jumpAction.action.started += TryJump;
    }

    public void TryJump(InputAction.CallbackContext context)
    {
        if (playerManager.moveStun <= 0)
            jumpBufferTimer = jumpBufferTime;
    }

    private void FixedUpdate()
    {
        if (playerManager.moveStun <= 0 && playerManager.moveStun != -1)
        {
            rb.sharedMaterial = movePhysicsMaterial;

            bool grounded = playerManager.IsGrounded();

            float horizontalInput = playerManager.moveAction.action.ReadValue<float>();

            if (Mathf.Abs(horizontalInput) > 0.1f)
            {
                rb.linearVelocityX = CMath.TryAdd(rb.linearVelocityX, horizontalInput * accel * Time.fixedDeltaTime, speed);
                playerManager.SetDirection(horizontalInput > 0 ? EntityDirection.RIGHT : EntityDirection.LEFT);
            }
            else
                rb.linearVelocityX = CMath.TrySub(rb.linearVelocityX, accel * Time.fixedDeltaTime);

            if (grounded)
                cayoteTimer = cayoteTime;

            if (jumpBufferTimer > 0 && cayoteTimer > 0 && jumpSpamTimer <= 0)
            {
                rb.linearVelocityY = Mathf.Max(0, rb.linearVelocityY) + jumpForce;
                jumpBufferTimer = 0;
                cayoteTimer = 0;
                jumpSpamTimer = jumpSpamTime;
            }
        }
        else
            rb.sharedMaterial = stunnedPhysicsMaterial;

        jumpBufferTimer = Mathf.Max(0, jumpBufferTimer - Time.fixedDeltaTime);
        cayoteTimer = Mathf.Max(0, cayoteTimer - Time.fixedDeltaTime);
        jumpSpamTimer = Mathf.Max(0, jumpSpamTimer - Time.fixedDeltaTime);
    }
}
