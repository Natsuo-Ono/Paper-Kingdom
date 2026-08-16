using UnityEngine;

[CreateAssetMenu(
    fileName = "VesselData",
    menuName = "Paper Kingdom/Vessel Data"
)]
public class VesselData : ScriptableObject
{
    [Header("Identity")]
    public string characterName;

    public int rarity;

    public VesselClass vesselClass;
    public VesselSubClass subClass;

    [Header("Combat")]
    public ElementType element;
    public DamageType damageType;

    public float attackRange;

    [Header("Base Stats")]
    public float baseHP;
    public float baseATK;
    public float baseDEF;

    [Header("Stat Growth")]
    public float hpGrowth;
    public float atkGrowth;
    public float defGrowth;
}