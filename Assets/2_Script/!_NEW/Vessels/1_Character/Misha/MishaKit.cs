using UnityEngine;

public class MishaKit : VesselKit
{
    private bool skillActive;

    [Header("Basic Attack")]
    public float basicAttack1Multiplier = 0.90f;
    public float basicAttack2Multiplier = 0.80f;
    public float basicAttack3Multiplier = 1.20f;


    [Header("Basic Attack Projectile")]
    public VesselProjectile projectilePrefab;

    public Transform projectileSpawnPoint;

    public float projectileSpeed = 15f;

    public float projectileLifetime = 3f;


    [Header("Skill")]
    public float skillDuration = 10f;

    public float skillBasicAttackBonus = 0.15f;

    public float skillAttackSpeedBonus = 0.20f;


    [Header("Ultimate - Destruction")]
    public float ultimateMultiplier = 3.50f;

    public float ultimateRadius = 10f;

    public float ultimateForwardDistance = 5f;


    // =========================================================
    // BASIC ATTACK
    // =========================================================

    public override int GetBasicAttackCount()
    {
        return 3;
    }


    public override float GetBasicAttackMultiplier(
        int attackIndex
    )
    {
        float multiplier;

        switch (attackIndex)
        {
            case 1:
                multiplier = basicAttack1Multiplier;
                break;

            case 2:
                multiplier = basicAttack2Multiplier;
                break;

            case 3:
                multiplier = basicAttack3Multiplier;
                break;

            default:
                multiplier = 1f;
                break;
        }


        if (skillActive)
        {
            multiplier += skillBasicAttackBonus;
        }


        return multiplier;
    }

    public override void UseBasicAttack(
    int attackIndex,
    Transform target,
    VesselStats stats
)
    {
        FireBasicAttackProjectile(
            attackIndex,
            target,
            stats
        );
    }

    public void FireBasicAttackProjectile(
    int attackIndex,
    Transform target,
    VesselStats stats
)
    {
        if (projectilePrefab == null)
        {
            Debug.LogError(
                "Misha has no projectile prefab assigned!"
            );

            return;
        }

        if (projectileSpawnPoint == null)
        {
            Debug.LogError(
                "Misha has no projectile spawn point assigned!"
            );

            return;
        }

        if (target == null)
        {
            Debug.LogError(
                "Misha has no target for projectile!"
            );

            return;
        }


        VesselProjectile projectile =
            Instantiate(
                projectilePrefab,
                projectileSpawnPoint.position,
                projectileSpawnPoint.rotation
            );


        projectile.speed =
            projectileSpeed;

        projectile.lifetime =
            projectileLifetime;

        projectile.target =
            target;

        projectile.ownerStats = 
            stats;

        projectile.damageMultiplier =
    GetBasicAttackMultiplier(attackIndex);


        // =====================================================
        // BA1
        // =====================================================

        if (attackIndex == 1)
        {
            projectile.isHoming = true;
            projectile.isPiercing = false;
            projectile.isExploding = false;
        }


        // =====================================================
        // BA2
        // =====================================================

        else if (attackIndex == 2)
        {
            projectile.isHoming = true;
            projectile.isPiercing = false;
            projectile.isExploding = false;
        }


        // =====================================================
        // BA3
        // =====================================================

        else if (attackIndex == 3)
        {
            projectile.isHoming = true;
            projectile.isPiercing = false;
            projectile.isExploding = true;
        }
    }


    // =========================================================
    // ATTACK SPEED
    // =========================================================

    public override float GetAttackSpeedMultiplier()
    {
        if (skillActive)
        {
            return 1f + skillAttackSpeedBonus;
        }

        return 1f;
    }


    // =========================================================
    // SKILL
    // =========================================================

    public override void UseSkill()
    {
        if (skillActive)
        {
            Debug.Log(
                "Misha's Skill is already active!"
            );

            return;
        }


        skillActive = true;


        Debug.Log(
            "Misha used Skill!"
        );

        Debug.Log(
            "Basic Attacks enhanced!"
        );

        Debug.Log(
            "BA Bonus: +" +
            (skillBasicAttackBonus * 100f) +
            "%"
        );

        Debug.Log(
            "Attack Speed Bonus: +" +
            (skillAttackSpeedBonus * 100f) +
            "%"
        );


        Invoke(
            nameof(EndSkill),
            skillDuration
        );
    }


    private void EndSkill()
    {
        skillActive = false;

        Debug.Log(
            "Misha's Skill has ended."
        );
    }


    // =========================================================
    // ULTIMATE - DESTRUCTION
    // =========================================================

    public override void UseUltimate(
        VesselStats stats
    )
    {
        if (stats == null)
        {
            Debug.LogError(
                "Misha cannot use Destruction because VesselStats is missing!"
            );

            return;
        }


        if (stats.vesselData == null)
        {
            Debug.LogError(
                "Misha cannot use Destruction because VesselData is missing!"
            );

            return;
        }


        Debug.Log(
            "Misha used Destruction!"
        );


        Vector3 center =
            transform.position +
            transform.forward *
            ultimateForwardDistance;


        Collider[] hits =
            Physics.OverlapSphere(
                center,
                ultimateRadius
            );


        foreach (Collider hit in hits)
        {
            EnemyHealth enemyHealth =
                hit.GetComponentInParent<EnemyHealth>();


            if (enemyHealth == null)
                continue;


            float damage =
                stats.ATK *
                ultimateMultiplier;


            enemyHealth.TakeDamage(
                Mathf.RoundToInt(damage),
                stats.vesselData.damageType,
                stats.vesselData.element
            );
        }
    }


    // =========================================================
    // DESTRUCTION GIZMO
    // =========================================================

    private void OnDrawGizmosSelected()
    {
        Vector3 center =
            transform.position +
            transform.forward * ultimateForwardDistance;

        Gizmos.DrawWireSphere(
            center,
            ultimateRadius
        );

        Gizmos.DrawLine(
            transform.position,
            center
        );
    }
}