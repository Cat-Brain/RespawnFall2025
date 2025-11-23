using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
public class PlayerAnimator : MonoBehaviour
{
    public Animator animator, animator2;

    public bool running, stunned, jumping, grounded;

    public PlayerManager playerManager;

    void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    void Update()
    {
        running = playerManager.moveAction.action.inProgress;
        stunned = playerManager.Stunned();
        jumping = playerManager.playerMove.tapJumpTimer > 0;
        grounded = playerManager.IsGrounded();

        animator.SetBool("isRunning", running);
        animator.SetBool("stunned", stunned);
        animator.SetBool("jumping", jumping);
        animator.SetBool("grounded", grounded);
        
        animator2.SetBool("stunned", stunned);
    }
}
