using UnityEngine;

[CreateAssetMenu(fileName = "New ApplyStatModOnTick", menuName = "StatusComponents/ApplyStatModOnTick")]
public class ApplyStatModOnTick : StatusComponent
{
    public int tickIndex;
    public StatModifier statMod;
    public StatTarget target;

    public override void Tick(StatusEffect effect, int tickIndex)
    {
        if (this.tickIndex == tickIndex)
            effect.health.stats.ApplyMod(new StatChange(statMod, effect, target));
    }
}