using DamageNumbersPro;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public DamageNumber damagePopup;

    public List<Health> healths = new();

    void Awake()
    {
        damagePopup.PrewarmPool();
    }
}
