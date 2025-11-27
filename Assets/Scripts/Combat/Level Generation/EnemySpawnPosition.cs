using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EnemySpawnPosition : MonoBehaviour
{
    public List<Enemy> validSpawns;

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
        return Vector2Int.RoundToInt(transform.position);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = debugRenderColor;
        Gizmos.DrawLine(transform.position, transform.position + CMath.V3110);
        Gizmos.DrawLine(transform.position + Vector3.up, transform.position + Vector3.right);
    }
}
