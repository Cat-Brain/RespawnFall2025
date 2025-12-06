using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ItemRarity
{
    public Rarity rarity;
    public List<InventoryItem> items;
}

[Serializable]
public struct ItemProbability
{
    public Rarity rarity;
    public int likelihood;
}

public class ItemRandomizer : MonoBehaviour
{
    public List<ItemRarity> rarities;

    public InventoryItem RandomItem(in List<ItemProbability> probabilities)
    {
        int totalProbability = 0;
        probabilities.ForEach((probability) => totalProbability += probability.likelihood);

        int rarityValue = UnityEngine.Random.Range(0, totalProbability);
        foreach (ItemProbability probability in probabilities)
        {
            if (probability.likelihood < rarityValue)
            {
                rarityValue -= probability.likelihood;
                continue;
            }
            foreach (ItemRarity rarity in rarities)
                if (rarity.rarity == probability.rarity)
                    return rarity.items[UnityEngine.Random.Range(0, rarity.items.Count)];
        }
        Debug.LogWarning("Could not find valid item!");
        return null;
    }
}
