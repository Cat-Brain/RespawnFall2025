using UnityEngine;

public class DeathZoneMove : MonoBehaviour
{
    public DeathZoneBottomBarrier barrier;

    public float additionalDeadlyHeight;

    public float riseSpeed;
    public float initialRiseStun;

    public float riseStun, risenHeight;

    private float initialHeight;

    private GameManager gameManager;

    void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        gameManager.deathZone = this;

        riseStun = initialRiseStun;
        initialHeight = transform.position.y;
    }

    void Update()
    {
        riseStun = Mathf.Max(0, riseStun - Time.deltaTime);

        if (riseStun > 0)
            return;

        risenHeight += riseSpeed * Time.deltaTime;
        gameManager.playerManager.playerCamera.minHeight = risenHeight;

        transform.position = CMath.Vector3XZ_Y(
            CMath.Vector3ToXZ(transform.position),
            initialHeight + risenHeight);

        barrier.height = risenHeight;
    }

    public float DeadlyHeight()
    {
        return risenHeight + additionalDeadlyHeight;
    }

    public void TryAddStun(float stunAmount, float maxStun)
    {
        if (riseStun >= maxStun)
            return;

        riseStun = Mathf.Min(maxStun, riseStun + stunAmount);
    }
}
