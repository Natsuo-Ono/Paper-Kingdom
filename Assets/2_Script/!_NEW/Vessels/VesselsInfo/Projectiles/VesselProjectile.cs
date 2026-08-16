using System.Collections.Generic;
using UnityEngine;

public class VesselProjectile : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 15f;
    public float lifetime = 3f;

    [Header("Homing")]
    public bool isHoming = false;
    public Transform target;

    [Header("Behavior")]
    public bool isPiercing = false;
    public bool isExploding = false;

    [Header("Damage")]
    public float damageMultiplier = 1f;
    public VesselStats ownerStats;

    private HashSet<EnemyHealth> hitEnemies =
        new HashSet<EnemyHealth>();


    private void Start()
    {
        Destroy(gameObject, lifetime);
    }


    private void Update()
    {
        MoveProjectile();
    }


    private void MoveProjectile()
    {
        // =====================================================
        // HOMING
        // =====================================================

        if (isHoming && target != null)
        {
            Vector3 direction =
                (target.position - transform.position).normalized;

            transform.position +=
                direction *
                speed *
                Time.deltaTime;

            if (direction != Vector3.zero)
            {
                transform.forward = direction;
            }

            return;
        }


        // =====================================================
        // STRAIGHT
        // =====================================================

        transform.position +=
            transform.forward *
            speed *
            Time.deltaTime;
    }


    private void OnTriggerEnter(Collider other)
    {
        EnemyHealth enemyHealth =
            other.GetComponentInParent<EnemyHealth>();


        if (enemyHealth == null)
            return;


        // Prevent hitting the same enemy repeatedly
        // unless we eventually want special multi-hit behavior.
        if (hitEnemies.Contains(enemyHealth))
            return;


        hitEnemies.Add(enemyHealth);


        // =====================================================
        // DAMAGE
        // =====================================================

        if (ownerStats == null)
        {
            Debug.LogError(
                "Projectile has no owner VesselStats!"
            );

            return;
        }


        if (ownerStats.vesselData == null)
        {
            Debug.LogError(
                "Projectile owner's VesselData is missing!"
            );

            return;
        }


        float damage =
            ownerStats.ATK *
            damageMultiplier;


        enemyHealth.TakeDamage(
            Mathf.RoundToInt(damage),
            ownerStats.vesselData.damageType,
            ownerStats.vesselData.element
        );


        Debug.Log(
            "Projectile hit " +
            other.gameObject.name +
            " for " +
            Mathf.RoundToInt(damage) +
            " damage!"
        );


        // =====================================================
        // IMPACT
        // =====================================================

        if (!isPiercing)
        {
            HandleImpact();
        }
    }


    private void HandleImpact()
    {
        if (isExploding)
        {
            Explode();
        }


        Destroy(gameObject);
    }


    private void Explode()
    {
        Debug.Log(
            "Projectile exploded!"
        );

        // Explosion AoE + knockback
        // will be added next.
    }
}