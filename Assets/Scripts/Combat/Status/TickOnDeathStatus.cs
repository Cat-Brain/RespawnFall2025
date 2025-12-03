using UnityEngine;

[CreateAssetMenu(fileName = "New TickOnDeathStatus", menuName = "StatusComponents/TickOnDeathStatus")]
public class TickOnDeathStatus : StatusComponent, IOnDeathStatus
{
    public int tickIndex;

    public void OnDeath(StatusEffect effect)
    {
        Debug.Log("!!");
        effect.Tick(tickIndex);
    }
}