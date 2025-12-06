using UnityEngine;

public class StandardWeaponPrimary : WeaponPrimary
{
    public virtual void OnActivate()
    {
        SFXManager.Instance.Play(SFXManager.Instance.shootIce);   

        weapon.reload.remainingAmmo--;
    }
}
