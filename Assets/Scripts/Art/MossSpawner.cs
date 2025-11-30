using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class MossSpawner : MonoBehaviour
{
    public Tilemap tilemap;

    public Vector3Int tilePos;
    public bool tileOn, makeUp, makeRight;

    void Update()
    {
        tilePos = tilemap.WorldToCell(transform.position);
        tileOn = tilemap.HasTile(tilePos);
        makeUp = tileOn != tilemap.HasTile(tilePos + Vector3Int.up);
        makeRight = tileOn != tilemap.HasTile(tilePos + Vector3Int.right);
    }
}
