using UnityEngine;

public class WeaponPrimary : ScriptableObject
{
    [HideInInspector] public Weapon weapon;

    public virtual void Began() { }

    public virtual void Str(Weapon weapon)
    {
        this.weapon = weapon;
    }

    public virtual void Upd(bool activated) { }

    public virtual void Ended() { }
}
