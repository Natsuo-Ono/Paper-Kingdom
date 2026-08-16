using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    [Header("Damage Number")]
    public GameObject damageNumberPrefab;
    public Vector3 damageNumberOffset =
        new Vector3(0f, 2f, 0f);

    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(
        int damage,
        DamageType damageType,
        ElementType elementType
    )
    {
        currentHealth -= damage;

        // Show / refresh health bar
        EnemyHealthBar healthBar =
            GetComponentInChildren<EnemyHealthBar>();

        if (healthBar != null)
        {
            healthBar.ShowHealthBar();
        }

        SpawnDamageNumber(
            damage,
            damageType,
            elementType
        );

        Debug.Log(
            gameObject.name +
            " took " +
            damage +
            " damage. HP: " +
            currentHealth
        );

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void SpawnDamageNumber(
        int damage,
        DamageType damageType,
        ElementType elementType
    )
    {
        if (damageNumberPrefab == null)
            return;

        Vector3 spawnPosition =
            transform.position +
            damageNumberOffset;

        GameObject damageObject = Instantiate(
            damageNumberPrefab,
            spawnPosition,
            Quaternion.identity
        );

        DamageNumber damageNumber =
            damageObject.GetComponent<DamageNumber>();

        if (damageNumber != null)
        {
            damageNumber.Setup(
                damage,
                damageType,
                elementType
            );
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}