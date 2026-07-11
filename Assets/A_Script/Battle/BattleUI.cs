using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BattleUI : MonoBehaviour
{
    public BattlePlayerSlot[] playerSlots;

    public BattleEnemySlot[] enemySlots;

    public void SetupBattle()
    {
        foreach (BattlePlayerSlot slot in playerSlots)
        {
            if (slot.gameObject.activeSelf)
                slot.SetWaiting();
        }

        var allies = BattleManager.Instance.allies;

        for (int i = 0; i < playerSlots.Length; i++)
        {
            if (i < allies.Count)
            {
                playerSlots[i].gameObject.SetActive(true);
                playerSlots[i].Setup(allies[i]);
            }
            else
            {
                playerSlots[i].gameObject.SetActive(false);
            }
        }

        var enemies = BattleManager.Instance.enemies;

        for (int i = 0; i < enemySlots.Length; i++)
        {
            if (i < enemies.Count)
            {
                enemySlots[i].gameObject.SetActive(true);
                enemySlots[i].Setup(enemies[i]);
            }
            else
            {
                enemySlots[i].gameObject.SetActive(false);
            }
        }
    }

    public void RefreshPlayerUI()
    {
        foreach (BattlePlayerSlot slot in playerSlots)
        {
            if (slot.gameObject.activeSelf)
                slot.Refresh();
        }
    }

    public void RefreshEnemyUI()
    {
        foreach (BattleEnemySlot slot in enemySlots)
        {
            if (slot.gameObject.activeSelf)
                slot.Refresh();
        }
    }
}