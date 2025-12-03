using UnityEngine;

public class InventoryPickupTick : OnTickEffect
{
    public InventoryItem item;

    public override void OnTick(TickEntity tickEntity)
    {
        FindAnyObjectByType<InventoryController>().InstantiateItem(item);
    }
}
