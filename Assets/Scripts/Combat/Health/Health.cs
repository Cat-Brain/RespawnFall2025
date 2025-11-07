using DamageNumbersPro;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health, maxHealth;
    public float fractionalHealthOffset;
    public List<StatusEffect> statuses = new();

    [HideInInspector] public HealthManager healthManager;
    protected int healthIndex = -1;
    protected bool alive = true;

    public void Awake()
    {
        healthManager = FindAnyObjectByType<HealthManager>();
        healthIndex = healthManager.healths.Count;
        healthManager.healths.Add(this);
    }

    void Update()
    {
        if (!alive)
            return;
        foreach (StatusEffect status in statuses)
        {
            if (!alive)
                return;
            if (status.enabled)
                status.Update();
        }

        foreach (StatusEffect status in statuses)
        {
            if (!alive)
                return;
            if (status.shouldRemove)
                status.End();
        }
        statuses.RemoveAll(status => status.shouldRemove);
    }

    public DamageNumber SpawnDamageNumber(int damage, Vector2 location)
    {
        DamageNumber damageNumber = healthManager.damagePopup.Spawn(location, damage, transform);
        damageNumber.SetSpamGroup("Damage On Health " + healthIndex.ToString());
        return damageNumber;
    }

    public void Die()
    {
        alive = false;
        OnDeath();
    }

    public bool ApplyHitDamage(float damage, Hit? hit = null)
    {
        if (!alive)
            return true;

        float effectiveDamage = damage + fractionalHealthOffset;
        if (effectiveDamage >= health)
        {
            OnHealthChange(0, hit);
            fractionalHealthOffset = 0;
            Die();
            return !alive;
        }
        fractionalHealthOffset = effectiveDamage % 1;
        effectiveDamage -= fractionalHealthOffset;
        OnHealthChange(health - Mathf.RoundToInt(effectiveDamage), hit);

        return false;
    }

    public bool ApplyHitStatus(HitStatus hitStatus)
    {
        if (!alive)
            return true;
        int index = statuses.FindIndex(status => status.status == hitStatus.status);
        if (index == -1)
            statuses.Add(new StatusEffect(this, hitStatus));
        else
            statuses[index].ApplyStack(hitStatus.components);
        return !alive;
    }

    public bool ApplyHit(Hit hit)
    {
        if (!alive)
            return true;
        OnHit(hit);
        if (!alive)
            return true;
        ApplyHitDamage(hit.damage, hit);
        if (!alive)
            return true;
        foreach (HitStatus hitStatus in hit.statuses)
            ApplyHitStatus(hitStatus);
        return !alive;
    }

    protected virtual void OnHealthChange(int newHealth, Hit? hit)
    {
        if (health == newHealth)
            return;

        if (newHealth < health)
            SpawnDamageNumber(health - newHealth, hit.HasValue && hit.Value.position != Vector2.zero ? hit.Value.position : transform.position);
        else
            Debug.LogWarning("We need to implement something for healing!!!");
        health = newHealth;
    }

    protected virtual void OnHit(Hit hit) { }
    protected virtual void OnDeath() { }
}
