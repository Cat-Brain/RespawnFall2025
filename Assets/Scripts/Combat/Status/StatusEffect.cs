using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class StatusEffect
{
    public Health health;

    public Status status;
    public List<StatusComponent> components;
    public bool enabled, shouldRemove;

    public StatusEffect(Health health, HitStatus hitStatus)
    {
        this.health = health;
        status = hitStatus.status;
        Start();
        ApplyStack(hitStatus.components);

        // Add callback stuff here
    }

    public void ApplyStack(List<StatusComponent> components)
    {
        foreach (StatusComponent component in components)
        {
            int index = this.components.FindIndex(component2 => component.GetType() == component2.GetType());
            if (index != -1)
                this.components[index].Reapply(this, component);
        }
    }

    public void Start()
    {
        enabled = true;
        shouldRemove = false;

        components = status.components.Select(component => UnityEngine.Object.Instantiate(component)).ToList();
        foreach (StatusComponent component in components)
            component.Str(this);
    }

    public void Update()
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
        foreach (StatusComponent component in components)
            component.OnHit(this, ref hit);
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

    public T GetComponent<T>() where T : StatusComponent
    {
        return (T)components.Find(component => component is T);
    }
}
