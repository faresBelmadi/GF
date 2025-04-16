using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPassive : ScriptableObject
{
    [field: SerializeField] public string Nom {  get; private set; }
}
