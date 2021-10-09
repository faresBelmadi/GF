using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RunData 
{
    public int ClassID;
    public bool Ended;
    public PlayerData player;
}

[System.Serializable]
public class PlayerData
{
    public int HP;
    public int Volonté;
    public int Conscience;
    public int Essence;
    public int dmg;
    public int armor;
    public int Speed;
    
    public List<int> BoughtSpellID;
}

[System.Serializable]
public class GameData
{
    public List<RunData> previousRuns;
    public RunData CurrentRun;
}

