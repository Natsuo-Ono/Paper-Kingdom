using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BattlePlayerSlot : MonoBehaviour
{
    [Header("UI")]
    public Image portrait;

    [Header("Colors")]
    public Color activeColor = Color.white;
    public Color waitingColor = Color.gray;
    public Color finishedColor = new Color(0.35f, 0.35f, 0.35f);

    [Header("Info")]
    public TMP_Text levelText;
    public TMP_Text nameText;

    [Header("Status")]
    public Transform statusHolder;

    public Image hpBar;
    public Image shieldBar;
    public Image ultimateBar;

    public TMP_Text hpText;

    public Button button;

    [HideInInspector]
    public CharacterInstance character;

    [HideInInspector]
    public bool hasActed;

    public void Setup(CharacterInstance instance)
    {
        Debug.Log("SETUP CALLED");

        if (instance == null)
        {
            Debug.LogError("INSTANCE IS NULL");
            return;
        }

        Debug.Log(instance.characterData.characterName);

        character = instance;

        hasActed = false;

        Debug.Log("Portrait exists? " + (portrait != null));
        Debug.Log("Icon exists? " + (instance.characterData.icon != null));

        portrait.sprite = instance.characterData.icon;

        levelText.text = $"Lv.{instance.level}";
        nameText.text = instance.characterData.characterName;

        Refresh();
    }

    public void Refresh()
    {
        hpText.text = $"{character.currentHP}/{character.maxHP}";

        hpBar.fillAmount =
            (float)character.currentHP / character.maxHP;

        // Shield later
        shieldBar.fillAmount = 0f;

        // Ultimate later
        ultimateBar.fillAmount = 0f;

        ultimateBar.fillAmount =
        (float)character.ultimateCharge /
        character.characterData.ultimate.ultimateCharge;

        levelText.text = $"Lv.{character.level}";
        nameText.text = character.characterData.characterName;

        if (!character.isAlive)
        {
            gameObject.SetActive(false);
            return;
        }
    }

    public void OnClick()
    {
        switch (BattleManager.Instance.currentState)
        {
            case BattleState.PlayerChooseCharacter:
                BattleManager.Instance.SelectCharacter(this);
                break;

            case BattleState.PlayerChooseAlly:
                BattleManager.Instance.SelectAlly(this);
                break;
        }
    }

    public void SetCurrent()
    {
        portrait.color = activeColor;
    }

    public void SetWaiting()
    {
        portrait.color = waitingColor;
    }

    public void SetFinished()
    {
        portrait.color = finishedColor;
    }

    public void UpdateVisual()
    {
        if (!character.isAlive)
        {
            gameObject.SetActive(false);
            return;
        }

        if (hasActed)
        {
            portrait.color = finishedColor;
        }
        else
        {
            portrait.color = waitingColor;
        }

        Refresh();
    }
}