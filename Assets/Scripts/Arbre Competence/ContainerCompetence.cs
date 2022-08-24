using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContainerCompetence : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Competence LaCompetence;
    public Sprite Acheter;
    public TextMeshProUGUI NomSpell, Stat, Cout;
    public Button ButtonBuy, ButtonEquip, ButtonUnEquip;
    public GameObject ZoneDescription, AffichageAchat;

    public void Affichage()
    {
        NomSpell.text = "Spell :\n" + LaCompetence.Spell.Nom.ToString();
        Stat.text = "";
        foreach(var item in LaCompetence.ModifStat)
        {
            Stat.text += DescriptionStat(item.StatModif) + item.Valeur + "\n";
        }
        if(LaCompetence.Bought == true)
        {
            Cout.text = "";
        }
        else
        {
            Cout.text = "Cout : " + LaCompetence.Essence.ToString();
        }
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

    #region Description

    public void OnPointerEnter(PointerEventData eventData)
    {
        ZoneDescription.SetActive(true);
        ZoneDescription.GetComponentInChildren<TextMeshProUGUI>().text = LaCompetence.Spell.Nom + "\nCost : ";
        foreach (var item in LaCompetence.Spell.Costs)
        {
            if (item.typeCost == TypeCostSpell.volonte)
                ZoneDescription.GetComponentInChildren<TextMeshProUGUI>().text += item.Value + "V ";
            if (item.typeCost == TypeCostSpell.radiance)
                ZoneDescription.GetComponentInChildren<TextMeshProUGUI>().text += item.Value + "R ";
            if (item.typeCost == TypeCostSpell.conscience)
                ZoneDescription.GetComponentInChildren<TextMeshProUGUI>().text += item.Value + "C ";
        }
        ZoneDescription.GetComponentInChildren<TextMeshProUGUI>().text += "\n" + LaCompetence.Spell.Description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ZoneDescription.SetActive(false);
    }

    #endregion Description


}
