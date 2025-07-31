using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatPassive : AbstractPassive, IPassiveEffect, IEffectOnStat<StatsHandler>
{
    public void Apply(StatsHandler charStat)
    {
        throw new System.NotImplementedException();
    }
}
