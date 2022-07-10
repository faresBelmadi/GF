using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New dialogue", menuName = "Remake/Capacité/Create New Effet", order = 11)]
public class EffetRemake : ScriptableObject
{
    public TypeEffetRemake TypeEffet;
    public CibleRemake Cible;
    public int Pourcentage;
    public int ValeurBrut;
    public int NbAttaque;

    public JoueurStatRemake ResultEffet(JoueurStatRemake Caster)
    {
        JoueurStatRemake ModifState = ScriptableObject.CreateInstance("JoueurStatRemake") as JoueurStatRemake;
        switch (this.TypeEffet)
        {

            default:
                ModifState = ResultEffetCommun(Caster);
                break;
        }
        return ModifState;
    }

    public JoueurStatRemake ResultEffet(EnnemiStatRemake Caster)
    {
        JoueurStatRemake ModifState = ScriptableObject.CreateInstance("JoueurStatRemake") as JoueurStatRemake;
        ModifState = ResultEffetCommun(Caster);
        return ModifState;
    }

    private JoueurStatRemake ResultEffetCommun(CharacterStatRemake Caster)
    {
        JoueurStatRemake ModifState = ScriptableObject.CreateInstance("JoueurStatRemake") as JoueurStatRemake;
        ModifState = JoueurStatRemake.CreateFromCharacter(ResultEffetBase(Caster));
        switch (this.TypeEffet)
        {
            case TypeEffetRemake.Clairvoyance:
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

    private CharacterStatRemake ResultEffetBase(CharacterStatRemake Caster)
    {
        CharacterStatRemake ModifState = ScriptableObject.CreateInstance("CharacterStatRemake") as CharacterStatRemake;
        switch (this.TypeEffet)
        {
            case TypeEffetRemake.DegatsForceAme:
                ModifState.Radiance -= (Mathf.FloorToInt(Pourcentage / 100f) * NbAttaque) * Caster.ForceAme;
                break;
            case TypeEffetRemake.DegatsBrut:
                ModifState.Radiance -= ValeurBrut * NbAttaque;
                break;
            default:
                break;
        }
        return ModifState;
    }
}
