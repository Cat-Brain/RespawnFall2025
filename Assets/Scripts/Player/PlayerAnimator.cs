using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator animator, animator2;

    public bool running, stunned, jumping, grounded, singing;

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
        singing = playerManager.blockInvulnerability > 0;

        animator.SetBool("isRunning", running);
        animator.SetBool("stunned", stunned);
        animator.SetBool("jumping", jumping);
        animator.SetBool("grounded", grounded);
        
        animator2.SetBool("stunned", stunned);
        animator2.SetBool("singing", singing);
    }
}
