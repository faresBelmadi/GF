using JetBrains.Annotations;
using System;
using System.Collections.Generic;

[System.Serializable]
public class RunData 
{
    public int ClassID;
    public bool Ended;
    public PlayerData player;
    public MapData map;
    //public Map currentmap;
}

[System.Serializable]
public class MapData
{
    public int usedSeed;
    public List<int> visitedRoomIds;
    public List<Tuple<int, int>> roomSelectedEncounter;
}

[System.Serializable]
public class PlayerData
{
    public int Radiance;
    public int RadianceMax;
    public int Volonter;
    public int Conscience;
    public int Essence;
    public int ForceAme;
    public int Vitesse;
    public int Clairvoyance;
    public int VolonterMax;
    public int ConscienceMax;
    public int Conviction;
    public int Resilience;
    public int Calme;
    public int SlotsSouvenir;

    public List<int> BoughtSpellID;
    public List<int> EquipedSouvenirID;
    public List<int> AppliedCurrentBuffDebuff;

}

[System.Serializable]
public class GameData
{
    public List<RunData> previousRuns;
    public RunData CurrentRun;
}

