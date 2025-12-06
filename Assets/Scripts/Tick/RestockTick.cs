using UnityEngine;

public class RestockTick : OnTickEffect
{
    public override void OnTick(TickEntity tickEntity)
    {
        foreach (ShopItem item in FindObjectsByType<ShopItem>(FindObjectsSortMode.None))
            item.Restock();
    }
}
