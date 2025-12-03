using UnityEngine;

[CreateAssetMenu(fileName = "New TickStatus", menuName = "StatusComponents/TickStatus")]
public class TickStatus : StatusComponent
{
    public int tickIndex;
    public bool tickOnStart;
    public float timePerTick;
    protected float timeTillTick;

    public override void Str(StatusEffect effect)
    {
        timeTillTick = timePerTick;
        if (tickOnStart)
            effect.Tick(tickIndex);
    }

    public override void Upd(StatusEffect effect)
    {
        timeTillTick -= Time.deltaTime;
        if (timeTillTick <= 0)
            effect.Tick(tickIndex);
    }

    public override void Tick(StatusEffect effect, int tickIndex)
    {
        if (this.tickIndex == tickIndex && timeTillTick <= 0)
            timeTillTick += timePerTick;
    }
}
