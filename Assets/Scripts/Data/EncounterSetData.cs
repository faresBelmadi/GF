using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = " new EncounterSetData", menuName = "Encounter/Create New EncounterSetData")]
public class EncounterSetData : ScriptableObject
{ 
    [field: SerializeField] public List<EncounterAlea> EncounterNeutralAleaList { get; private set; }
    [field: SerializeField] public List<EncounterAlea> EncounterClassAleaList { get; private set; }
    [field: SerializeField] public List<Encounter> EncounterNeutralList { get; private set; }
    [field: SerializeField] public List<Encounter> EncounterClassList { get; private set; }
    [field: SerializeField] public List<Encounter> EncounterEliteList { get; private set; }
    [field: SerializeField] public List<Encounter> EncounterClassEliteList { get; private set; }
    [field: SerializeField] public List<Encounter> EncounterBossList { get; private set; }
}