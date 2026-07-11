using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Formation", menuName = "Paper Kingdom/Enemy Formation")]
public class EnemyFormation : ScriptableObject
{
    [Header("Information")]
    public string formationName;

    [Header("Battle Positions")]
    public EnemySlot position1;
    public EnemySlot position2;
    public EnemySlot position3;
    public EnemySlot position4;
}