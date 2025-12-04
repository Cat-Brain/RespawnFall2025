using System;
using System.Linq;
using UnityEngine;

public class StatusEffect : MonoBehaviour
{
    public HealthInst health;

    public Status status;
    public StatusComponent[] components;
    public IOnHitStatus[] onHitComponents;
    public IOnDeathStatus[] onDeathComponents;
    public bool shouldRemove;

    public void ApplyStack(StatusComponent[] components)
    {
        foreach (StatusComponent component in components)
        {
            int index = Array.FindIndex(this.components, component2 => component.GetType() == component2.GetType());
            if (index != -1)
                this.components[index].Reapply(this, component);
        }
    }

    public void Init(HealthInst health, HitStatus hitStatus)
    {
        this.health = health;
        status = hitStatus.status;

        shouldRemove = false;

        components = status.components.Select(component => Instantiate(component)).ToArray();

        onHitComponents = components.Where(component => component is IOnHitStatus)
            .Select(component => component as IOnHitStatus).ToArray();
        onDeathComponents = components.Where(component => component is IOnDeathStatus)
            .Select(component => component as IOnDeathStatus).ToArray();

        foreach (StatusComponent component in components)
            component.Str(this);

        ApplyStack(hitStatus.components);
    }

    public void Upd()
    {
        if (!enabled)
            return;
        foreach (StatusComponent component in components)
            component.Upd(this);
    }

    public void End()
    {
        foreach (StatusComponent component in components)
            component.End(this);
    }

    public void OnHit(ref Hit hit)
    {
        foreach (IOnHitStatus component in onHitComponents)
            component.OnHit(this, ref hit);
    }

    public void OnDeath()
    {
        foreach (IOnDeathStatus component in onDeathComponents)
            component.OnDeath(this);
    }

    public void Tick(int tickIndex)
    {
        if (!enabled)
            return;
        foreach (StatusComponent component in components)
            component.Tick(this, tickIndex);
    }

    public void Destruct()
    {
        enabled = false;
        shouldRemove = true;
    }

    public T GetComp<T>() where T : StatusComponent
    {
        return (T)Array.Find(components, component => component is T);
    }
}
