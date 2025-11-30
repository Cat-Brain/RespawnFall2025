using UnityEngine;

public class GrassSpawner : MonoBehaviour
{
    public GameObject grassPrefab;

    public float minOffset, maxOffset;
    public float grassHeight, grassDensity,
        grassIndentHeight, grassIndentWidth;

    void Awake()
    {
        for (float x = minOffset; x <= maxOffset; x += 1 / grassDensity)
            Instantiate(grassPrefab, transform.position + Vector3.right * x, Quaternion.identity, transform).
                transform.localScale = new Vector3(1,
                grassHeight - (grassHeight - grassIndentHeight) * Mathf.Exp(-x * x / grassIndentWidth), 1);
    }
}
