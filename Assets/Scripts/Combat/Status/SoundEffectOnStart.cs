using UnityEngine;
using FMODUnity;

[CreateAssetMenu(fileName = "New SoundEffectOnStart", menuName = "StatusComponents/SoundEffectOnStart")]

public class SoundEffectOnStart : StatusComponent
{
    public EventReference evt;
    public override void Str(StatusEffect effect)
    {
        SFXManager.Instance.Play(evt);
    }
}
