using UnityEngine;

public class StandardWeaponPrimary : WeaponPrimary
{
    public AudioClip[] activationClips;

    public virtual void OnActivate()
    {
        weapon.reload.remainingAmmo--;
    }
}
