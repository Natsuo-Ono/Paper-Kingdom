using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Paper Kingdom/Skill")]
public class Skill : ScriptableObject
{
    [Header("Information")]
    public string skillName;

    [TextArea]
    public string description;

    [Header("Targeting")]
    public SkillType skillType;
    public TargetType targetType;

    [Header("Scaling")]
    public ScalingType scalingType;
    public float multiplier;
    public int ultimateCharge;

    [Header("Special Effects")]
    public SkillSpecialEffect specialEffect;

    public float reviveHPPercent;

    public float instantKillChance;
    public float bossInstantKillChance;

    [Header("Status Effects")]
    public List<StatusApplication> statusApplications = new();
}