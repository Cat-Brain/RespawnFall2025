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

    void Start()
    {
        SetRotation(DesiredRotation());
    }

    void LateUpdate()
    {
        SpringUtils.CalcDampedSpringMotionParams(ref flipSpring, Time.deltaTime, flipSpringFrequency, flipSpringDamping);

        float baseRotation = transform.localEulerAngles.y;
        float rotation = CMath.Mod(baseRotation + 90, 360) - 90; // -90 - 270

        SpringUtils.UpdateDampedSpringMotion(ref rotation, ref velocity, DesiredRotation(), flipSpring);

        SetRotation(rotation);
    }

    public void SetRotation(float rotation)
    {
        transform.localEulerAngles = CMath.Vector3XZ_Y(CMath.Vector3ToXZ(transform.localEulerAngles), rotation);
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
