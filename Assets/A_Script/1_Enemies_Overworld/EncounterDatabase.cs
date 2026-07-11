using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Encounter Database", menuName = "Paper Kingdom/Encounter Database")]
public class EncounterDatabase : ScriptableObject
{
    public EncounterArea area;

    public List<EnemyFormation> formations = new();
}