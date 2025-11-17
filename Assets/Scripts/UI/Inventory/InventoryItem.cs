using com.cyborgAssets.inspectorButtonPro;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New InventoryItem", menuName = "Items/Item")]
public class InventoryItem : ScriptableObject
{
    public InventoryLayer layer;
    public List<Vector2Int> positions;

    public Sprite sprite;

    [HideInInspector] public Vector2Int dimensions;

    protected void FindDimensions()
    {
        if (positions == null)
            dimensions = Vector2Int.zero;
        dimensions = positions[0];
        for (int i = 1; i < positions.Count; i++)
            dimensions = Vector2Int.Max(dimensions, positions[i]);
        dimensions += Vector2Int.one;
    }

    protected void CleanOffsets()
    {
        if (positions == null)
            return;

        // Remove duplicates:
        positions = positions.Distinct().ToList();

        // Allign such that (0, 0) is the bottom left of the AABB:
        Vector2Int minPos = positions[0];
        for (int i = 1; i < positions.Count; i++)
            minPos = Vector2Int.Min(minPos, positions[i]);

        for (int i = 0; i < positions.Count; i++)
            positions[i] -= minPos;

        // Sort by left to right then top to bottom:
        for (int i = 0; i < positions.Count - 1; i++)
        {
            int bestIndex = i;
            for (int j = i + 1; j < positions.Count; j++)
                if (positions[bestIndex].y < positions[j].y ||
                    (positions[bestIndex].y == positions[j].y &&
                    positions[bestIndex].x > positions[j].x))
                    bestIndex = j;
            (positions[i], positions[bestIndex]) = (positions[bestIndex], positions[i]);
        }
    }

    [ProButton]
    public void Clean()
    {
        CleanOffsets();
        FindDimensions();
    }
}
