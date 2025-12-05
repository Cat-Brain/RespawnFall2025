using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EnemySpawnPosition : MonoBehaviour
{
    public List<Enemy> validSpawns;

    public Vector2 offset;
    public Color debugRenderColor;

    public Vector2Int gridPos;

    public void Awake()
    {
        gridPos = FindGridPos();
    }

    public void Update()
    {
        if (!Application.isPlaying)
            gridPos = FindGridPos();
    }

    public Vector2Int FindGridPos()
    {
        return Vector2Int.RoundToInt((Vector2)transform.localPosition + offset);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = debugRenderColor;
        Vector3 basePos = transform.position + (Vector3)offset;
        Gizmos.DrawLine(basePos, basePos + CMath.V3110);
        Gizmos.DrawLine(basePos + Vector3.up, basePos + Vector3.right);
    }
}
