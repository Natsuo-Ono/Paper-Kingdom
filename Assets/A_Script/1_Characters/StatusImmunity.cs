public enum StatusImmunity
{
    // FOR SPECIAL TYPES OF ENEMIES
    Burn,           // Pyro
    Drown,          // Hydro
    Poison,         // Flora
    Quicksand,      // Geo

    Corrosion,      // Umbra Special Dmg
    Repentance,     // Lumen Special Dmg

    Blindness,      // Negative Status
    Vulnerable,     // Negative Status
    NegateHealing,  // Negative Status

    // FOR BOSSES
    Bleeding,       // True Damage // SPECIAL CASE

    Electrified,    // Turn Denial
    Frozen,         // Turn Denial
    Sleep,          // Turn Denial
}