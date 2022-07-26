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

    public JoueurStat ResultEffet(JoueurStat Caster, int LastDamageTake = 0)
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

    public JoueurStat ResultEffet(EnnemiStat Caster, int LastDamageTaken = 0)
    {
        JoueurStat ModifState = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
        ModifState = ResultEffetCommun(Caster, LastDamageTaken);
        return ModifState;
    }

    private JoueurStat ResultEffetCommun(CharacterStat Caster, int LastDamageTaken = 0)
    {
        JoueurStat ModifState = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
        ModifState = JoueurStat.CreateFromCharacter(ResultEffetBase(Caster, LastDamageTaken));
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

    private CharacterStat ResultEffetBase(CharacterStat Caster, int LastDamageTaken = 0)
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
            case TypeEffet.Conviction:
                ModifState.Conviction += ValeurBrut * NbAttaque;
                break;
            case TypeEffet.AugmentationPourcentageFA:
                ModifState.ForceAme += (Mathf.FloorToInt(Pourcentage / 100f) * NbAttaque) * Caster.ForceAme;
                break;
            case TypeEffet.RadianceMax:
                ModifState.RadianceMax += ValeurBrut * NbAttaque;
                break;
            case TypeEffet.AugmentationDegat:
                ModifState.Radiance -= (Mathf.FloorToInt(Pourcentage / 100f) * NbAttaque) * LastDamageTaken;
                    break;
            default:
                break;
        }
        return ModifState;
    }
}
