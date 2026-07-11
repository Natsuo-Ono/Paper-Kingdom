using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyInstance
{
    [Header("Enemy")]
    public Enemy enemyData;

    [Header("Progression")]
    public int level;

    [Header("Stats")]
    public int maxHP;
    public int currentHP;

    public int attack;
    public int defense;

    public int currentShield;

    [Header("Battle")]
    public int ultimateCharge;

    public bool isAlive = true;

    public bool isDefending = false;

    [Header("Status Effects")]
    public List<ActiveStatus> activeStatuses = new();

    public int expReward;
    public int goldReward;

    // Helper
    public string Name => enemyData.enemyName;

    public EnemyInstance(Enemy enemy, int enemyLevel)
    {
        enemyData = enemy;
        level = enemyLevel;

        RefreshStats();
        RefreshRewards();

        currentHP = maxHP;
        ultimateCharge = 0;
    }

    public void RefreshStats()
    {
        if (enemyData.useFixedStats)
        {
            maxHP = enemyData.fixedHP;
            attack = enemyData.fixedAttack;
            defense = enemyData.fixedDefense;

            currentShield = enemyData.fixedShield;
        }
        else
        {
            maxHP = LevelSystem.GetMaxHP(enemyData, level);
            attack = LevelSystem.GetAttack(enemyData, level);
            defense = LevelSystem.GetDefense(enemyData, level);
        }
    }

    public void Heal(int amount)
    {
        if (!isAlive)
            return;

        currentHP += amount;

        if (currentHP > maxHP)
            currentHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        if (!isAlive)
            return;

        if (isDefending)
            damage = Mathf.CeilToInt(damage * 0.3f);

        // Shield absorbs damage first
        if (currentShield > 0)
        {
            if (damage <= currentShield)
            {
                currentShield -= damage;
                return;
            }

            damage -= currentShield;
            currentShield = 0;
        }

        currentHP -= damage;

        if (currentHP <= 0)
        {
            currentHP = 0;
            isAlive = false;
        }
    }

    public void GainUltimateCharge(int amount = 1)
    {
        ultimateCharge += amount;

        if (ultimateCharge > enemyData.ultimate.ultimateCharge)
            ultimateCharge = enemyData.ultimate.ultimateCharge;
    }

    public void ResetUltimateCharge()
    {
        ultimateCharge = 0;
    }

    public void StartTurn()
    {
        isDefending = false;
    }

    public void RemoveNegativeStatuses()
    {
        activeStatuses.RemoveAll(status =>
            StatusHelper.IsNegative(status.status));
    }

    public void RefreshRewards()
    {
        if (enemyData.useFixedRewards)
        {
            expReward = enemyData.fixedEXP;
            goldReward = enemyData.fixedGold;
        }
        else
        {
            expReward =
                LevelSystem.GetEXP(enemyData, level);

            goldReward =
                LevelSystem.GetGold(enemyData, level);
        }
    }
}