using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = " new FloorData", menuName = "Floor/Create New FloorData")]
public class FloorData : ScriptableObject
{
    [field: SerializeField, Tooltip("The encounter of the floor")] public EncounterSetData EncounterSet { get; private set; }
    [field: SerializeField, Tooltip("The list of the room in this floor (except boss, start & loot")] public List<TypeRoom> RoomPool { get; private set; }
}
