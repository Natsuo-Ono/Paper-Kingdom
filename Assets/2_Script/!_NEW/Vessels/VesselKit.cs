using UnityEngine;

public abstract class VesselKit : MonoBehaviour
{
    // =========================================================
    // BASIC ATTACK
    // =========================================================

    public abstract int GetBasicAttackCount();

    public abstract float GetBasicAttackMultiplier(
        int attackIndex
    );

    public abstract void UseBasicAttack(
    int attackIndex,
    Transform target,
    VesselStats stats
    );


    // =========================================================
    // ATTACK SPEED
    // =========================================================

    public abstract float GetAttackSpeedMultiplier();


    // =========================================================
    // SKILL
    // =========================================================

    public abstract void UseSkill();


    // =========================================================
    // ULTIMATE
    // =========================================================

    public abstract void UseUltimate(
        VesselStats stats
    );
}