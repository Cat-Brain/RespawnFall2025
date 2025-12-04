using UnityEngine;

[CreateAssetMenu(fileName = "New MoneyOnSellItem", menuName = "Item Components/MoneyOnSellItem")]
public class MoneyOnSellItem : ItemComponent
{
    public int flatMoney, moneyPerCell;

    public override void OnSell(InventoryItem item)
    {
        item.inst.controller.manager.currentLevelMoney += flatMoney + item.positions.Count * moneyPerCell;
    }
}
