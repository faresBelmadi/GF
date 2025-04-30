using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class WarriorBehavior : JoueurBehavior
{

    protected override void ResolvePassif()
    {
        base.ResolvePassif();
    }
    protected override void UpdateStat()
    {
        var resilienceBonus = (Stat.Conscience / 1) * 1;
        Stat.ResiliencePassif = Mathf.FloorToInt(resilienceBonus);
        Stat.ResiliencePassif = Stat.Conscience;
        
    }
}
