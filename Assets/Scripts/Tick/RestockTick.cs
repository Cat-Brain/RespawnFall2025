using System.Collections.Generic;
using UnityEngine;

public class RestockTick : OnTickEffect
{
    public List<ShopItem> items;

    public override void OnTick(TickEntity tickEntity)
    {
        foreach (ShopItem item in items)
            item.Restock();
    }
}
