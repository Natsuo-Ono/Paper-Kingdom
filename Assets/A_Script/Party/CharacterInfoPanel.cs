using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoPanel : MonoBehaviour
{
    public static CharacterInfoPanel Instance;

    [Header("UI")]
    public Image portrait;

    public TMP_Text nameText;
    public TMP_Text titleText;
    public TMP_Text descriptionText;

    [Header("Progression")]
    public TMP_Text levelText;
    public Image expBar;
    public TMP_Text expText;

    [Header("Stats")]
    public TMP_Text hpText;
    public TMP_Text atkText;
    public TMP_Text defText;

    [Header("Element")]
    public ElementDatabase elementDatabase;

    public Image elementIcon;

    private CharacterInstance currentCharacter;

    void Awake()
    {
        Instance = this;
    }

    public void SetCharacter(CharacterInstance character)
    {
        currentCharacter = character;

        portrait.sprite = character.characterData.portrait;

        nameText.text = character.characterData.characterName;

        levelText.text = $"Lv. {character.level}";

        expBar.fillAmount =
            (float)character.currentEXP /
            LevelSystem.GetRequiredEXP(character.level);

        expText.text =
            $"{character.currentEXP}/{LevelSystem.GetRequiredEXP(character.level)}";

        hpText.text = character.maxHP.ToString();
        atkText.text = character.attack.ToString();
        defText.text = character.defense.ToString();

        ElementData data =
            elementDatabase.Get(character.characterData.element);

        if (data != null)
        {
            elementIcon.sprite = data.icon;
            elementIcon.color = data.color;
        }

        ShowHero();
    }


    public void ShowHero()
    {
        if (currentCharacter == null)
            return;

        Character data = currentCharacter.characterData;

        nameText.text = data.characterName;
        titleText.text = "";
        descriptionText.text = data.description;
    }

    public void ShowPassive()
    {
        if (currentCharacter == null)
            return;

        Character data = currentCharacter.characterData;

        nameText.text = data.characterName;
        titleText.text = data.passive.passiveName;
        descriptionText.text = data.passive.description;
    }

    public void ShowSkill()
    {
        if (currentCharacter == null)
            return;

        Character data = currentCharacter.characterData;

        nameText.text = data.characterName;
        titleText.text = data.basicAttack.skillName;
        descriptionText.text = data.basicAttack.description;
    }

    public void ShowUltimate()
    {
        if (currentCharacter == null)
            return;

        Character data = currentCharacter.characterData;

        nameText.text = data.characterName;
        titleText.text = data.ultimate.skillName;
        descriptionText.text = data.ultimate.description;
    }

    public void AddCurrentCharacter()
    {
        if (currentCharacter == null)
            return;

        PartyManager.Instance.AddCharacter(currentCharacter);

        Debug.Log(PartyManager.Instance.currentParty.Count);

        foreach (var c in PartyManager.Instance.currentParty)
            Debug.Log(c.characterData.characterName);
    }
}