using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPassive : ScriptableObject
{
    [field: Header("Nom")]
    [field: SerializeField] 
    public string DefaultNom {  get; private set; }
    [field: SerializeField]
    public string IdTradName { get; private set; }
    [field: Header("Description")]
    [field: SerializeField]
    public string DefaultDescription { get; private set; } = "Missing";
    [field: SerializeField]
    public virtual string IdTradDesc { get; private set; }
}
