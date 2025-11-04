using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health, maxHealth;
    public float fractionalHealthOffset;
    public List<StatusEffect> statuses = new();

    protected bool alive = true;

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

    public void Die()
    {
        alive = false;
        OnDeath();
    }

    public bool ApplyHitDamage(float damage)
    {
        if (!alive)
            return true;

        float effectiveDamage = damage + fractionalHealthOffset;
        if (effectiveDamage >= health)
        {
            health = 0;
            fractionalHealthOffset = 0;
            Die();
            return !alive;
        }
        fractionalHealthOffset = effectiveDamage % 1;
        effectiveDamage -= fractionalHealthOffset;
        OnHealthChange(health - Mathf.RoundToInt(effectiveDamage));

        return false;
    }

    public bool ApplyHitStatus(HitStatus hitStatus)
    {
        if (!alive)
            return true;
        int index = statuses.FindIndex(status => status.status == hitStatus.status);
        if (index == -1)
            statuses.Add(new StatusEffect(hitStatus));
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
        ApplyHitDamage(hit.damage);
        if (!alive)
            return true;
        foreach (HitStatus hitStatus in hit.statuses)
            ApplyHitStatus(hitStatus);
        return !alive;
    }

    protected virtual void OnHealthChange(int newHealth)
    {
        health = newHealth;
    }

    protected virtual void OnHit(Hit hit) { }
    protected virtual void OnDeath() { }
}
