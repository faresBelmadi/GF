using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStartTurnPassive<T> : IEffect
{
    void ApplyOnTurnStart(T stat);
}
