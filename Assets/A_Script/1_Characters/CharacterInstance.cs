using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterInstance
{
    [Header("Character")]
    public Character characterData;

    [Header("Progression")]
    public int level = 1;
    public int currentEXP = 0;

    [Header("Stats")]
    public int maxHP;
    public int currentHP;

    public int attack;
    public int defense;

    [Header("Battle")]
    public int ultimateCharge;

    public bool isAlive = true;
    public bool isDefending = false;

    [Header("Status Effects")]
    public List<ActiveStatus> activeStatuses = new();

    public bool hasTakenTurn;

    // Helper Property
    public string Name => characterData.characterName;

    public CharacterInstance(Character character)
    {
        characterData = character;

        level = 1;
        currentEXP = 0;

        RefreshStats();

        currentHP = maxHP;
        ultimateCharge = 0;
    }

    public void RefreshStats()
    {
        maxHP = LevelSystem.GetMaxHP(characterData, level);
        attack = LevelSystem.GetAttack(characterData, level);
        defense = LevelSystem.GetDefense(characterData, level);

        currentHP = Mathf.Min(currentHP, maxHP);
    }

    public void Heal(int amount)
    {
        if (!isAlive)
            return;

        currentHP = Mathf.Min(currentHP + amount, maxHP);
    }

    public void TakeDamage(int damage)
    {
        if (!isAlive)
            return;

        if (isDefending)
            damage = Mathf.CeilToInt(damage * 0.3f);

        currentHP -= damage;

        if (currentHP <= 0)
        {
            currentHP = 0;
            isAlive = false;
        }
    }

    public void Revive(float hpPercent)
    {
        if (isAlive)
            return;

        isAlive = true;

        currentHP = Mathf.CeilToInt(maxHP * hpPercent);
    }

    public void GainUltimateCharge(int amount = 1)
    {
        ultimateCharge += amount;

        if (ultimateCharge > characterData.ultimate.ultimateCharge)
            ultimateCharge = characterData.ultimate.ultimateCharge;
    }

    public void ResetUltimateCharge()
    {
        ultimateCharge = 0;
    }

    public void StartTurn()
    {
        isDefending = false;
    }

    public void GainEXP(int amount)
    {
        currentEXP += amount;

        while (level < 499 &&
               currentEXP >= LevelSystem.GetRequiredEXP(level))
        {
            currentEXP -= LevelSystem.GetRequiredEXP(level);

            level++;

            RefreshStats();

            currentHP = maxHP;

            Debug.Log($"{characterData.characterName} leveled to {level}");
            Debug.Log($"HP={maxHP} ATK={attack} DEF={defense}");
        }
    }

    public void RemoveNegativeStatuses()
    {
        activeStatuses.RemoveAll(status =>
            StatusHelper.IsNegative(status.status));
    }

}