using UnityEngine;

public enum EntityDirection
{
    LEFT, RIGHT, NEUTRAL
}

public class FlipToDirection : MonoBehaviour
{
    public float flipSpringFrequency, flipSpringDamping;
    public EntityDirection direction;

    public SpringUtils.tDampedSpringMotionParams flipSpring = new();
    private float velocity = 0;

    void LateUpdate()
    {
        SpringUtils.CalcDampedSpringMotionParams(ref flipSpring, Time.deltaTime, flipSpringFrequency, flipSpringDamping);

        float baseRotation = transform.eulerAngles.y;
        float rotation = CMath.Mod(baseRotation + 90, 360) - 90; // -90 - 270

        SpringUtils.UpdateDampedSpringMotion(ref rotation, ref velocity, DesiredRotation(), flipSpring);

        transform.eulerAngles = CMath.Vector3XZ_Y(CMath.Vector3ToXZ(transform.eulerAngles), rotation);
    }

    public float DesiredRotation()
    {
        return DirectionToRotation(direction);
    }

    public static float DirectionToRotation(EntityDirection direction)
    {
        return direction == EntityDirection.LEFT ? 180 : direction == EntityDirection.NEUTRAL ? 90 : 0;
    }
}
