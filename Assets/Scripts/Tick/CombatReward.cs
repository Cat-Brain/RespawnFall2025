using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatReward : OnTickEffect
{
    public GameObject itemPrefab;
    public List<ItemProbability> itemProbabilities;
    public int toSpawnItems;
    public float maxItemRot, itemForce, itemThrowTime;

    [HideInInspector] public GameManager manager;
    [HideInInspector] public ItemRandomizer randomizer;

    [HideInInspector] public float itemThrowTimer = 0;
    [HideInInspector] public bool hasRewarded = false;

    void Awake()
    {
        manager = FindAnyObjectByType<GameManager>();
    }

    public override void OnTick(TickEntity tickEntity)
    {
        if (hasRewarded || manager.inCombat)
            return;

        StartCoroutine(SpawnReward());
    }

    public IEnumerator SpawnReward()
    {
        if (hasRewarded)
            yield break;

        hasRewarded = true;
        for (int i = 0; i < toSpawnItems; i++)
        {
            yield return new WaitForSeconds(itemThrowTime);
            if (randomizer == null)
                randomizer = FindAnyObjectByType<ItemRandomizer>();
            GameObject newItem = Instantiate(itemPrefab, transform.position, Quaternion.identity, transform);
            newItem.GetComponent<InventoryPickupTick>().SetItem(
                randomizer.RandomItem(itemProbabilities));
            newItem.GetComponent<Rigidbody2D>().linearVelocity = itemForce *
                CMath.Rotate(Vector2.up, Random.Range(-maxItemRot, maxItemRot) * Mathf.Deg2Rad);

            itemThrowTimer += itemThrowTime;
        }
    }
}
