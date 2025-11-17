using UnityEngine;

public class ButtonTick : MonoBehaviour
{
    public void Tick()
    {
        foreach (OnTickEffect effect in GetComponents<OnTickEffect>())
            effect.OnTick();
    }
}
