using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnDamagePassive : IEffect
{
    void ApplyEffectOnDommage(AbstractStatsHandler stat);
}
