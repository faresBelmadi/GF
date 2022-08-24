using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContainerCompetence : MonoBehaviour
{
    public Competence LaCompetence;
    public Sprite Acheter;
    public TextMeshProUGUI NomSpell, Stat, Cout;
    public Button ButtonBuy, ButtonEquip, ButtonUnEquip;
    public GameObject ZoneDescription;

    public void Affichage()
    {
        NomSpell.text = "Spell :\n" + LaCompetence.Spell.Nom.ToString();
        foreach(var item in LaCompetence.ModifStat)
        {
            Stat.text += DescriptionStat(item.StatModif) + item.Valeur + "\n";
        }
        Cout.text = "Cout : " + LaCompetence.Essence.ToString();
    }

    private string DescriptionStat(StatModif Stat)
    {
        string DescTemp = "";
        switch (Stat)
        {
            case StatModif.RadianceMax:
                DescTemp = "Radiance Max : ";
                break;
            case StatModif.ForceAme:
                DescTemp = "Force d'Ame : ";
                break;
            case StatModif.Vitesse:
                DescTemp = "Vitesse : ";
                break;
            case StatModif.Conviction:
                DescTemp = "Conviction : ";
                break;
            case StatModif.Resilience:
                DescTemp = "Resilience : ";
                break;
            case StatModif.Calme:
                DescTemp = "Calme : ";
                break;
            case StatModif.VolonterMax:
                DescTemp = "Volonter Max : ";
                break;
            case StatModif.ConscienceMax:
                DescTemp = "Conscience Max : ";
                break;
            case StatModif.Clairvoyance:
                DescTemp = "Clairvoyance : ";
                break;
        }
        return DescTemp;
    }

    public void OnMouseEnter()
    {
        
    }
}
