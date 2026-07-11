using UnityEngine;

public class ActionPanel : MonoBehaviour
{
    public static ActionPanel Instance;

    public RectTransform panel;

    public float speed = 10f;

    Vector2 targetPosition;

    void Awake()
    {
        Instance = this;

        targetPosition = new Vector2(
            panel.anchoredPosition.x,
            -270);

        panel.anchoredPosition = targetPosition;
    }

    void Update()
    {
        panel.anchoredPosition = Vector2.Lerp(
            panel.anchoredPosition,
            targetPosition,
            Time.deltaTime * speed);
    }

    public void Show()
    {
        targetPosition = new Vector2(
            panel.anchoredPosition.x,
            -90);
    }

    public void Hide()
    {
        targetPosition = new Vector2(
            panel.anchoredPosition.x,
            -270);
    }

    public void HoverAttack()
    {
        CharacterInstance c =
            BattleManager.Instance.selectedCharacter.character;

        ActionDescriptionUI.Instance.Preview(
            c.characterData.basicAttack.skillName,
            c.characterData.basicAttack.description
        );
    }

    public void ExitAttack()
    {
        ActionDescriptionUI.Instance.HidePreview();
    }

    public void AttackButton()
    {
        if (BattleManager.Instance.selectedCharacter == null)
            return;

        CharacterInstance c =
            BattleManager.Instance.selectedCharacter.character;

        ActionDescriptionUI.Instance.Lock(
            c.characterData.basicAttack.skillName,
            c.characterData.basicAttack.description
        );

        BattleManager.Instance.selectedSkill =
            c.characterData.basicAttack;

        BattleManager.Instance.DecideTarget();
    }

    public void HoverDefend()
    {
        ActionDescriptionUI.Instance.Preview(
    "Defend", "Reduce incoming damage by 75% until your next turn.\nGain Ultimate Charge.");
    }

    public void ExitDefend()
    {
        ActionDescriptionUI.Instance.HidePreview();
    }

    public void DefendButton()
    {
        ActionDescriptionUI.Instance.Lock(
        "Defend",
        "Reduce incoming damage by 75% until your next turn.\nGain Ultimate Charge."
    );

        if (BattleManager.Instance.selectedCharacter == null)
            return;

        BattleManager.Instance.Defend();
    }

    public void HoverUltimate()
    {
        if (BattleManager.Instance.selectedCharacter == null)
            return;

        CharacterInstance c =
            BattleManager.Instance.selectedCharacter.character;

        ActionDescriptionUI.Instance.Preview(
            c.characterData.ultimate.skillName,
            c.characterData.ultimate.description
        );
    }

    public void ExitUltimate()
    {
        ActionDescriptionUI.Instance.HidePreview();
    }

    public void UltimateButton()
    {
        if (BattleManager.Instance.selectedCharacter == null)
            return;

        CharacterInstance c =
            BattleManager.Instance.selectedCharacter.character;

        ActionDescriptionUI.Instance.Lock(
            c.characterData.ultimate.skillName,
            c.characterData.ultimate.description
        );

        BattleManager.Instance.SelectUltimate();
    }
}