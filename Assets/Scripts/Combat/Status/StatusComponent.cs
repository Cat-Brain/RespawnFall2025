using UnityEngine;

public class StatusComponent : ScriptableObject
{
    public virtual void Reapply(StatusEffect effect, StatusComponent newApplicant) { }

    public virtual void Str(StatusEffect effect) { }
    public virtual void Upd(StatusEffect effect) { }
    public virtual void End(StatusEffect effect) { }
}
