using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleEnemySlot : MonoBehaviour
{
    [Header("UI")]
    public Image portrait;
    public Image highlight;

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
    public EnemyInstance enemy;

    public void Setup(EnemyInstance instance)
    {
        gameObject.SetActive(true);

        enemy = instance;

        portrait.sprite = instance.enemyData.icon;

        levelText.text = $"Lv.{enemy.level}";
        nameText.text = enemy.Name;

        Refresh();
    }

    public void Refresh()
    {
        if (!enemy.isAlive)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        levelText.text = $"Lv.{enemy.level}";
        nameText.text = enemy.Name;

        hpText.text = $"{enemy.currentHP}/{enemy.maxHP}";

        hpBar.fillAmount =
            (float)enemy.currentHP / enemy.maxHP;

        shieldBar.fillAmount =
            enemy.maxHP == 0 ? 0 :
            (float)enemy.currentShield / enemy.maxHP;

        ultimateBar.fillAmount =
            (float)enemy.ultimateCharge /
            enemy.enemyData.ultimate.ultimateCharge;
    }

    public void OnClick()
    {
        if (BattleManager.Instance.currentState ==
            BattleState.PlayerChooseEnemy)
        {
            BattleManager.Instance.SelectEnemy(this);
        }
    }

    public void SetHighlight(bool value)
    {
        highlight.enabled = value;
    }
}