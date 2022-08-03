using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuStatManager : MonoBehaviour
{
    public JoueurStat Stat, StatTemp;

    public GameObject SouvenirPrefab;
    public GameObject SouvenirSpawn;
    public List<GameObject> Souvenir;

    public TextMeshProUGUI ValeurRadiance, ValeurFA, ValeurVitesse, ValeurConviction, ValeurResilience, ValeurCalme, ValeurVolonter, ValeurConscience, ValeurClairvoyance;
    public TextMeshProUGUI ModifRadiance, ModifFA, ModifVitesse, ModifConviction, ModifResilience, ModifCalme, ModifVolonter, ModifConscience, ModifClairvoyance;

    public void StartMenuStat()
    {
        Debug.Log("hello");
        Stat = GameManager.instance.playerStat;
        StatTemp = Instantiate(Stat);
        foreach (var item in Stat.ListSouvenir)
        {
            var temp = Instantiate(SouvenirPrefab, SouvenirSpawn.transform);
            temp.GetComponent<SouvenirUI>().LeSouvenir = item;
            //temp.GetComponent<SpellCombat>().Act = DoAction;
            temp.GetComponent<SouvenirUI>().StartUp();

            Souvenir.Add(temp);
        }
        UpdateStatUI();
    }

    void Update()
    {
        UpdateStatUI();
    }

    public void UpdateStatUI()
    {
        ValeurRadiance.text = StatTemp.Radiance.ToString() + "/" + StatTemp.RadianceMax;
        ValeurFA.text = StatTemp.ForceAme.ToString();
        ValeurVitesse.text = StatTemp.Vitesse.ToString();
        ValeurConviction.text = StatTemp.Conviction.ToString();
        ValeurResilience.text = StatTemp.Resilience.ToString();
        ValeurCalme.text = StatTemp.Calme.ToString();
        ValeurVolonter.text = StatTemp.VolonterMax.ToString();
        ValeurConscience.text = StatTemp.ConscienceMax.ToString();
        ValeurClairvoyance.text = StatTemp.Clairvoyance.ToString();

        ModifTempsReel(Stat.Radiance, StatTemp.Radiance, ModifRadiance);
        ModifTempsReel(Stat.ForceAme, StatTemp.ForceAme, ModifFA);
        ModifTempsReel(Stat.Vitesse, StatTemp.Vitesse, ModifVitesse);
        ModifTempsReel(Stat.Conviction, StatTemp.Conviction, ModifConviction);
        ModifTempsReel(Stat.Resilience, StatTemp.Resilience, ModifResilience);
        ModifTempsReel(Stat.Calme, StatTemp.Calme, ModifCalme);
        ModifTempsReel(Stat.VolonterMax, StatTemp.VolonterMax, ModifVolonter);
        ModifTempsReel(Stat.ConscienceMax, StatTemp.ConscienceMax, ModifConscience);
        ModifTempsReel(Stat.Clairvoyance, StatTemp.Clairvoyance, ModifClairvoyance);
    }

    public void ModifTempsReel(int original, int nouveau, TextMeshProUGUI Text)
    {
        if (nouveau < original)
        {
            Text.color = Color.red;
            Text.text = "(";
        }
        else if (nouveau > original)
        {
            Text.color = Color.green;
            Text.text = "(+";
        }
        else if (nouveau == original)
        {
            Text.color = Color.grey;
            Text.text = "(";
        }
        Text.text += (nouveau - original).ToString() + ")";
    }

    public void End()
    {
        StartCoroutine(GameManager.instance.pmm.EndMenuStat());
    }
}
