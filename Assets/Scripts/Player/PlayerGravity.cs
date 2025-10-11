using UnityEngine;

public class PlayerGravity : MonoBehaviour
{
    public float gravity, tapJumpGravity;

    public bool inQuickFall = false;

    private PlayerManager playerManager;
    private Rigidbody2D rb;

    void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (inQuickFall && (rb.linearVelocityY <= 0 || playerManager.Stunned() ||
            playerManager.jumpAction.action.inProgress))
            inQuickFall = false;
        rb.linearVelocityY -= (inQuickFall ? tapJumpGravity : gravity) * Time.fixedDeltaTime;
    }
}
