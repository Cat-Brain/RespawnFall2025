using UnityEngine;

public class TickEntity : MonoBehaviour
{
    public void Tick()
    {
        foreach (OnTickEffect effect in GetComponents<OnTickEffect>())
            effect.OnTick(this);
    }
}
