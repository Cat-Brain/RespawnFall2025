using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBlock : MonoBehaviour
{
    public float blockTime, blockStunTime, earlyBlockBufferTime;
    private float blockStunTimer = 0, earlyBlockBufferTimer = 0;

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
            // Insert some sort of particle here or something
        }

        blockStunTimer = Mathf.Max(0, blockStunTimer - Time.deltaTime);
        earlyBlockBufferTimer = Mathf.Max(0, earlyBlockBufferTimer - Time.deltaTime);
    }
}
