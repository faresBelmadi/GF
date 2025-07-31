using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatPassive : AbstractPassive, IPassiveEffect, IEffectOnStat<StatsHandler<CharacterStat>>
{
    public void Apply(StatsHandler<CharacterStat> charStat)
    {
        throw new System.NotImplementedException();
    }
}
