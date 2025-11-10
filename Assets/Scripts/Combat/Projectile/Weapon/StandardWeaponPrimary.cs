using UnityEngine;

public class StandardWeaponPrimary : WeaponPrimary
{
    public AudioClip[] activationClips;

    public virtual void OnActivate()
    {
        weapon.reload.remainingAmmo--;
        AudioManager.instance.PlaySoundFXClip(activationClips[Random.Range(0, activationClips.Length)], weapon.wielder, 1);
    }
}
