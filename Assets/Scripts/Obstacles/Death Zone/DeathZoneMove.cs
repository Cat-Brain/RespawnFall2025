using UnityEngine;

public class DeathZoneMove : MonoBehaviour
{
    public DeathZoneBottomBarrier barrier;

    public float riseSpeed;
    public float initialRiseStun;

    public float riseStun, risenHeight;

    private float initialHeight;

    void Awake()
    {
        riseStun = initialRiseStun;
        initialHeight = transform.position.y;
    }

    void Update()
    {
        riseStun = Mathf.Max(0, riseStun - Time.deltaTime);

        if (riseStun > 0)
            return;

        risenHeight += riseSpeed * Time.deltaTime;

        transform.position = CMath.Vector3XZ_Y(
            CMath.Vector3ToXZ(transform.position),
            initialHeight + risenHeight);

        barrier.height = risenHeight;
    }
}
