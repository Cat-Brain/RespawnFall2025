using UnityEngine;

public class DeathZoneBottomBarrier : MonoBehaviour
{
    public float height;

    private float startPosition, startHeight;

    void Awake()
    {
        startPosition = transform.position.y;
        startHeight = transform.localScale.y;
    }

    void LateUpdate()
    {
        transform.localScale = CMath.Vector3XZ_Y(
            CMath.Vector3ToXZ(transform.localScale), height + startHeight);
        transform.position = CMath.Vector3XZ_Y(CMath.Vector3ToXZ(
            transform.position), startPosition + height * 0.5f);
    }
}
