using System.Collections.Generic;
using UnityEngine;

public class DisableComponentsOnTick : OnTickEffect
{
    public List<Behaviour> components;

    public override void OnTick(TickEntity tickEntity)
    {
        foreach (Behaviour component in components)
            component.enabled = false;
    }
}