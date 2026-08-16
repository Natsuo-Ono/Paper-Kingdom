using UnityEngine;

public class VesselTargeting : MonoBehaviour
{
    [Header("Targeting")]
    public float attackRange = 10f;
    public LayerMask enemyLayer;

    public Transform CurrentTarget { get; private set; }

    public Transform FindNearestEnemy()
    {
        Collider[] enemies = Physics.OverlapSphere(
            transform.position,
            attackRange,
            enemyLayer
        );

        Transform nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Collider enemy in enemies)
        {
            float distance = Vector3.Distance(
                transform.position,
                enemy.transform.position
            );

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        CurrentTarget = nearestEnemy;

        return CurrentTarget;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            transform.position,
            attackRange
        );
    }
}