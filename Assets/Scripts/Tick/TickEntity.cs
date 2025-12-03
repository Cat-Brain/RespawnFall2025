using UnityEngine;

public class TickEntity : MonoBehaviour
{
    public virtual void Tick()
    {
        foreach (OnTickEffect effect in GetComponents<OnTickEffect>())
            effect.OnTick(this);
    }
}
