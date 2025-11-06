using UnityEngine;

[CreateAssetMenu(fileName = "New TickStatus", menuName = "StatusComponents/TickStatus")]
public class TickStatus : StatusComponent
{
    public bool tickOnStart;
    public float timePerTick;
    protected float timeTillTick;

    public override void Str(StatusEffect effect)
    {
        timeTillTick = timePerTick;
        if (tickOnStart)
            effect.Tick();
    }

    public override void Upd(StatusEffect effect)
    {
        timeTillTick -= Time.deltaTime;
        if (timeTillTick <= 0)
            effect.Tick();
    }

    public override void Tick(StatusEffect effect)
    {
        if (timeTillTick <= 0)
            timeTillTick += timePerTick;
    }
}
