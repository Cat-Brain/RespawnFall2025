using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public PlayerManager playerManager;

    public float rotationMultiplier, maxRotation;
    public float rotationSpringFrequency, rotationSpringDamping;
    public float moveSpringFrequency, moveSpringDamping;

    public float angularVelocity = 0;
    [HideInInspector] public Vector2 velocity = Vector2.zero;
    public float lastXVelocity = 0;
    public SpringUtils.tDampedSpringMotionParams rotationSpring = new(), moveSpring = new();

    void LateUpdate()
    {
        SpringUtils.CalcDampedSpringMotionParams(ref rotationSpring, Time.deltaTime, rotationSpringFrequency, rotationSpringDamping);
        float rotation = CMath.Rot0To360IntoN180To180(transform.eulerAngles.z);
        SpringUtils.UpdateDampedSpringMotion(ref rotation, ref angularVelocity, 0, rotationSpring);
        transform.eulerAngles = new Vector3(0, 0, Mathf.Clamp(rotation, -maxRotation, maxRotation));

        SpringUtils.CalcDampedSpringMotionParams(ref moveSpring, Time.deltaTime, moveSpringFrequency, moveSpringDamping);
        Vector2 position = transform.position;
        SpringUtils.UpdateDampedSpringMotion(ref position.x, ref velocity.x, playerManager.transform.position.x, moveSpring);
        SpringUtils.UpdateDampedSpringMotion(ref position.y, ref velocity.y, playerManager.transform.position.y, moveSpring);
        transform.position = CMath.Vector3XY_Z(position, transform.position.z);
    }

    void FixedUpdate()
    {
        angularVelocity += rotationMultiplier * (playerManager.rb.linearVelocityX - lastXVelocity);
        lastXVelocity = playerManager.rb.linearVelocityX;
    }
}
