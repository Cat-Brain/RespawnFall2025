using UnityEngine;

public class ItemComponent : ScriptableObject
{
    public virtual void OnPlace(InventoryItem item) { }
    public virtual void OnRemove(InventoryItem item) { }
    public virtual void OnSell(InventoryItem item) { }
}
