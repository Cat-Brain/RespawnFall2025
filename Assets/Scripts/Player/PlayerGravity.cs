using UnityEngine;

public class PlayerGravity : MonoBehaviour
{
    public float gravity, tapJumpGravity;

    private PlayerManager playerManager;
    private Rigidbody2D rb;

    void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.linearVelocityY -= (rb.linearVelocityY <= 0 || playerManager.jumpAction.action.inProgress ?
            gravity : tapJumpGravity) * Time.fixedDeltaTime;
    }
}
