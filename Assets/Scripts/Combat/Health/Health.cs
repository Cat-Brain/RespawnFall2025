using UnityEngine;

public class Health : ScriptableObject
{
    [HideInInspector] public HealthInst inst;

    [Tooltip("Set to -1 to default to max health")]
    public int startHealth = -1;
    public int maxHealth;

    public virtual void Init()
    {
        inst.health = startHealth == -1 ? maxHealth : startHealth;
    }

    public virtual void OnDeath() { }
}
