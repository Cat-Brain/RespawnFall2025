using UnityEngine;

[CreateAssetMenu(fileName = "New HitOnTickStatus", menuName = "StatusComponents/HitOnTickStatus")]
public class HitOnTickStatus : StatusComponent
{
    public int tickIndex;
    public Hit hit;

    public override void Tick(StatusEffect effect, int tickIndex)
    {
        if (this.tickIndex == tickIndex)
            effect.health.ApplyHit(hit);
    }
}
