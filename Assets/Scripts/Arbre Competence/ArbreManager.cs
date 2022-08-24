using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

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
        ValeurRadiance.text = Stat.Radiance.ToString() + "/" + Stat.RadianceMax;
        ValeurFA.text = Stat.ForceAme.ToString();
        ValeurVitesse.text = Stat.Vitesse.ToString();
        ValeurConviction.text = Stat.Conviction.ToString();
        ValeurResilience.text = Stat.Resilience.ToString();
        ValeurCalme.text = Stat.Calme.ToString();
        ValeurVolonter.text = Stat.Volonter.ToString() + "/" + Stat.VolonterMax.ToString();
        ValeurConscience.text = Stat.Conscience.ToString() + "/" + Stat.ConscienceMax.ToString();
        ValeurClairvoyance.text = Stat.Clairvoyance.ToString();
        ValeurEssence.text = Stat.Essence.ToString();
    }

    public void InstantiateArbre()
    {
        for(int i=1; i < PanelCompetence.Count; i++)
        {
            PanelCompetence[i].GetComponent<ContainerCompetence>().LaCompetence = Class.Competences.First(c => c.IDLvl == i);
            PanelCompetence[i].GetComponent<ContainerCompetence>().Affichage();
        }

        UpdateCout();
    }

    public void UpdateCout()
    {
        foreach(var item in PanelCompetence)
        {
            for (int i = 0; i < item.GetComponent<ContainerCompetence>().LaCompetence.IDLier.Count; i++)
            {
                if(Class.Competences.First(c => c.IDLvl == item.GetComponent<ContainerCompetence>().LaCompetence.IDLier[i]).Bought == true && Class.Competences.First(c => c.IDLvl == item.GetComponent<ContainerCompetence>().LaCompetence.IDLier[i]).IDLvl != 0)
                {
                    item.GetComponent<ContainerCompetence>().LaCompetence.Essence -= 200;
                }
            }
        }
    }

    public void EndArbre()
    {

    }
}
