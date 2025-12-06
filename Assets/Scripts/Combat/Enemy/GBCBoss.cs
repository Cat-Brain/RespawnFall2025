using UnityEngine;

public class GBCBoss : Enemy
{
    public GameObject summonPrefab, smokePrefab, projectilePrefab;
    public Projectile projectile;

    public HealthInst health;
    public FlipToDirection flip;

    public float width;
    public int smokeSpawns;

    public enum Phase
    {
        PHASE_1, PHASE_2
    }

    Phase phase = Phase.PHASE_1;

    public enum Attack
    {
        CHARGE_RIGHT, CHARGE_LEFT, SHOOT_FROM_LEFT, SHOOT_FROM_RIGHT, SHOOT_FROM_MIDDLE, SUMMON
    }

    Attack attack;

    public bool grounded;
    public float groundedTimer, emmergeTimer, attackTimer;

    public void Update()
    {
        phase = health.health > health.data.maxHealth * 0.5f ? Phase.PHASE_1 : Phase.PHASE_2;


    }

    public override void ActiveEnter()
    {
        attack = Attack.SUMMON;
        attackTimer = 0;
        grounded = false;
        base.ActiveEnter();
    }

    public override void ActiveUpdate()
    {
        if (grounded)
        {

        }
    }
}
