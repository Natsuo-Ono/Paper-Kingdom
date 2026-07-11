using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    private int enemyTurnIndex = 0;
    public TMP_Text turnText;

    public Skill selectedSkill;

    [Header("Turn")]
    public int currentTurn = 1;
    public int maxTurns = 50;

    [Header("Retreat")]
    public TMP_Text retreatText;

    private int retreatAttempts = 0;

    [Header("Scene")]
    public GameObject battleUI;

    public BattleUI battleUIManager;

    public Camera explorationCamera;
    public Camera battleCamera;

    [Header("State")]
    public BattleState currentState;

    [Header("Battle")]
    public List<CharacterInstance> allies = new();
    public List<EnemyInstance> enemies = new();

    [Header("Selection")]
    public BattlePlayerSlot selectedCharacter;

    public BattlePlayerSlot selectedAlly;

    [Header("Enemy Selection")]
    public BattleEnemySlot selectedEnemy;

    [Header("Victory")]
    public CanvasGroup victoryPanel;

    [Header("Defeat")]
    public CanvasGroup defeatPanel;

    [Header("Battle Result")]
    public RectTransform resultPanel;

    public TMP_Text expEarnedText;
    public TMP_Text goldEarnedText;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (currentState != BattleState.PlayerChooseCharacter)
            return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(RetreatRoutine());
        }
    }

    void RefreshTurnUI()
    {
        turnText.text = $"Turn {currentTurn}";
    }

    public void StartBattle(EnemyFormation formation)
    {
        // reset past UI
        ResetBattleResultUI();

        currentState = BattleState.StartBattle;

        Debug.Log("Battle Started");

        currentTurn = 1;
        RefreshTurnUI();

        // Freeze player
        FindObjectOfType<PlayerMovement>().enabled = false;

        // Cameras
        explorationCamera.gameObject.SetActive(false);
        battleCamera.gameObject.SetActive(true);

        // Battle UI
        battleUI.SetActive(true);

        allies.Clear();
        enemies.Clear();

        foreach (CharacterInstance character in PartyManager.Instance.currentParty)
        {
            allies.Add(character);

            character.ultimateCharge = 0;
            character.isDefending = false;
            character.hasTakenTurn = false;

            Debug.Log("===== ALLIES =====");

            foreach (CharacterInstance c in allies)
            {
                Debug.Log(c.characterData.characterName);
            }
        }

        AddEnemy(formation.position1);
        AddEnemy(formation.position2);
        AddEnemy(formation.position3);
        AddEnemy(formation.position4);

        battleUIManager.SetupBattle();

        Debug.Log("=== PLAYER SLOTS ===");

        foreach (BattlePlayerSlot slot in battleUIManager.playerSlots)
        {
            if (!slot.gameObject.activeSelf)
                continue;

            if (slot.character == null)
            {
                Debug.Log("NULL CHARACTER");
            }
            else
            {
                Debug.Log(slot.character.characterData.characterName);
            }
        }

        currentState = BattleState.PlayerChooseCharacter;

        foreach (BattlePlayerSlot slot in battleUIManager.playerSlots)
        {
            if (slot.gameObject.activeSelf)
                slot.SetWaiting();
        }

        SelectCharacter(battleUIManager.playerSlots[0]);
    }

    void AddEnemy(EnemySlot slot)
    {
        if (slot.enemy == null)
            return;

        enemies.Add(new EnemyInstance(slot.enemy, slot.level));
    }

    public void EndBattle(bool victory)
    {
        currentState = victory
            ? BattleState.Victory
            : BattleState.Defeat;

        Debug.Log(victory ? "YOU WIN" : "YOU LOSE");

        battleUI.SetActive(false);

        explorationCamera.gameObject.SetActive(true);
        battleCamera.gameObject.SetActive(false);

        FindObjectOfType<PlayerMovement>().enabled = true;

        EncounterManager.Instance.BattleFinished();
    }

    public void SelectCharacter(BattlePlayerSlot slot)
    {
        if (currentState != BattleState.PlayerChooseCharacter)
            return;

        if (slot.hasActed)
            return;

        if (!slot.character.isAlive)
            return;

        // Reset everyone's color first
        foreach (BattlePlayerSlot playerSlot in battleUIManager.playerSlots)
        {
            if (!playerSlot.gameObject.activeSelf)
                continue;

            if (playerSlot.hasActed)
                playerSlot.SetFinished();
            else
                playerSlot.SetWaiting();
        }

        selectedCharacter = slot;

        // Reset previous description
        ActionDescriptionUI.Instance.Unlock();

        // Highlight the selected one
        selectedCharacter.SetCurrent();

        ActionPanel.Instance.Show();
    }

    public void SelectAlly(BattlePlayerSlot slot)
    {
        if (currentState != BattleState.PlayerChooseAlly)
            return;

        if (!slot.character.isAlive)
            return;

        selectedAlly = slot;

        ExecutePlayerAction();
    }

    public void SelectEnemy(BattleEnemySlot slot)
    {
        if (currentState != BattleState.PlayerChooseEnemy)
            return;

        if (!slot.enemy.isAlive)
            return;

        if (selectedEnemy != null)
            selectedEnemy.SetHighlight(false);

        selectedEnemy = slot;
        selectedEnemy.SetHighlight(true);

        ExecutePlayerAction();
    }

    public void DeselectCharacter()
    {
        if (selectedCharacter == null)
            return;

        //selectedCharacter.SetHighlight(false);
        selectedCharacter.SetFinished();

        selectedCharacter = null;

        ActionPanel.Instance.Hide();
    }

    public void DecideTarget()
    {
        switch (selectedSkill.targetType)
        {
            case TargetType.SingleEnemy:

                currentState = BattleState.PlayerChooseEnemy;

                EnableEnemyButtons(true);
                EnablePlayerButtons(false);

                break;

            case TargetType.SingleAlly:

                currentState = BattleState.PlayerChooseAlly;

                EnableEnemyButtons(false);
                EnablePlayerButtons(true);

                break;

            case TargetType.Self:

                selectedAlly = selectedCharacter;

                ExecutePlayerAction();

                break;

            case TargetType.AllEnemies:

                currentState = BattleState.PlayerChooseEnemy;

                EnableEnemyButtons(true);
                EnablePlayerButtons(false);

                foreach (BattleEnemySlot slot in battleUIManager.enemySlots)
                {
                    if (slot.gameObject.activeSelf)
                        slot.SetHighlight(true);
                }

                break;

            case TargetType.AllAllies:

                currentState = BattleState.PlayerChooseAlly;

                EnableEnemyButtons(false);
                EnablePlayerButtons(true);

                foreach (BattlePlayerSlot slot in battleUIManager.playerSlots)
                {
                    if (!slot.gameObject.activeSelf)
                        continue;

                    slot.SetCurrent();   // brighten everyone
                }

                break;
        }
    }

    void ExecutePlayerAction()
    {
        CharacterInstance attacker = selectedCharacter.character;

        switch (selectedSkill.skillType)
        {
            case SkillType.Damage:
                ExecuteDamage();
                break;

            case SkillType.Heal:
                ExecuteHeal();
                break;

                /*
                case SkillType.Buff:
                    ExecuteBuff();
                    break;

                case SkillType.Debuff:
                    ExecuteDebuff();
                    break;

                case SkillType.Revive:
                    ExecuteRevive();
                    break;
                */
        }

        if (selectedSkill == attacker.characterData.ultimate)
            attacker.ResetUltimateCharge();
        else
            attacker.GainUltimateCharge();

        // Character has finished its turn
        selectedCharacter.hasActed = true;
        selectedCharacter.Refresh();
        selectedCharacter.SetFinished();

        Debug.Log($"Finished Slot: {selectedCharacter.name}");

        EnableEnemyButtons(true);
        EnablePlayerButtons(true);

        selectedCharacter = null;
        selectedEnemy = null;
        selectedSkill = null;
        selectedAlly = null;

        ActionPanel.Instance.Hide();

        currentState = BattleState.PlayerChooseCharacter;

        CheckPlayerTurnFinished();

        battleUIManager.RefreshPlayerUI();
        battleUIManager.RefreshEnemyUI();
    }

    void ExecuteDamage()
    {
        CharacterInstance attacker = selectedCharacter.character;

        int baseDamage = Mathf.RoundToInt(
            attacker.attack *
            (selectedSkill.multiplier / 100f)
        );

        switch (selectedSkill.targetType)
        {
            case TargetType.SingleEnemy:

                float multiplier = ElementSystem.GetMultiplier(
                    attacker.characterData.element,
                    selectedEnemy.enemy.enemyData.element
                );

                int damage = Mathf.RoundToInt(baseDamage * multiplier);

                selectedEnemy.enemy.TakeDamage(damage);

                if (multiplier > 1f)
                {
                    FloatingTextManager.Instance.SpawnMessage(
                        selectedEnemy.GetComponent<RectTransform>(),
                        "Effective!",
                        new Color(0.65f, 0.05f, 0.05f)
                    );
                }
                else if (multiplier < 1f)
                {
                    FloatingTextManager.Instance.SpawnMessage(
                        selectedEnemy.GetComponent<RectTransform>(),
                        "Resisted!",
                        Color.cyan
                    );

                }

                FloatingTextManager.Instance.Spawn(
                    selectedEnemy.GetComponent<RectTransform>(),
                    damage,
                    Color.red,
                    false
                );

                selectedEnemy.Refresh();

                Debug.Log(attacker.characterData.characterName +
                    " dealt " + damage +
                    " damage to " +
                    selectedEnemy.enemy.Name);

                break;

            case TargetType.AllEnemies:

                foreach (BattleEnemySlot slot in battleUIManager.enemySlots)
                {
                    if (!slot.gameObject.activeSelf)
                        continue;

                    if (!slot.enemy.isAlive)
                        continue;

                    float aoeMultiplier = ElementSystem.GetMultiplier(
                        attacker.characterData.element,
                        slot.enemy.enemyData.element
                    );

                    int finalDamage = Mathf.RoundToInt(
                        baseDamage * aoeMultiplier
                    );

                    slot.enemy.TakeDamage(finalDamage);

                    if (aoeMultiplier > 1f)
                    {
                        FloatingTextManager.Instance.SpawnMessage(
                            slot.GetComponent<RectTransform>(),
                            "Effective!",
                            new Color(0.65f, 0.05f, 0.05f)
                        );
                    }
                    else if (aoeMultiplier < 1f)
                    {
                        FloatingTextManager.Instance.SpawnMessage(
                            slot.GetComponent<RectTransform>(),
                            "Resisted!",
                            Color.cyan
                        );
                    }

                    FloatingTextManager.Instance.Spawn(
                        slot.GetComponent<RectTransform>(),
                        finalDamage,
                        Color.red,
                        false
                    );

                    slot.Refresh();
                }

                Debug.Log(attacker.characterData.characterName +
                    " attacked all enemies.");

                break;
        }

        RefreshBattleUI();
        CheckBattleFinished();
    }

    void ExecuteHeal()
    {
        CharacterInstance healer = selectedCharacter.character;

        int heal = Mathf.RoundToInt(
            healer.attack *
            (selectedSkill.multiplier / 100f)
        );

        switch (selectedSkill.targetType)
        {
            case TargetType.Self:

                healer.Heal(heal);

                FloatingTextManager.Instance.Spawn(
                    selectedCharacter.GetComponent<RectTransform>(),
                    heal,
                    Color.green,
                    true
                );

                break;

            case TargetType.SingleAlly:

                selectedAlly.character.Heal(heal);

                FloatingTextManager.Instance.Spawn(
                    selectedAlly.GetComponent<RectTransform>(),
                    heal,
                    Color.green,
                    true
                );

                break;

            case TargetType.AllAllies:

                foreach (BattlePlayerSlot slot in battleUIManager.playerSlots)
                {
                    if (!slot.gameObject.activeSelf)
                        continue;

                    if (!slot.character.isAlive)
                        continue;

                    slot.character.Heal(heal);

                    FloatingTextManager.Instance.Spawn(
                        slot.GetComponent<RectTransform>(),
                        heal,
                        Color.green,
                        true
                    );
                }

                break;
        }

        RefreshBattleUI();
    }

    public void Defend()
    {
        CharacterInstance defender = selectedCharacter.character;

        defender.isDefending = true;

        defender.GainUltimateCharge();

        selectedCharacter.hasActed = true;
        selectedCharacter.Refresh();
        selectedCharacter.SetFinished();

        RefreshBattleUI();

        EnableEnemyButtons(true);
        EnablePlayerButtons(true);

        selectedCharacter = null;
        selectedEnemy = null;
        selectedSkill = null;

        ActionPanel.Instance.Hide();

        currentState = BattleState.PlayerChooseCharacter;

        CheckPlayerTurnFinished();
    }

    public void SelectUltimate()
    {
        CharacterInstance attacker =
            selectedCharacter.character;

        if (attacker.ultimateCharge <
            attacker.characterData.ultimate.ultimateCharge)
        {
            Debug.Log("Ultimate not ready.");

            return;
        }

        selectedSkill =
            attacker.characterData.ultimate;

        DecideTarget();
    }

    void EnableEnemyButtons(bool value)
    {
        foreach (BattleEnemySlot slot in battleUIManager.enemySlots)
        {
            if (!slot.gameObject.activeSelf)
                continue;

            slot.button.interactable = value;
        }
    }

    void EnablePlayerButtons(bool value)
    {
        foreach (BattlePlayerSlot slot in battleUIManager.playerSlots)
        {
            if (!slot.gameObject.activeSelf)
                continue;

            slot.button.interactable = value;
        }
    }

    void SelectNextCharacter()
    {
        foreach (BattlePlayerSlot slot in battleUIManager.playerSlots)
        {
            if (!slot.gameObject.activeSelf)
                continue;

            if (slot.hasActed)
                continue;

            if (!slot.character.isAlive)
                continue;

            SelectCharacter(slot);
            return;
        }
    }

    void CheckPlayerTurnFinished()
    {
        foreach (BattlePlayerSlot slot in battleUIManager.playerSlots)
        {
            if (slot.gameObject.activeSelf &&
                !slot.hasActed &&
                slot.character.isAlive)
            {
                return;
            }
        }

        Debug.Log("Enemy Turn");

        currentState = BattleState.EnemyTurn;

        enemyTurnIndex = 0;

        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(0.5f);

        while (enemyTurnIndex < enemies.Count)
        {
            EnemyInstance enemy = enemies[enemyTurnIndex];

            if (enemy.isAlive)
            {
                if (enemy.ultimateCharge >= enemy.enemyData.ultimate.ultimateCharge)
                {
                    EnemyUseSkill(enemy, enemy.enemyData.ultimate, true);
                }
                else
                {
                    EnemyUseSkill(enemy, enemy.enemyData.basicAttack, false);
                }

                yield return new WaitForSeconds(0.8f);
            }

            enemyTurnIndex++;
        }

        currentTurn++;

        RefreshTurnUI();

        if (currentTurn > maxTurns)
        {
            SuddenDeath();
            yield break;
        }

        StartPlayerTurn();
    }

    void ShowDamage(CharacterInstance target, int damage)
    {
        foreach (BattlePlayerSlot slot in battleUIManager.playerSlots)
        {
            if (slot.character == target)
            {
                FloatingTextManager.Instance.Spawn(
                    slot.GetComponent<RectTransform>(),
                    damage,
                    Color.red,
                    false
                );

                break;
            }
        }
    }

    void EnemyUseSkill(EnemyInstance enemy, Skill skill, bool isUltimate)
    {
        int baseDamage = Mathf.RoundToInt(
            enemy.attack *
            (skill.multiplier / 100f)
        );

        switch (skill.targetType)
        {
            case TargetType.SingleEnemy:

                List<CharacterInstance> targets = new();

                foreach (CharacterInstance ally in allies)
                {
                    if (ally.isAlive)
                        targets.Add(ally);
                }

                if (targets.Count == 0)
                    return;

                CharacterInstance target =
                    targets[Random.Range(0, targets.Count)];

                float multiplier = ElementSystem.GetMultiplier(
                    enemy.enemyData.element,
                    target.characterData.element
                );

                int damage = Mathf.RoundToInt(
                    baseDamage * multiplier
                );

                if (target.isDefending)
                    damage = Mathf.RoundToInt(damage * 0.25f);

                target.TakeDamage(damage);

                ShowDamage(target, damage);

                Debug.Log($"{enemy.Name} used {skill.skillName} on {target.characterData.characterName}");

                break;

            case TargetType.AllEnemies:

                foreach (CharacterInstance ally in allies)
                {
                    if (!ally.isAlive)
                        continue;

                    float aoeMultiplier = ElementSystem.GetMultiplier(
                        enemy.enemyData.element,
                        ally.characterData.element
                    );

                    int finalDamage = Mathf.RoundToInt(
                        baseDamage * aoeMultiplier
                    );

                    if (ally.isDefending)
                        finalDamage = Mathf.RoundToInt(finalDamage * 0.25f);

                    ally.TakeDamage(finalDamage);

                    ShowDamage(ally, finalDamage);
                }

                Debug.Log($"{enemy.Name} used {skill.skillName} (AOE)");

                break;
        }

        if (isUltimate)
            enemy.ResetUltimateCharge();
        else
            enemy.GainUltimateCharge();

        RefreshBattleUI();
        CheckBattleFinished();
    }

    void StartPlayerTurn()
    {
        EnablePlayerButtons(true);
        EnableEnemyButtons(true);

        foreach (BattlePlayerSlot slot in battleUIManager.playerSlots)
        {
            if (!slot.gameObject.activeSelf)
                continue;

            slot.hasActed = false;
            slot.SetWaiting();
            slot.Refresh();

            foreach (CharacterInstance ally in allies)
            {
                ally.isDefending = false;
            }
        }

        currentState = BattleState.PlayerChooseCharacter;

        SelectCharacter(battleUIManager.playerSlots[0]);
    }

    public void RefreshBattleUI()
    {
        battleUIManager.RefreshPlayerUI();
        battleUIManager.RefreshEnemyUI();
    }

    void SuddenDeath()
    {
        Debug.Log("Turn limit reached!");

        foreach (CharacterInstance ally in allies)
        {
            if (ally.isAlive)
                ally.TakeDamage(999999);
        }

        RefreshBattleUI();

        LoseBattle();
    }

    IEnumerator RetreatRoutine()
    {
        currentState = BattleState.Retreating;

        ActionPanel.Instance.Hide();
        ActionDescriptionUI.Instance.Unlock();

        EnablePlayerButtons(false);
        EnableEnemyButtons(false);

        retreatText.gameObject.SetActive(true);

        string[] dots =
        {
        "",
        ".",
        ". .",
        ". . ."
    };

        for (int i = 0; i < 8; i++)
        {
            retreatText.text = "Retreating " + dots[i % dots.Length];

            yield return new WaitForSeconds(0.35f);
        }

        AttemptEscape();
    }

    float CalculateEscapeChance()
    {
        float playerAverage = 0f;

        foreach (CharacterInstance ally in allies)
        {
            playerAverage += ally.level;
        }

        playerAverage /= allies.Count;

        float enemyAverage = 0f;

        foreach (EnemyInstance enemy in enemies)
        {
            enemyAverage += enemy.level;
        }

        enemyAverage /= enemies.Count;

        float chance =
            50f +
            (playerAverage - enemyAverage) * 0.5f;

        chance -= retreatAttempts * 15f;

        return Mathf.Clamp(chance, 5f, 90f);
    }

    void AttemptEscape()
    {
        float chance = CalculateEscapeChance();

        float roll = Random.Range(0f, 100f);

        Debug.Log($"Escape Chance: {chance}% | Roll: {roll}");

        if (roll <= chance)
        {
            EscapeSuccess();
        }
        else
        {
            EscapeFailed();
        }
    }

    void EscapeSuccess()
    {
        retreatText.text = "Escaped!";

        retreatAttempts = 0;

        StartCoroutine(EscapeSuccessRoutine());
    }

    IEnumerator EscapeSuccessRoutine()
    {
        yield return new WaitForSeconds(1f);

        retreatText.gameObject.SetActive(false);

        EndBattle(false);
    }

    void EscapeFailed()
    {
        retreatText.text = "Couldn't Escape!";

        retreatAttempts++;

        StartCoroutine(EscapeFailedRoutine());
    }

    IEnumerator EscapeFailedRoutine()
    {
        yield return new WaitForSeconds(1f);

        retreatText.gameObject.SetActive(false);

        currentState = BattleState.EnemyTurn;

        enemyTurnIndex = 0;

        StartCoroutine(EnemyTurn());
    }

    void CheckBattleFinished()
    {
        if (currentState == BattleState.Victory ||
            currentState == BattleState.Defeat)
            return;

        bool allEnemiesDead = true;

        foreach (EnemyInstance enemy in enemies)
        {
            if (enemy.isAlive)
            {
                allEnemiesDead = false;
                break;
            }
        }

        if (allEnemiesDead)
        {
            WinBattle();
            return;
        }

        bool allPlayersDead = true;

        foreach (CharacterInstance ally in allies)
        {
            if (ally.isAlive)
            {
                allPlayersDead = false;
                break;
            }
        }

        if (allPlayersDead)
        {
            LoseBattle();
        }
    }

    IEnumerator ShowVictory()
    {
        victoryPanel.alpha = 0;
        victoryPanel.gameObject.SetActive(true);

        float t = 0;

        while (t < .5f)
        {
            t += Time.deltaTime;

            victoryPanel.alpha = t / .5f;

            yield return null;
        }

        yield return new WaitForSeconds(1f);

        t = .25f;

        while (t > 0)
        {
            t -= Time.deltaTime;

            victoryPanel.alpha = t / .25f;

            yield return null;
        }

        victoryPanel.gameObject.SetActive(false);
    }

    IEnumerator ShowDefeat()
    {
        defeatPanel.alpha = 0;
        defeatPanel.gameObject.SetActive(true);

        float t = 0;

        while (t < .5f)
        {
            t += Time.deltaTime;

            defeatPanel.alpha = t / .5f;

            yield return null;
        }

        yield return new WaitForSeconds(1f);
    }

    IEnumerator VictorySequence()
    {
        int totalEXP = 0;
        int totalGold = 0;

        foreach (EnemyInstance enemy in enemies)
        {
            totalEXP += enemy.expReward;
            totalGold += enemy.goldReward;
        }

        yield return StartCoroutine(ShowVictory());

        yield return StartCoroutine(
            ShowResults(totalEXP, totalGold));

        GiveRewards(totalEXP, totalGold);

        yield return new WaitForSeconds(3.0f);

        EndBattle(true);
    }

    IEnumerator DefeatSequence()
    {
        yield return StartCoroutine(ShowDefeat());

        yield return StartCoroutine(
            ShowResults(0, 0));

        yield return new WaitForSeconds(3.0f);

        EndBattle(false);
    }

    IEnumerator ShowResults(int exp, int gold)
    {
        Vector2 start = new Vector2(950, resultPanel.anchoredPosition.y);
        Vector2 end = new Vector2(350, resultPanel.anchoredPosition.y);

        resultPanel.anchoredPosition = start;

        float t = 0;

        while (t < 0.4f)
        {
            t += Time.deltaTime;

            resultPanel.anchoredPosition =
                Vector2.Lerp(start, end, t / .4f);

            yield return null;
        }

        // Show earned rewards first
        expEarnedText.text = exp.ToString();
        goldEarnedText.text = gold.ToString();

        // Let the player read them
        yield return new WaitForSeconds(1.25f);

        // Then animate
        yield return StartCoroutine(CountDown(expEarnedText, exp));

        yield return new WaitForSeconds(0.25f);

        yield return StartCoroutine(CountDown(goldEarnedText, gold));
    }

    IEnumerator CountDown(TMP_Text text, int value)
    {
        int current = value;

        while (current > 0)
        {
            text.text = current.ToString();

            current -= Mathf.Max(1, current / 15);

            if (current < 0)
                current = 0;

            yield return null;
        }

        text.text = "0";
    }

    void GiveRewards(int exp, int gold)
    {
        foreach (CharacterInstance ally in allies)
        {
            ally.GainEXP(exp);
        }

        RefreshBattleUI();

        // PlayerData.Instance.gold += gold; // later
    }

    void WinBattle()
    {
        if (currentState == BattleState.Victory)
            return;

        currentState = BattleState.Victory;

        StartCoroutine(VictorySequence());
    }

    void LoseBattle()
    {
        if (currentState == BattleState.Defeat)
            return;

        currentState = BattleState.Defeat;

        StartCoroutine(DefeatSequence());
    }

    void ResetBattleResultUI()
    {
        victoryPanel.gameObject.SetActive(false);
        victoryPanel.alpha = 0;

        defeatPanel.gameObject.SetActive(false);
        defeatPanel.alpha = 0;

        resultPanel.anchoredPosition =
            new Vector2(950, resultPanel.anchoredPosition.y);

        expEarnedText.text = "";
        goldEarnedText.text = "";
    }
}