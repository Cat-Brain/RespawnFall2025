using DamageNumbersPro;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public DamageNumber damagePopup;

    void Awake()
    {
        damagePopup.PrewarmPool();
    }
}
