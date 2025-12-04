using UnityEngine;

[CreateAssetMenu(fileName = "New TickOnStartStatus", menuName = "StatusComponents/TickOnStartStatus")]
public class TickOnStartStatus : StatusComponent
{
    public int tickIndex;

    public override void Str(StatusEffect effect)
    {
        effect.Tick(tickIndex);
    }
}
