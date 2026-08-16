using UnityEngine;

public class VesselStats : MonoBehaviour
{
    [Header("Character Data")]
    public VesselData vesselData;

    [Header("Level")]
    public int level = 1;

    [Header("Runtime Stats")]
    public float MaxHP { get; private set; }
    public float CurrentHP { get; private set; }

    public float ATK { get; private set; }
    public float DEF { get; private set; }

    private void Awake()
    {
        CalculateStats();

        CurrentHP = MaxHP;
    }

    public void CalculateStats()
    {
        if (vesselData == null)
        {
            Debug.LogError(
                gameObject.name +
                " has no VesselData assigned!"
            );

            return;
        }

        MaxHP = CalculateStat(
            vesselData.baseHP,
            vesselData.hpGrowth
        );

        ATK = CalculateStat(
            vesselData.baseATK,
            vesselData.atkGrowth
        );

        DEF = CalculateStat(
            vesselData.baseDEF,
            vesselData.defGrowth
        );
    }

    private float CalculateStat(float baseStat, float growth)
    {
        return baseStat + (growth * (level - 1));
    }
}