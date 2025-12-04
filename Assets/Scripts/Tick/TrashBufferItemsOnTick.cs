using UnityEngine;

public class TrashBufferItemsOnTick : OnTickEffect
{
    public override void OnTick(TickEntity tickEntity)
    {
        FindAnyObjectByType<InventoryController>().TrashBuffer();
    }
}
