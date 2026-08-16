using UnityEngine;

public class VesselCombatState : MonoBehaviour
{
    public bool IsAttacking { get; private set; }

    public bool CanMove => !IsAttacking;

    public void StartAction()
    {
        IsAttacking = true;
    }

    public void EndAction()
    {
        IsAttacking = false;
    }
}