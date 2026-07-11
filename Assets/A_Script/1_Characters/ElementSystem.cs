using UnityEngine;

public static class ElementSystem
{
    public const float Weak = 0.5f;
    public const float Normal = 1.0f;
    public const float Strong = 1.5f;

    public static float GetMultiplier(Element attacker, Element defender)
    {
        switch (attacker)
        {
            case Element.Pyro:

                switch (defender)
                {
                    case Element.Flora: return Strong;
                    case Element.Hydro: return Weak;
                    default: return Normal;
                }

            case Element.Hydro:

                switch (defender)
                {
                    case Element.Pyro: return Strong;
                    case Element.Electro: return Weak;
                    default: return Normal;
                }

            case Element.Electro:

                switch (defender)
                {
                    case Element.Hydro: return Strong;
                    case Element.Flora: return Weak;
                    default: return Normal;
                }

            case Element.Flora:

                switch (defender)
                {
                    case Element.Electro: return Strong;
                    case Element.Pyro: return Weak;
                    default: return Normal;
                }

            case Element.Lumen:

                switch (defender)
                {
                    case Element.Umbra: return Strong;
                    case Element.Lumen: return Weak;
                    default: return Normal;
                }

            case Element.Umbra:

                switch (defender)
                {
                    case Element.Lumen: return Strong;
                    case Element.Umbra: return Weak;
                    default: return Normal;
                }

            default:
                return Normal;
        }
    }

    public static bool IsCritical(float multiplier)
    {
        return multiplier > 1f;
    }

    public static bool IsWeak(float multiplier)
    {
        return multiplier < 1f;
    }
}