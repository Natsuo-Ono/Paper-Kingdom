using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Passive", menuName = "Paper Kingdom/Passive")]
public class Passive : ScriptableObject
{
    [Header("Information")]
    public PassiveID passiveID;

    public string passiveName;

    [TextArea]
    public string description;

    [Header("Effect")]
    public PassiveEffect effect;

    public List<StatusEffect> requiredStatuses;

    [Header("Condition")]
    public PassiveCondition condition;

    [Header("Trigger")]
    public PassiveTrigger trigger;

    [Header("Values")]
    public float value;
    public float chance;
}