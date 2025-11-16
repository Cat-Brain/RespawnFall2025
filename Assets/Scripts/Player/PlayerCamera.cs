using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public PlayerManager playerManager;

    public float moveSpringFrequency, moveSpringDamping;
    public float minHeight, maxHeight;

    public bool resetPosOnAwake;

    [HideInInspector] public float velocity = 0;
    public SpringUtils.tDampedSpringMotionParams moveSpring = new();

    private void Awake()
    {
        if (resetPosOnAwake)
            transform.position = new Vector3(0, DesiredHeight(), transform.position.z);
    }

    void LateUpdate()
    {
        SpringUtils.CalcDampedSpringMotionParams(ref moveSpring, Time.deltaTime, moveSpringFrequency, moveSpringDamping);
        float position = transform.position.y;
        SpringUtils.UpdateDampedSpringMotion(ref position, ref velocity, DesiredHeight(), moveSpring);
        transform.position = CMath.Vector3XZ_Y(CMath.Vector3ToXZ(transform.position), position);
    }

    public float DesiredHeight()
    {
        return Mathf.Clamp(playerManager.transform.position.y, minHeight, maxHeight);
    }
}
