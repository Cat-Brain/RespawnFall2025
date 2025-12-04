using System.Collections.Generic;
using UnityEngine;

public class CombatReward : MonoBehaviour
{
    public GameObject itemPrefab;
    public List<InventoryItem> potentialSpawns;
    public int toSpawnItems;
    public float maxItemRot, itemForce, itemThrowTime;

    [HideInInspector] public GameManager manager;

    [HideInInspector] public float itemThrowTimer = 0;
    [HideInInspector] public bool hasRewarded = false;

    void Awake()
    {
        manager = FindAnyObjectByType<GameManager>();
    }

    void Update()
    {
        if (hasRewarded || manager.inCombat)
            return;

        if (toSpawnItems == 0)
        {
            hasRewarded = true;
            return;
        }

        itemThrowTimer -= Time.deltaTime;
        if (itemThrowTimer > 0)
            return;

        GameObject newItem = Instantiate(itemPrefab, transform.position, Quaternion.identity, transform);
        newItem.GetComponent<InventoryPickupTick>().SetItem(
            potentialSpawns[Random.Range(0, potentialSpawns.Count)]);
        newItem.GetComponent<Rigidbody2D>().linearVelocity = itemForce *
            CMath.Rotate(Vector2.up, Random.Range(-maxItemRot, maxItemRot) * Mathf.Deg2Rad);

        itemThrowTimer += itemThrowTime;
        toSpawnItems--;
    }
}
