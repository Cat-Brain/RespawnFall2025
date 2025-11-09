using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnDeath : Health
{
    public float deathTime;
    public List<Behaviour> disabledOnDeath;

    protected override void OnDeath()
    {
        transform.DOScale(0, deathTime).OnComplete(() => Destroy(gameObject));
        enabled = false;
        foreach (Behaviour component in disabledOnDeath)
            component.enabled = false;
        base.OnDeath();
    }
}
