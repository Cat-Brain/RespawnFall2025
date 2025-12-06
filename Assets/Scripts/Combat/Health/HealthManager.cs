using DamageNumbersPro;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public GameManager gameManager;
    public DamageNumber damagePopup;

    public List<HealthInst> healths = new();

    void Awake()
    {
        damagePopup.PrewarmPool();
    }
}
