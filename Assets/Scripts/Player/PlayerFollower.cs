using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFollower : MonoBehaviour
{
    public PlayerManager playerManager;

    public float followDistance;
    public float followSpringFrequency, followSpringDamping;

    public SpringUtils.tDampedSpringMotionParams followSpring = new();
    private Vector2 velocity = Vector2.zero;

    void Update()
    {
        SpringUtils.CalcDampedSpringMotionParams(ref followSpring, Time.deltaTime, followSpringFrequency, followSpringDamping);

        Vector2 currentPos = transform.position, desiredPos = Vector2.right * followDistance;
        if (playerManager.direction == EntityDirection.RIGHT)
            desiredPos.x *= -1;
        desiredPos += (Vector2)playerManager.transform.position;

        SpringUtils.UpdateDampedSpringMotion(ref currentPos.x, ref velocity.x, desiredPos.x, followSpring);
        SpringUtils.UpdateDampedSpringMotion(ref currentPos.y, ref velocity.y, desiredPos.y, followSpring);

        transform.position = CMath.Vector3XY_Z(currentPos, transform.position.z);
    }
}
