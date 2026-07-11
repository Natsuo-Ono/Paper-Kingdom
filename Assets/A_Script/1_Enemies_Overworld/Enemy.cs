using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Paper Kingdom/Enemy")]
public class Enemy : ScriptableObject
{
    [Header("Information")]
    public EnemyID enemyID;

    public string enemyName;

    [TextArea]
    public string description;

    [Header("Classification")]
    public EnemyRank enemyRank;

    public CharacterClass characterClass;

    public Element element;

    [Header("UI")]
    public Sprite icon;

    // Optional later
    // public Sprite portrait;

    [Header("Base Stats")]
    public int baseHP;
    public int baseAttack;
    public int baseDefense;

    [Header("Override Stats")]

    public bool useFixedStats;

    public int fixedHP;
    public int fixedAttack;
    public int fixedDefense;

    public int fixedShield;

    [Header("Abilities")]
    public Skill basicAttack;

    public Skill ultimate;

    [Header("Passive")]
    public Passive passive;

    [Header("Rewards")]

    public bool useFixedRewards;

    public int fixedEXP;
    public int fixedGold;

    public int baseEXP;
    public int baseGold;
}