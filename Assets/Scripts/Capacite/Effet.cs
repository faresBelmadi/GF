using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New dialogue", menuName = "Remake/Capacité/Create New Effet", order = 11)]
public class Effet : ScriptableObject
{
    public TypeEffet TypeEffet;
    public Cible Cible;
    public int Pourcentage;
    public int ValeurBrut;
    public int NbAttaque;

    public JoueurStat ResultEffet(JoueurStat Caster)
    {
        JoueurStat ModifState = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
        switch (this.TypeEffet)
        {

            default:
                ModifState = ResultEffetCommun(Caster);
                break;
        }
        return ModifState;
    }

    public JoueurStat ResultEffet(EnnemiStat Caster)
    {
        JoueurStat ModifState = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
        ModifState = ResultEffetCommun(Caster);
        return ModifState;
    }

    private JoueurStat ResultEffetCommun(CharacterStat Caster)
    {
        JoueurStat ModifState = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
        ModifState = JoueurStat.CreateFromCharacter(ResultEffetBase(Caster));
        switch (this.TypeEffet)
        {
            case TypeEffet.Clairvoyance:
                if (NbAttaque >= 1)
                {
                    ModifState.Clairvoyance = ValeurBrut * NbAttaque;
                }
                else
                {
                    ModifState.Clairvoyance = ValeurBrut;
                }
                break;
            default:
                break;
        }
        return ModifState;
    }

    private CharacterStat ResultEffetBase(CharacterStat Caster)
    {
        CharacterStat ModifState = ScriptableObject.CreateInstance("CharacterStat") as CharacterStat;
        switch (this.TypeEffet)
        {
            case TypeEffet.DegatsForceAme:
                ModifState.Radiance -= (Mathf.FloorToInt(Pourcentage / 100f) * NbAttaque) * Caster.ForceAme;
                break;
            case TypeEffet.DegatsBrut:
                ModifState.Radiance -= ValeurBrut * NbAttaque;
                break;
            default:
                break;
        }
        return ModifState;
    }
}
