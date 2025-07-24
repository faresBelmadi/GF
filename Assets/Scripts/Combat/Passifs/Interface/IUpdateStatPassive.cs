using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpdateStatPassive : IEffect
{
    void InitPassif(StatsHolder stat);
    void Clear();
    void UpdateStat ();
}
