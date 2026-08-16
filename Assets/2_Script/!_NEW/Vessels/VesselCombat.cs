using UnityEngine;

public class VesselCombat : MonoBehaviour
{
    [Header("References")]
    public AttackRangeIndicator attackRangeIndicator;

    private VesselTargeting targeting;
    private VesselStats stats;
    private VesselKit kit;

    [Header("Basic Attack")]
    public float attackCooldown = 0.5f;

    private float nextAttackTime;
    private int basicAttackIndex = 0;

    // =========================================================
    // ACTION STATE
    // =========================================================

    public bool IsActing { get; private set; }

    public bool CanMove => !IsActing;


    private void Awake()
    {
        targeting = GetComponent<VesselTargeting>();
        stats = GetComponent<VesselStats>();
        kit = GetComponent<VesselKit>();

        if (kit == null)
        {
            Debug.LogError(
                gameObject.name +
                " has no VesselKit!"
            );
        }
    }


    private void Update()
    {
        HandleBasicAttackInput();
        HandleAbilityInput();
    }

    private void FaceTarget(Transform target)
    {
        if (target == null)
            return;

        Vector3 direction =
            target.position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.001f)
            return;

        transform.rotation =
            Quaternion.LookRotation(direction);
    }


    // =========================================================
    // BASIC ATTACK INPUT
    // =========================================================

    private void HandleBasicAttackInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (CanUseBasicAttack())
            {
                if (attackRangeIndicator != null)
                    attackRangeIndicator.Show();
            }
        }


        if (Input.GetMouseButtonUp(0))
        {
            if (attackRangeIndicator != null)
                attackRangeIndicator.Hide();

            TryBasicAttack();
        }
    }

    public void ResetBasicAttackCombo()
    {
        basicAttackIndex = 1;
    }


    // =========================================================
    // BASIC ATTACK
    // =========================================================

    private bool CanUseBasicAttack()
    {
        if (IsActing)
            return false;

        if (Time.time < nextAttackTime)
            return false;

        return true;
    }

    private float GetCurrentAttackCooldown()
    {
        float attackSpeedMultiplier =
            kit.GetAttackSpeedMultiplier();

        return attackCooldown / attackSpeedMultiplier;
    }


    private void TryBasicAttack()
    {
        if (!CanUseBasicAttack())
            return;


        if (targeting == null)
        {
            Debug.LogError(
                gameObject.name +
                " has no VesselTargeting!"
            );

            return;
        }


        if (kit == null)
        {
            Debug.LogError(
                gameObject.name +
                " has no VesselKit!"
            );

            return;
        }


        Transform target =
            targeting.FindNearestEnemy();


        if (target == null)
        {
            Debug.Log("No enemy in range.");
            return;
        }

        FaceTarget(target);

        StartAction();


        nextAttackTime = Time.time + GetCurrentAttackCooldown();


        // Get the next combo attack.

        basicAttackIndex++;

        if (basicAttackIndex > kit.GetBasicAttackCount())
        {
            basicAttackIndex = 1;
        }


        float multiplier =
    kit.GetBasicAttackMultiplier(
        basicAttackIndex
    );


        Debug.Log(
            gameObject.name +
            " Basic Attack " +
            basicAttackIndex +
            " | " +
            (multiplier * 100f) +
            "% ATK"
        );

    kit.UseBasicAttack(
    basicAttackIndex,
    target,
    stats
    );

        EndAction();
    }

    // =========================================================
    // Inputs for Skill and Ult
    // =========================================================

    private void HandleAbilityInput()
    {
        // Skill
        if (Input.GetKeyDown(KeyCode.E))
        {
            TrySkill();
        }


        // Ultimate
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TryUltimate();
        }
    }

    // Skill

    private void TrySkill()
    {
        if (IsActing)
            return;

        if (kit == null)
        {
            Debug.LogError(
                gameObject.name +
                " has no VesselKit!"
            );

            return;
        }


        StartAction();


        Debug.Log(
            gameObject.name +
            " is using Skill."
        );


        kit.UseSkill();


        EndAction();
    }

    // Ult

    private void TryUltimate()
    {
        if (IsActing)
            return;

        if (kit == null)
        {
            Debug.LogError(
                gameObject.name +
                " has no VesselKit!"
            );

            return;
        }


        StartAction();


        Debug.Log(
            gameObject.name +
            " is using Ultimate."
        );


        kit.UseUltimate(stats);


        EndAction();
    }

    // =========================================================
    // TEMPORARY DAMAGE
    // =========================================================

    private void PerformTemporaryDamage(
    Transform target,
    float multiplier
)
    {
        if (stats == null)
        {
            Debug.LogError(
                gameObject.name +
                " has no VesselStats!"
            );

            return;
        }

        if (stats.vesselData == null)
        {
            Debug.LogError(
                gameObject.name +
                " has no VesselData!"
            );

            return;
        }

        EnemyHealth enemyHealth =
            target.GetComponent<EnemyHealth>();

        if (enemyHealth == null)
            return;

        float damage =
            stats.ATK * multiplier;

        DamageType damageType =
            stats.vesselData.damageType;

        ElementType elementType =
            stats.vesselData.element;

        enemyHealth.TakeDamage(
            Mathf.RoundToInt(damage),
            damageType,
            elementType
        );
    }


    // =========================================================
    // ACTION STATE
    // =========================================================

    public void StartAction()
    {
        IsActing = true;
    }


    public void EndAction()
    {
        IsActing = false;
    }
}