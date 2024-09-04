using System.Collections.Generic;

[System.Serializable]
public class EncounterOption
{
    public List<int> possibleId;
    public int Count { get {  return possibleId.Count; } }
}
