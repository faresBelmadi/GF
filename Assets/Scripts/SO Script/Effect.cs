using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New effect", menuName = "Create New Effect", order = 1)]
public class Effect : ScriptableObject
{
    public BuffType type;
    public int nbAttaques;
    public Cible target;
    public int pourcentageEffet;

    public void DoAction(int strength, out int hpToRemove, out int nbAttaque)
    {
        SwitchType(out hpToRemove,strength, out nbAttaque);

    }    
    public void SwitchType(out int hpToRemove, int strength, out int nbAttaque)
    {
        nbAttaque = 0;
        switch(type)
        {
            case BuffType.DégatsForceAme:
            hpToRemove = Mathf.FloorToInt((pourcentageEffet/100f) * strength);
            nbAttaque = nbAttaques;
            break;
            case BuffType.DégatsBrut:
            hpToRemove = pourcentageEffet;
            nbAttaque = nbAttaques;
            break;
            case BuffType.Att:
            case BuffType.AttBrut:
            case BuffType.Def:
            case BuffType.DefBrut:
            hpToRemove = 0;
            break;
            case BuffType.Cible:
            case BuffType.Armure:
            case BuffType.Colère:
            case BuffType.Doute:
            case BuffType.Découragé:
            case BuffType.Execution:
            case BuffType.Peur:
            case BuffType.PourEnFinir:
            case BuffType.Vulnérable:
            hpToRemove = 0;
            break;

            case BuffType.Soin:
            hpToRemove = -pourcentageEffet;
            break;

            default:
            hpToRemove = 0;
            break;
        }
    }
}

