using System.Collections.Generic;
using System.Net.NetworkInformation;

public class StatusEffect
{
    public Status status;
    public List<StatusComponent> components;
    public bool enabled, shouldRemove;

    public StatusEffect(HitStatus hitStatus)
    {
        status = hitStatus.status;
        Start();
        ApplyStack(hitStatus.components);

        // Add callback stuff here
    }

    public void ApplyStack(List<StatusComponent> components)
    {
        foreach (StatusComponent component in components)
        {
            int index = this.components.IndexOf(component);
            if (index != -1)
                this.components[index].Reapply(this, component);
        }
    }

    public void Start()
    {
        enabled = true;
        shouldRemove = false;

        components = status.components;
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
            component.Upd(this);
    }

    public void Destruct()
    {
        enabled = false;
        shouldRemove = true;
    }
}
