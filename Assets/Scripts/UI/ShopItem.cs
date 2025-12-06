using NUnit.Framework.Internal;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItem : OnTickEffect
{
    public InventoryPickupTick pickupTick;
    public List<EventOnAction> eventActions;
    public TextMeshProUGUI priceText, dimensionsText;

    public List<ItemProbability> probabilities;

    public InventoryItem item = null;

    [HideInInspector] public ItemRandomizer randomizer;

    void Awake()
    {
        Restock();
    }

    public void Restock()
    {
        if (randomizer == null)
            randomizer = FindAnyObjectByType<ItemRandomizer>();
        item = randomizer.RandomItem(probabilities);
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
