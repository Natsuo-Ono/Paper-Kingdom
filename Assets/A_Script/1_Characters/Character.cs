using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Paper Kingdom/Character")]
public class Character : ScriptableObject
{
    [Header("Information")]
    public string characterName;

    [TextArea]
    public string description;

    public CharacterClass characterClass;
    public Element element;

    [Header("UI")]
    public Sprite icon;          // Small icon for menus
    public Sprite portrait;      // Character menu artwork
    public Sprite battleSprite;  // Full-body sprite in battle

    [Header("Base Stats")]
    public int baseHP;
    public int baseAttack;
    public int baseDefense;

    [Header("Abilities")]
    public Skill basicAttack;
    public Skill ultimate;

    [Header("Passive")]
    public Passive passive;

    public int level = 1;

    /**
    public int currentEXP;

    public int RequiredEXP()
    {
        return level * 100;
    }
    **/
}