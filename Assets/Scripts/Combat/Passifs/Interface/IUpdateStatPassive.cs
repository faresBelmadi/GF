using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpdateStatPassive : IEffect
{
    void InitPassif(CharacterStat stat);
    void Clear();
    void UpdateStat ();
}
