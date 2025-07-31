using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILootEssencePassive : IEffect
{
   int Value { get; }
    void ApplyLootEffect(PlayerStatsHandler stat);
}
