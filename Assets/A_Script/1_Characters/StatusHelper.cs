public static class StatusHelper
{
    public static int GetDuration(StatusEffect effect)
    {
        switch (effect)
        {
            case StatusEffect.Shield:
                return 999;

            case StatusEffect.Burn:
            case StatusEffect.Drown:
            case StatusEffect.Poison:
            case StatusEffect.Quicksand:

            case StatusEffect.Bleeding:

            case StatusEffect.Corrosion:
            case StatusEffect.Repentance:

            case StatusEffect.Blindness:
            case StatusEffect.Vulnerable:
            case StatusEffect.NegateHealing:

            case StatusEffect.Regen:
            case StatusEffect.AtkBoost:
                return 3;

            case StatusEffect.Electrified:
            case StatusEffect.Frozen:
            case StatusEffect.Sleep:

            case StatusEffect.Taunt:
                return 1;

            default:
                return 0;
        }
    }

    public static bool IsNegative(StatusEffect effect)
    {
        switch (effect)
        {
            case StatusEffect.Burn:
            case StatusEffect.Drown:
            case StatusEffect.Poison:
            case StatusEffect.Quicksand:

            case StatusEffect.Bleeding:

            case StatusEffect.Corrosion:
            case StatusEffect.Repentance:

            case StatusEffect.Blindness:
            case StatusEffect.Vulnerable:
            case StatusEffect.NegateHealing:

            case StatusEffect.Electrified:
            case StatusEffect.Frozen:
            case StatusEffect.Sleep:
                return true;

            default:
                return false;
        }
    }
}