using System;
using System.Linq;

[Serializable]
public class StatusEffect
{
    public HealthInst health;

    public Status status;
    public StatusComponent[] components;
    public IOnHitStatus[] onHitComponents;
    public bool enabled, shouldRemove;

    public StatusEffect(HealthInst health, HitStatus hitStatus)
    {
        this.health = health;
        status = hitStatus.status;
        Start();
        ApplyStack(hitStatus.components);

        // Add callback stuff here
    }

    public void ApplyStack(StatusComponent[] components)
    {
        foreach (StatusComponent component in components)
        {
            int index = Array.FindIndex(this.components, component2 => component.GetType() == component2.GetType());
            if (index != -1)
                this.components[index].Reapply(this, component);
        }
    }

    public void Start()
    {
        enabled = true;
        shouldRemove = false;

        components = status.components.Select(component => UnityEngine.Object.Instantiate(component)).ToArray();

        onHitComponents = new IOnHitStatus[components.Count(component => component is IOnHitStatus)];
        int index = 0;
        foreach (StatusComponent component in components)
            if (component is IOnHitStatus)
                onHitComponents[index++] = component as IOnHitStatus;

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
        foreach (IOnHitStatus component in onHitComponents)
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
        return (T)Array.Find(components, component => component is T);
    }
}
