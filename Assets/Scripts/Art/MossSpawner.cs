using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class MossSpawner : MonoBehaviour
{
    public GameObject mossPrefab;

    public Tilemap tilemap;

    public int mossPerEdge;
    public float mossMaxRadius, mossMinRadius;

    [HideInInspector] public bool tileOn, makeUp, makeRight, makeDown, makeLeft;

    void Awake()
    {
        if (!Application.isPlaying)
            return;

        makeUp = CheckPos(transform.position + transform.up);
        makeRight = CheckPos(transform.position + transform.right);
        makeDown = CheckPos(transform.position - transform.up);
        makeLeft = CheckPos(transform.position - transform.right);

        if (makeUp)
            SpawnMossDir(transform.up);
        if (makeRight)
            SpawnMossDir(transform.right);
        if (makeDown)
            SpawnMossDir(-transform.up);
        if (makeLeft)
            SpawnMossDir(-transform.right);
    }

    public bool CheckPos(Vector2 pos)
    {
        Collider2D[] overlappedColliders = Physics2D.OverlapPointAll(pos);
        foreach (Collider2D col in overlappedColliders)
            if (col.CompareTag(tag))
                return false;
        return true;
    }

    void SpawnMoss(Vector2 pos)
    {
        Instantiate(mossPrefab, pos, Quaternion.identity, transform).transform.localScale =
            Vector3.one * Random.Range(mossMinRadius, mossMaxRadius);
    }

    public void SpawnMossEdge(Vector2 minPos, Vector2 maxPos)
    {
        for (int i = 0; i < mossPerEdge; i++)
            SpawnMoss(Vector2.Lerp(minPos, maxPos, (float)i / mossPerEdge));
    }

    public void SpawnMossDir(Vector2 dir)
    {
        Vector2 rightDir = CMath.Rotate90Clock(dir);
        SpawnMossEdge((Vector2)transform.position + (dir - rightDir) * 0.5f,
            (Vector2)transform.position + (dir + rightDir) * 0.5f);
    }
}
