using UnityEngine;

[CreateAssetMenu(fileName = "New StatItem", menuName = "Item Components/StatItem")]
public class StatItem : ItemComponent
{
    public StatModifier modifier;
    public StatTarget target;

    [HideInInspector] public StatChange change = null;

    public override void OnPlace(InventoryItem item)
    {
        change = new StatChange(modifier, this, target);
        item.inst.controller.manager.playerManager.health.ApplyStat(change);
    }

    public override void OnRemove(InventoryItem item)
    {
        if (change == null)
            return;
        item.inst.controller.manager.playerManager.health.RemoveStat(change);
        change = null;
    }

    public override void OnSell(InventoryItem item)
    {
        OnRemove(item);
    }
}