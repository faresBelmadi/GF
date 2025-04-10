using System;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Stat Ennemi", menuName = "Character/Create New Ennemi", order = 11)]
public class EnnemiStat : CharacterStat
{
    public event Action OnCustomStatModification;

    [Space]
    public Sprite Icon;
    public string IdTradName;
    public string Nom;
    public int Dissimulation;
    public int DissimulationOriginal;
    public bool NoTension = false;
    public GameObject Spawnable;
    public EnnemiSpell Att1;
    public EnnemiSpell Att2;
    public EnnemiSpell Buff;
    public EnnemiSpell Debuff;
    //Uniquement pour Jeanne
    private int _divin;
    public int Divin
    {
        get => _divin;
        set
        {
            if (value != _divin)
            {
                _divin = value;
                OnCustomStatModification?.Invoke();
            }
        }
    }
}
