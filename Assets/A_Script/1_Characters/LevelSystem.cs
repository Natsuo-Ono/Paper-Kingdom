using UnityEngine;

public static class LevelSystem
{
    // EXP needed to level up
    public static int GetRequiredEXP(int level)
    {
        return 50 + ((level - 1) * 25);
    }

    // HP Formula
    public static int GetMaxHP(Character character, int level)
    {
        return character.baseHP + (GetHPGrowth(character.characterClass) * (level - 1));
    }

    // ATK Formula
    public static int GetAttack(Character character, int level)
    {
        return character.baseAttack + (GetAttackGrowth(character.characterClass) * (level - 1));
    }

    // DEF Formula
    public static int GetDefense(Character character, int level)
    {
        return character.baseDefense + (GetDefenseGrowth(character.characterClass) * (level - 1));
    }

    // HP Growth
    private static int GetHPGrowth(CharacterClass characterClass)
    {
        switch (characterClass)
        {
            case CharacterClass.Tank:
                return 42;

            case CharacterClass.Fighter:
                return 26;

            case CharacterClass.Mage:
                return 18;

            case CharacterClass.Support:
                return 16;

            case CharacterClass.Curser:
                return 20;

            default:
                return 0;
        }
    }

    // ATK Growth
    private static int GetAttackGrowth(CharacterClass characterClass)
    {
        switch (characterClass)
        {
            case CharacterClass.Tank:
                return 11;

            case CharacterClass.Fighter:
                return 15;

            case CharacterClass.Mage:
                return 14;

            case CharacterClass.Support:
                return 12;

            case CharacterClass.Curser:
                return 13;

            default:
                return 0;
        }
    }

    // DEF Growth
    private static int GetDefenseGrowth(CharacterClass characterClass)
    {
        switch (characterClass)
        {
            case CharacterClass.Tank:
                return 8;

            case CharacterClass.Fighter:
                return 5;

            case CharacterClass.Mage:
                return 2;

            case CharacterClass.Support:
                return 3;

            case CharacterClass.Curser:
                return 3;

            default:
                return 0;
        }
    }

    //====================
    // ENEMY REWARDS
    //====================

    public static int GetEXP(Enemy enemy, int level)
    {
        return Mathf.RoundToInt(
            enemy.baseEXP * Mathf.Pow(level, 0.55f)
        );
    }

    public static int GetGold(Enemy enemy, int level)
    {
        return Mathf.RoundToInt(
            enemy.baseGold * Mathf.Pow(level, 0.45f)
        );
    }

    // ENEMY
    public static int GetMaxHP(Enemy enemy, int level)
    {
        return enemy.baseHP + GetHPGrowth(enemy.characterClass) * (level - 1);
    }

    public static int GetAttack(Enemy enemy, int level)
    {
        return enemy.baseAttack + GetAttackGrowth(enemy.characterClass) * (level - 1);
    }

    public static int GetDefense(Enemy enemy, int level)
    {
        return enemy.baseDefense +
               GetDefenseGrowth(enemy.characterClass) * (level - 1);
    }
}