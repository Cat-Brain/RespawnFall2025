using UnityEngine;

public class InventoryPickupTick : OnTickEffect
{
    public SpriteRenderer sr;
    public InventoryItem item;

    public override void OnTick(TickEntity tickEntity)
    {
        FindAnyObjectByType<InventoryController>().InstantiateItem(item);
    }

    public void SetItem(InventoryItem item)
    {
        this.item = item;
        sr.sprite = item.sprite;
    }
}
