using com.cyborgAssets.inspectorButtonPro;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Flags]

public enum Rarity
{
    NONE = 0,
    COMMON = 1,
    UNCOMMON = 2,
    RARE = 3,
    LEGENDARY = 4,
    SHINY = 5
}
public enum InventoryLayer
{
    None = 0,
    DEFAULT = 1,
    UNIMPLEMENTED1 = 2,
    //UNIMPLEMENTED2 = 4,
    //UNIMPLEMENTED3 = 8,
    //UNIMPLEMENTED4 = 16,
    //UNIMPLEMENTED5 = 32,
    //UNIMPLEMENTED6 = 64,
    //UNIMPLEMENTED7 = 128,
}

public class InventoryController : MonoBehaviour
{
    [HideInInspector] public GameManager manager;
    public Canvas canvas;
    public RectTransform inventoryTransform, bufferTransform, floatingTransform;
    public TextMeshProUGUI descriptionText;
    public GameObject inventoryItemPrefab;

    public Vector2Int dimensions;
    public float cellWidth, bufferWidth;
    public float itemSpringFrequency, itemSpringDamping;

    public SpringUtils.tDampedSpringMotionParams itemSpring = new();
    public List<InventoryInst> items = new(), bufferItems = new();
    public byte[,] map;

    void Awake()
    {
        manager = FindAnyObjectByType<GameManager>();
    }

    void Update()
    {
        SpringUtils.CalcDampedSpringMotionParams(ref itemSpring, Time.deltaTime, itemSpringFrequency, itemSpringDamping);
        if (manager.gameState != GameState.INVENTORY)
            descriptionText.text = "";
    }

    [ProButton]
    public InventoryInst InstantiateItem(InventoryItem item)
    {
        InventoryInst inst = Instantiate(inventoryItemPrefab, floatingTransform).GetComponent<InventoryInst>();
        inst.item = item;
        inst.controller = this;

        inst.rectTransform.anchoredPosition = inst.desiredPos = WorldPos(FindValidPosition(item));
        inst.Init();
        AddToInventory(inst);
        inst.Clean();

        return inst;
    }

    [ProButton]
    public void FindMap()
    {
        map = new byte[dimensions.x, dimensions.y];
        foreach (InventoryInst item in items)
            foreach (Vector2Int position in item.item.positions)
            {
                Vector2Int gridPos = position + item.gridPos;
                map[gridPos.x, gridPos.y] |= (byte)item.item.layer;
            }
    }

    [ProButton]
    public void CleanDimensions()
    {
        inventoryTransform.sizeDelta = cellWidth * (Vector2)dimensions;
    }

    public Vector2 GlobalToLocalOffset(Vector2 offset)
    {
        return offset / canvas.scaleFactor;
    }

    public Vector2Int LocalGridPos(Vector2 position)
    {
        return Vector2Int.RoundToInt(
            position / cellWidth + (dimensions - Vector2.one) * 0.5f);
    }

    public Vector2 WorldPos(Vector2 gridPos)
    {
        return (gridPos + (Vector2.one - dimensions) * 0.5f) * cellWidth;
    }

    public Vector2 RoundToGridPos(Vector2 position)
    {
        return WorldPos(LocalGridPos(position));
    }

    public bool ValidPosition(InventoryItem item, Vector2Int position)
    {
        if (position.x < 0 || position.x + item.dimensions.x > dimensions.x ||
            position.y < 0 || position.y + item.dimensions.y > dimensions.y)
            return false;

        FindMap();

        byte[,] overlapMap = new byte[item.dimensions.x, item.dimensions.y];

        foreach (Vector2Int offset in item.positions)
        {
            Vector2Int gridPos = offset + position;
            overlapMap[offset.x, offset.y] = (byte)(map[gridPos.x, gridPos.y] & (byte)item.layer);
        }

        for (int x = 0; x < item.dimensions.x; x++)
            for (int y = 0; y < item.dimensions.y; y++)
                if (overlapMap[x, y] != 0)
                    return false;
        return true;
    }

    public bool ValidPosition(InventoryInst item)
    {
        return ValidPosition(item.item, LocalGridPos(item.desiredPos));
    }

    public Vector2Int FindValidPosition(InventoryItem item)
    {
        Vector2Int pos;
        for (int y = dimensions.y - item.dimensions.y; y >= 0; y--)
            for (int x = 0; x < dimensions.x; x++)
                if (ValidPosition(item, pos = new Vector2Int(x, y)))
                    return pos;

        return -Vector2Int.one;
    }

    public float BufferWidth()
    {
        if (bufferItems.Count == 0)
            return 0;

        float result = cellWidth * bufferItems[0].item.dimensions.x;
        for (int i = 1; i < bufferItems.Count; i++)
            result += bufferWidth + cellWidth * bufferItems[i].item.dimensions.x;

        return result;
    }

    public Vector2 BufferPos(int bufferIndex)
    {
        if (bufferItems.Count == 0)
            return Vector2.zero;

        float totalWidth = BufferWidth(), horizontalOffset = 0;
        for (int i = 0; i < bufferIndex; i++)
            horizontalOffset += bufferWidth + cellWidth * bufferItems[i].item.dimensions.x;

        return (horizontalOffset + (cellWidth - totalWidth) * 0.5f) * Vector2.right +
            (0.5f - bufferItems[bufferIndex].item.dimensions.y) * cellWidth * Vector2.up;
    }

    [ProButton]
    public void SnapInventoryPosition(InventoryInst item)
    {
        item.gridPos = LocalGridPos(item.desiredPos);
        item.rectTransform.SetParent(inventoryTransform);
        item.desiredPos = WorldPos(item.gridPos);
    }

    [ProButton]
    public void SnapBufferPosition(InventoryInst item)
    {
        item.gridPos = -Vector2Int.one;
        item.rectTransform.SetParent(bufferTransform);
        item.desiredPos = BufferPos(item.index);
    }

    public void SilentAddItems()
    {
        foreach (InventoryInst item in items)
            item.item.OnPlace();
    }

    public void SilentRemoveItems()
    {
        foreach (InventoryInst item in items)
            item.item.OnRemove();
    }

    public void AddToInventory(InventoryInst item)
    {
        if (ValidPosition(item))
            AddToItems(item);
        else
            AddToBuffer(item);
    }

    public void AddToItems(InventoryInst item)
    {
        item.index = items.Count;
        SnapInventoryPosition(item);

        SilentRemoveItems();
        items.Add(item);
        SilentAddItems();
    }

    public void AddToBuffer(InventoryInst item)
    {
        item.index = bufferItems.Count;
        bufferItems.Add(item);

        for (int i = 0; i < bufferItems.Count; i++)
            SnapBufferPosition(bufferItems[i]);
    }

    public void RemoveFromInventory(InventoryInst item)
    {
        if (item.gridPos == -Vector2Int.one)
            RemoveFromBuffer(item);
        else
            RemoveFromItems(item);
    }

    public void RemoveFromItems(InventoryInst item)
    {
        SilentRemoveItems();
        items.RemoveAt(item.index);

        item.index = -1;
        item.gridPos = -Vector2Int.one;

        item.desiredPos += inventoryTransform.anchoredPosition - floatingTransform.anchoredPosition;
        item.rectTransform.SetParent(floatingTransform);

        for (int i = 0; i < items.Count; i++)
            items[i].index = i;

        SilentAddItems();
    }

    public void RemoveFromBuffer(InventoryInst item)
    {
        bufferItems.RemoveAt(item.index);

        item.index = -1;
        item.gridPos = -Vector2Int.one;

        item.desiredPos += bufferTransform.anchoredPosition - floatingTransform.anchoredPosition;
        item.rectTransform.SetParent(floatingTransform);

        for (int i = 0; i < bufferItems.Count; i++)
        {
            bufferItems[i].index = i;
            SnapBufferPosition(bufferItems[i]);
        }
    }

    public void TrashBuffer()
    {
        foreach (InventoryInst item in bufferItems)
            Destroy(item.gameObject);

        bufferItems.Clear();
    }

    public void TrashInventory()
    {
        foreach (InventoryInst item in items)
            Destroy(item.gameObject);

        items.Clear();
    }

    public void TrashAll()
    {
        TrashBuffer();
        TrashInventory();
    }
}
