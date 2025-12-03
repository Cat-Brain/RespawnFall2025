using UnityEngine;

public class MossMove : MonoBehaviour
{
    public float radiusMultiplier;
    public float moveFrequency, moveDamping;

    public SpringUtils.tDampedSpringMotionParams moveSpring = new();
    public Vector2 basePos;
    public Vector2 vel = Vector2.zero;

    void Start()
    {
        basePos = transform.position;
    }

    void FixedUpdate()
    {
        SpringUtils.CalcDampedSpringMotionParams(ref moveSpring, Time.deltaTime, moveFrequency, moveDamping);

        Vector2 position = transform.position;
        SpringUtils.UpdateDampedSpringMotion(ref position.x, ref vel.x, basePos.x, moveSpring);
        SpringUtils.UpdateDampedSpringMotion(ref position.y, ref vel.y, basePos.y, moveSpring);

        transform.position = position;
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.enabled)
            return;

        Vector2 nearestPoint = collision.ClosestPoint(transform.position);
        float dist = Vector2.Distance(transform.position, nearestPoint);
        float radius = transform.localScale.x * radiusMultiplier;
        if (dist != 0 && dist < radius)
            transform.position += CMath.Vector3XY_Z(
                (nearestPoint - (Vector2)transform.position) * (dist - radius) / dist, 0);
    }
}
