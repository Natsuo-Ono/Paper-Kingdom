using UnityEngine;

[System.Serializable]
public class ActiveStatus
{
    public StatusEffect status;

    public int remainingTurns;

    public Character source;

    public float value;

    public ActiveStatus(StatusEffect status, int turns, Character source, float value = 0)
    {
        this.status = status;
        this.remainingTurns = turns;
        this.source = source;
        this.value = value;
    }
}