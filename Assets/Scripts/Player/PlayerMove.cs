using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float accel, speed;

    public float jumpForce;
    public int airJumps;

    [Tooltip("Time that a player can press the jump button early and still have it register")]
    public float jumpBufferTime;
    [Tooltip("Time that a player can press the jump button late after leaving ground and still jump")]
    public float cayoteTime;
    [Tooltip("Time that a player cannot jump after having done a jump (removes buggy unfun tech)")]
    public float jumpSpamTime;
    private float jumpBufferTimer = 0, cayoteTimer = 0, jumpSpamTimer = 0;
    public float tapJumpTimer = 0;
    public int remainingAirJumps;

    public PhysicsMaterial2D movePhysicsMaterial, stunnedPhysicsMaterial;

    private Rigidbody2D rb;
    private PlayerManager playerManager;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerManager = gameObject.GetComponent<PlayerManager>();
        //animator = GetComponent<Animator>();

        playerManager.jumpAction.action.started += TryJump;
        playerManager.jumpAction.action.canceled += TryTapJump;
    }

    public void TryJump(InputAction.CallbackContext context)
    {
        if (!playerManager.Stunned())
            jumpBufferTimer = jumpBufferTime;
    }

    public void TryTapJump(InputAction.CallbackContext context)
    {
        if (tapJumpTimer > 0)
            playerManager.playerGravity.inQuickFall = true;
    }

    private void FixedUpdate()
    {
        if (!playerManager.Stunned())
        {
            rb.sharedMaterial = movePhysicsMaterial;

            bool grounded = playerManager.IsGrounded();

            if (grounded)
                remainingAirJumps = airJumps;

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

            if (jumpBufferTimer > 0 && (cayoteTimer > 0 || remainingAirJumps > 0) && jumpSpamTimer <= 0)
            {
                rb.linearVelocityY = Mathf.Max(rb.linearVelocityY, jumpForce);
                //AnimationManager.instance.PlayAnimation(animClips[1]);

                if (cayoteTimer > 0)
                {
                    cayoteTimer = 0;
                    remainingAirJumps = airJumps;
                }
                else
                    remainingAirJumps--;

                jumpBufferTimer = 0;
                jumpSpamTimer = jumpSpamTime;
                tapJumpTimer = jumpForce / playerManager.playerGravity.gravity;
            }
        }
        else
            rb.sharedMaterial = stunnedPhysicsMaterial;

        jumpBufferTimer = Mathf.Max(0, jumpBufferTimer - Time.fixedDeltaTime);
        cayoteTimer = Mathf.Max(0, cayoteTimer - Time.fixedDeltaTime);
        jumpSpamTimer = Mathf.Max(0, jumpSpamTimer - Time.fixedDeltaTime);
        tapJumpTimer = Mathf.Max(0, tapJumpTimer - Time.fixedDeltaTime);
        if (rb.linearVelocityY <= 0)
            tapJumpTimer = 0;
    }
}
