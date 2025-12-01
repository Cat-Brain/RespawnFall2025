using UnityEngine;

public class StatusComponent : ScriptableObject
{
    public virtual void Reapply(StatusEffect effect, StatusComponent newApplicant) { }

    public virtual void Str(StatusEffect effect) { }
    public virtual void Upd(StatusEffect effect) { }
    public virtual void End(StatusEffect effect) { }

    public virtual void Tick(StatusEffect effect, int tickIndex) { }
}

public interface IOnHitStatus
{
    public void OnHit(StatusEffect effect, ref Hit hit);
}

public interface IOnDeathStatus
{
    public void OnDeath(StatusEffect effect);
}