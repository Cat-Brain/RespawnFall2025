using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBlock : MonoBehaviour
{
    public float blockTime, blockStunTime, earlyBlockBufferTime, perfectBlockTime;
    private float blockStunTimer = 0, earlyBlockBufferTimer = 0, perfectBlockTimer = 0;

    private PlayerManager playerManager;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerManager.blockAction.action.started += TryBlock;
    }

    public void TryBlock(InputAction.CallbackContext context)
    {
        earlyBlockBufferTimer = earlyBlockBufferTime;
    }

    void Update()
    {
        if (earlyBlockBufferTimer > 0 && blockStunTimer <= 0)
        {
            playerManager.blockInvulnerability += blockTime;
            blockStunTimer = blockStunTime;
            earlyBlockBufferTimer = 0;
            perfectBlockTimer = perfectBlockTime;
            // Insert some sort of particle here or something
        }

        blockStunTimer = Mathf.Max(0, blockStunTimer - Time.deltaTime);
        earlyBlockBufferTimer = Mathf.Max(0, earlyBlockBufferTimer - Time.deltaTime);
        perfectBlockTimer = Mathf.Max(0, perfectBlockTimer - Time.deltaTime);
    }

    public void SuccessfulBlock()
    {
        if (perfectBlockTimer <= 0)
        {
            // Insert perfect block effect
            blockStunTimer = 0;
            earlyBlockBufferTimer = 0;
            return;
        }
        // Insert normal block particle effect
    }
}
