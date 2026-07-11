using UnityEngine;
using System.Collections;

public class EncounterManager : MonoBehaviour
{
    public static EncounterManager Instance;

    public bool canEncounter = true;

    public EncounterDatabase[] encounterDatabases;

    private EncounterTrigger currentTrigger;

    void Awake()
    {
        Instance = this;
    }

    public void EnterTrigger(EncounterTrigger trigger)
    {
        currentTrigger = trigger;
    }

    public void ExitTrigger()
    {
        currentTrigger = null;
    }

    EncounterDatabase GetDatabase(EncounterArea area)
    {
        foreach (EncounterDatabase database in encounterDatabases)
        {
            if (database.area == area)
                return database;
        }

        return null;
    }

    public void TryEncounter()
    {
        if (!canEncounter)
            return;

        if (currentTrigger == null)
            return;

        float roll = Random.Range(0f, 100f);

        if (roll <= currentTrigger.encounterChance)
        {
            StartEncounter();
        }
    }

    void StartEncounter()
    {
        canEncounter = false;

        Debug.Log("Battle Started!");

        EncounterDatabase database = GetDatabase(currentTrigger.area);

        if (database == null)
        {
            Debug.LogError("No Encounter Database found for " + currentTrigger.area);
            return;
        }

        if (database.formations.Count == 0)
        {
            Debug.LogError("No formations in " + database.area);
            return;
        }

        EnemyFormation formation =
            database.formations[Random.Range(0, database.formations.Count)];

        BattleManager.Instance.StartBattle(formation);
    }

    public void BattleFinished()
    {
        StartCoroutine(EncounterCooldown());
    }

    IEnumerator EncounterCooldown()
    {
        yield return new WaitForSeconds(Random.Range(5f, 15f));

        canEncounter = true;
    }
}