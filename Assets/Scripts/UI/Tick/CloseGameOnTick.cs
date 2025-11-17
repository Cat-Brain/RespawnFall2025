using UnityEngine;

public class CloseGameOnTick : OnTickEffect
{
    public override void OnTick()
    {
        Application.Quit();
    }
}
