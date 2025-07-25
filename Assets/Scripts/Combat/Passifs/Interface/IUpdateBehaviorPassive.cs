using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpdateBehaviorPassive : IEffect
{
    void InitPassif(CombatBehavior<StatsHandler> behavior);
    void Clear();
    void UpdateStat();
}

public interface IUpdateEnnemyBehaviorPassive : IEffect
{
    void InitPassif(EnnemyBehavior behavior);
    void Clear();
    void UpdateStat();
}