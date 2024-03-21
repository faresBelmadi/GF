using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Stat Joueur", menuName = "Character/Create New Joueur", order = 11)]
public class JoueurStat : CharacterStat
{
    public int Lvl;
    public int Volonter;
    public int VolonterMax;
    public int Conscience;
    public int ConscienceMax;
    public int Clairvoyance;
    public int ClairvoyanceOriginal;
    public List<Spell> ListSpell;
    public int SlotsSouvenir;
    public List<Souvenir> ListSouvenir;
    

    public void ModifStateAll(JoueurStat ModifState)
    {
        this.Volonter += ModifState.Volonter;
        this.VolonterMax += ModifState.VolonterMax;
        this.Conscience += ModifState.Conscience;
        this.ConscienceMax += ModifState.ConscienceMax;
        this.Clairvoyance += ModifState.Clairvoyance;
        base.ModifStateAll(ModifState);
        RectificationStat();
    }

    public new void RectificationStat()
    {
        if (this.Volonter > this.VolonterMax)
        {
            this.Volonter = this.VolonterMax;
        }
        if (this.Conscience > this.ConscienceMax)
        {
            this.Conscience = this.ConscienceMax;
        }
        base.RectificationStat();
    }
    
    public new void setZero()
    {
        this.Volonter = 0;
        this.Conscience = 0;
        this.ConscienceMax = 0;
        this.Clairvoyance = 0;
        this.VolonterMax= 0;
        base.setZero();
    }

    public static JoueurStat CreateFromCharacter(CharacterStat ToCreate)
    {
        JoueurStat ToReturn = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
        ToReturn.setZero();
        ToReturn.ModifStateAll(ToCreate);
        return ToReturn;
    }
}
