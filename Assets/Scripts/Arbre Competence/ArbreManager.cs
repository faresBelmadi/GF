using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.UI;

public class ArbreManager : MonoBehaviour
{
    public JoueurStat Stat;
    public ClassPlayer Class;

    public TextMeshProUGUI ValeurRadiance, ValeurFA, ValeurVitesse, ValeurConviction, ValeurResilience, ValeurCalme, ValeurVolonter, ValeurConscience, ValeurClairvoyance, ValeurEssence;
    public List<GameObject> PanelCompetence;

    public void StartArbre(JoueurStat _Stat)
    {
        Stat = _Stat;
        Class = GameManager.instance.classSO;
        InstantiateArbre();
        TextStat();
    }

    public void TextStat()
    {
        ValeurRadiance.text = "Radiance : " + Stat.Radiance.ToString() + "/" + Stat.RadianceMax;
        ValeurFA.text = "Force d'ame : " + Stat.ForceAme.ToString();
        ValeurVitesse.text = "Vitesse : " + Stat.Vitesse.ToString();
        ValeurConviction.text = "Conviction : " + Stat.Conviction.ToString();
        ValeurResilience.text = "Resilience : " + Stat.Resilience.ToString();
        ValeurCalme.text = "Calme : " + Stat.Calme.ToString();
        ValeurVolonter.text = "Volonter : " + Stat.Volonter.ToString() + "/" + Stat.VolonterMax.ToString();
        ValeurConscience.text = "Conscience : " + Stat.Conscience.ToString() + "/" + Stat.ConscienceMax.ToString();
        ValeurClairvoyance.text = "Clairvoyance : " + Stat.Clairvoyance.ToString();
        ValeurEssence.text = "Essence : " + Stat.Essence.ToString();
    }

    public void InstantiateArbre()
    {
        for(int i=1; i < PanelCompetence.Count; i++)
        {
            PanelCompetence[i].GetComponent<ContainerCompetence>().LaCompetence = Class.Competences.First(c => c.IDLvl == i);
            PanelCompetence[i].GetComponent<ContainerCompetence>().Affichage();
            ContainerCompetence temp = PanelCompetence[i].GetComponent<ContainerCompetence>();
            PanelCompetence[i].GetComponent<ContainerCompetence>().ButtonBuy.onClick.AddListener(() => { Buy(temp); });
        }

        UpdateCout();
    }

    public void UpdateCout()
    {
        foreach(var item in PanelCompetence)
        {
            item.GetComponent<ContainerCompetence>().LaCompetence.Essence = item.GetComponent<ContainerCompetence>().LaCompetence.EssenceOriginal;
            for (int i = 0; i < item.GetComponent<ContainerCompetence>().LaCompetence.IDLier.Count; i++)
            {
                if(Class.Competences.First(c => c.IDLvl == item.GetComponent<ContainerCompetence>().LaCompetence.IDLier[i]).Bought == true && Class.Competences.First(c => c.IDLvl == item.GetComponent<ContainerCompetence>().LaCompetence.IDLier[i]).IDLvl != 0)
                {
                    item.GetComponent<ContainerCompetence>().LaCompetence.Essence -= 200;
                }
            }
            if(item.GetComponent<ContainerCompetence>().LaCompetence.IDLvl != 0)
            {
                item.GetComponent<ContainerCompetence>().Affichage();
            }
        }
    }

    public void EndArbre()
    {

    }

    public void Buy(ContainerCompetence Competence)
    {
        Competence.LaCompetence.Bought = true;
        Competence.AffichageAchat.GetComponent<Image>().sprite = Competence.Acheter;
        Competence.ButtonBuy.gameObject.SetActive(false);
        Competence.ButtonEquip.gameObject.SetActive(true);
        ModifStat(Competence.LaCompetence);
        UpdateCout();
        TextStat();
    }

    public void ModifStat(Competence LaCompetence)
    {
        foreach (var item in LaCompetence.ModifStat)
        {
            switch (item.StatModif)
            {
                case StatModif.RadianceMax:
                    Stat.RadianceMax -= item.Valeur;
                    break;
                case StatModif.ForceAme:
                    Stat.ForceAme -= item.Valeur;
                    break;
                case StatModif.Calme:
                    Stat.Calme -= item.Valeur;
                    break;
                case StatModif.Clairvoyance:
                    Stat.Clairvoyance -= item.Valeur;
                    break;
                case StatModif.ConscienceMax:
                    Stat.ConscienceMax -= item.Valeur;
                    break;
                case StatModif.Conviction:
                    Stat.Conviction -= item.Valeur;
                    break;
                case StatModif.Resilience:
                    Stat.Resilience -= item.Valeur;
                    break;
                case StatModif.Vitesse:
                    Stat.Vitesse -= item.Valeur;
                    break;
                case StatModif.VolonterMax:
                    Stat.VolonterMax -= item.Valeur;
                    break;
            }
        }
    }
}
