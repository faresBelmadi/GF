using System.Collections.Generic;
using UnityEngine;

public enum ConditionalBuff
{
    NONE,
    RoomAutel,
    RoomAlea,
    GainConscience,
    PerteConscience
}

[CreateAssetMenu(fileName = "New Buff/debuff", menuName = "Capacité/Create New BuffDebuff", order = 11)]
public class BuffDebuff : ScriptableObject
{
    public string Nom;
    public string idTradName;
    public string Description;
    public string idTradDescription;
    public List<Effet> Effet;
    public bool IsConsomable;
    public int TimingConsomationMinimum = 1;
    public List<BuffDebuff> Consomation;
    public bool IsDebuff;
    public Cible CibleApplication;
    public int IDCombatOrigine;
    public Decompte Decompte;
    public TimerApplication timerApplication;
    public int Temps;
    public Sprite Icon;
    public GameObject SpawnObject;
    public bool DirectApplication = true;
 
    [SerializeField]
    private ConditionalBuff _condition = ConditionalBuff.NONE;

    public ConditionalBuff ConditionnalBuff { get => _condition; }

    public int TimeLeft
    {
        get     //TODO: A améliorer
        {
            if (Decompte == Decompte.phase) return Temps;
            if (GameManager.Instance.BattleMan.EnemyScripts.Count == 0) return Temps;
            if (Decompte == Decompte.tour) return (Temps % GameManager.Instance.BattleMan.EnemyScripts.Count) + 1;
            else return -1;
        }

    }
}
