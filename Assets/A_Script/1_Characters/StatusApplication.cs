using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatusApplication
{
    public StatusEffect statusEffect;

    [Range(0, 100)]
    public int chance;
}