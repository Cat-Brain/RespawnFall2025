using UnityEngine;

public class DeathZoneBottomBarrier : MonoBehaviour
{
    public float height;

    private float startHeight;

    void Awake()
    {
        startHeight = transform.position.y;
    }

    void LateUpdate()
    {
        transform.localScale = CMath.Vector3XZ_Y(
            CMath.Vector3ToXZ(transform.localScale), height);
        transform.position = CMath.Vector3XZ_Y(CMath.Vector3ToXZ(
            transform.position), startHeight + height * 0.5f);
    }
}
