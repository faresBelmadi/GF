using System.Collections.Generic;

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
    public int Radiance;
    public int Volonter;
    public int Conscience;
    public int Essence;
    public int ForceAme;
    public int Vitesse;
    public int Clairvoyance;
    
    public List<int> BoughtSpellID;
}

[System.Serializable]
public class GameData
{
    public List<RunData> previousRuns;
    public RunData CurrentRun;
}

