using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItem : OnTickEffect
{
    public InventoryPickupTick pickupTick;
    public List<EventOnAction> eventActions;
    public TextMeshProUGUI priceText, dimensionsText;

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
        if (item != null)
            dimensionsText.text = item.dimensions.x.ToString() + "x" + item.dimensions.y.ToString();
        pickupTick.SetItem(item);
        SetActive(true);
    }

    public void SetActive(bool active)
    {
        foreach (EventOnAction eventAction in eventActions)
            eventAction.active = active;
        priceText.enabled = active;
        dimensionsText.enabled = active;
    }

    public override void OnTick(TickEntity tickEntity)
    {
        item = null;
        SetActive(false);
    }

    void Update()
    {
        if (item == null && pickupTick.item != null)
            pickupTick.SetItem(null);
    }
}
