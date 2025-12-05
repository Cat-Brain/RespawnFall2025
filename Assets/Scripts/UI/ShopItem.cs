using System.Collections.Generic;
using UnityEngine;

public class ShopItem : OnTickEffect
{
    public InventoryPickupTick pickupTick;
    public List<EventOnAction> eventActions;

    public List<InventoryItem> items;

    public InventoryItem item = null;

    void Awake()
    {
        Restock();
    }

    public void Restock()
    {
        item = items[Random.Range(0, items.Count)];
        UpdateItem();
    }

    public void UpdateItem()
    {
        pickupTick.SetItem(item);
        SetActiveEventActions(true);
    }

    public void SetActiveEventActions(bool active)
    {
        foreach (EventOnAction eventAction in eventActions)
            eventAction.active = active;
    }

    public override void OnTick(TickEntity tickEntity)
    {
        item = null;
        SetActiveEventActions(false);
    }

    void Update()
    {
        if (item == null && pickupTick.item != null)
            pickupTick.SetItem(null);
    }
}
