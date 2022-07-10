using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New dialogue", menuName = "Remake/Character/Create New Joueur", order = 11)]
public class JoueurStatRemake : CharacterStatRemake
{
    public int Volonter;
    public int VolonterMax;
    public int Conscience;
    public int ConscienceMax;
    public int Clairvoyance;
    public int ClairvoyanceOriginal;
    public List<SpellRemake> ListSpell;

    public void ModifStateAll(JoueurStatRemake ModifState)
    {
        this.Volonter += ModifState.Volonter;
        this.VolonterMax += ModifState.VolonterMax;
        this.Conscience += ModifState.Conscience;
        this.ConscienceMax += ModifState.ConscienceMax;
        this.Clairvoyance += ModifState.Clairvoyance;
        base.ModifStateAll(ModifState);
    }

    public static JoueurStatRemake CreateFromCharacter(CharacterStatRemake ToCreate)
    {
        JoueurStatRemake ToReturn = ScriptableObject.CreateInstance("JoueurStatRemake") as JoueurStatRemake;
        ToReturn.ModifStateAll(ToCreate);
        return ToReturn;
    }
}
