using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI.Extensions;
using static UnityEngine.UI.Extensions.ReorderableList;
using UnityEngine.UI;

public class MenuStatManager : MonoBehaviour
{
    public JoueurStat Stat, StatTemp;

    public GameObject SouvenirPrefab;
    public GameObject SouvenirSpawnEquiped;
    public GameObject SouvenirSpawnUnEquiped;
    public List<GameObject> Souvenir;
    public List<Souvenir> EquipedSouvenir;

    public TextMeshProUGUI ValeurRadiance, ValeurFA, ValeurVitesse, ValeurConviction, ValeurResilience, ValeurCalme, ValeurVolonter, ValeurConscience, ValeurClairvoyance;
    public TextMeshProUGUI ModifRadiance, ModifFA, ModifVitesse, ModifConviction, ModifResilience, ModifCalme, ModifVolonter, ModifConscience, ModifClairvoyance;

    public int NbSlotsEquiped;
    public TextMeshProUGUI NbSlots;

    public GameObject ArbreCompetencePrefab;
    public GameObject ArbreCompetence, Canvas, Menu;
 
    #region Start

    public void StartMenuStat()
    {
        Stat = GameManager.instance.playerStat;
        StatTemp = Instantiate(Stat);
        foreach (var item in Stat.ListSouvenir)
        {
            GameObject temp;
            if (item.Equiped == true)
            {
                ResetStatEnter(item);
                NbSlotsEquiped += item.Slots;
                temp = Instantiate(SouvenirPrefab, SouvenirSpawnEquiped.transform);
            }
            else
            {
                temp = Instantiate(SouvenirPrefab, SouvenirSpawnUnEquiped.transform);
            }
            temp.GetComponent<SouvenirUI>().LeSouvenir = item;
            temp.GetComponent<SouvenirUI>().StartUp();
            if (item.Equiped == true)
            {
                EquipedSouvenir.Add(item);
            }
            Souvenir.Add(temp);
        }
        UpdateStatUI();
    }

    public void ResetStatEnter(Souvenir LeSouvenir)
    {
        foreach (var item in LeSouvenir.ModificationStat)
        {
            switch (item.StatModif)
            {
                case StatModif.RadianceMax:
                    Stat.RadianceMax -= item.ValeurModifier;
                    break;
                case StatModif.ForceAme:
                    Stat.ForceAme -= item.ValeurModifier;
                    break;
                case StatModif.Calme:

                    break;
                case StatModif.Clairvoyance:

                    break;
                case StatModif.ConscienceMax:

                    break;
                case StatModif.Conviction:

                    break;
                case StatModif.Resilience:

                    break;
                case StatModif.Vitesse:

                    break;
                case StatModif.VolonterMax:

                    break;
            }
        }
    }

    #endregion Start

    #region Update

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
        ValeurVolonter.text = StatTemp.Volonter.ToString() + "/" + StatTemp.VolonterMax.ToString();
        ValeurConscience.text = StatTemp.Conscience.ToString() + "/" + StatTemp.ConscienceMax.ToString();
        ValeurClairvoyance.text = StatTemp.Clairvoyance.ToString();

        ModifTempsReel(Stat.RadianceMax, StatTemp.RadianceMax, ModifRadiance);
        ModifTempsReel(Stat.ForceAme, StatTemp.ForceAme, ModifFA);
        ModifTempsReel(Stat.Vitesse, StatTemp.Vitesse, ModifVitesse);
        ModifTempsReel(Stat.Conviction, StatTemp.Conviction, ModifConviction);
        ModifTempsReel(Stat.Resilience, StatTemp.Resilience, ModifResilience);
        ModifTempsReel(Stat.Calme, StatTemp.Calme, ModifCalme);
        ModifTempsReel(Stat.VolonterMax, StatTemp.VolonterMax, ModifVolonter);
        ModifTempsReel(Stat.ConscienceMax, StatTemp.ConscienceMax, ModifConscience);
        ModifTempsReel(Stat.Clairvoyance, StatTemp.Clairvoyance, ModifClairvoyance);

        NbSlots.text = NbSlotsEquiped + "/" + StatTemp.SlotsSouvenir;
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

    #endregion Update

    #region Application Souvenir

    public void ModifStat(Souvenir LeSouvenir, bool Equiped)
    {
        foreach (var item in LeSouvenir.ModificationStat)
        {
            switch (item.StatModif)
            {
                case StatModif.RadianceMax:
                    if (Equiped == true)
                    {
                        var Temp = Mathf.FloorToInt((item.Pourcentage / 100f) * Stat.RadianceMax);
                        StatTemp.RadianceMax += Temp;
                        item.ValeurModifier = Temp;
                    }
                    else
                    {
                        StatTemp.RadianceMax -= item.ValeurModifier;
                    }
                    break;
                case StatModif.ForceAme:
                    if (Equiped == true)
                    {
                        var Temp = Mathf.FloorToInt((item.Pourcentage / 100f) * Stat.ForceAme);
                        StatTemp.ForceAme += Temp;
                        item.ValeurModifier = Temp;
                    }
                    else
                    {
                        StatTemp.ForceAme -= item.ValeurModifier;
                    }
                    break;
                case StatModif.Calme:
                    StatTemp.Calme += (Mathf.FloorToInt(item.Pourcentage / 100f)) * StatTemp.Calme;
                    break;
                case StatModif.Clairvoyance:
                    StatTemp.Clairvoyance += (Mathf.FloorToInt(item.Pourcentage / 100f)) * StatTemp.Clairvoyance;
                    break;
                case StatModif.ConscienceMax:
                    StatTemp.ConscienceMax += (Mathf.FloorToInt(item.Pourcentage / 100f)) * StatTemp.ConscienceMax;
                    break;
                case StatModif.Conviction:
                    StatTemp.Conviction += (Mathf.FloorToInt(item.Pourcentage / 100f)) * StatTemp.Conviction;
                    break;
                case StatModif.Resilience:
                    StatTemp.Resilience += (Mathf.FloorToInt(item.Pourcentage / 100f)) * StatTemp.Resilience;
                    break;
                case StatModif.Vitesse:
                    StatTemp.Vitesse += (Mathf.FloorToInt(item.Pourcentage / 100f)) * StatTemp.Vitesse;
                    break;
                case StatModif.VolonterMax:
                    StatTemp.VolonterMax += (Mathf.FloorToInt(item.Pourcentage / 100f)) * StatTemp.VolonterMax;
                    break;
            }
        }
    }

    #endregion Application Souvenir

    #region Equiped

    public void TryToEquiped(ReorderableListEventStruct e)
    {
        if(NbSlotsEquiped + e.DroppedObject.GetComponent<SouvenirUI>().LeSouvenir.Slots > StatTemp.SlotsSouvenir)
        {
            e.DroppedObject.GetComponent<ReorderableListElement>().IsTransferable = false;
        }
    }

    public void Equiped(ReorderableListEventStruct e)
    {
        if (e.DroppedObject.GetComponent<SouvenirUI>().LeSouvenir.Equiped == false && NbSlotsEquiped+e.DroppedObject.GetComponent<SouvenirUI>().LeSouvenir.Slots <= StatTemp.SlotsSouvenir)
        {
            e.DroppedObject.GetComponent<SouvenirUI>().LeSouvenir.Equiped = true;
            EquipedSouvenir.Add(e.DroppedObject.GetComponent<SouvenirUI>().LeSouvenir);
            ModifStat(e.DroppedObject.GetComponent<SouvenirUI>().LeSouvenir, true);
            NbSlotsEquiped += e.DroppedObject.GetComponent<SouvenirUI>().LeSouvenir.Slots;
        }
    }

    public void UnEquiped(ReorderableListEventStruct e)
    {
        if (e.DroppedObject.GetComponent<SouvenirUI>().LeSouvenir.Equiped == true)
        {
            e.DroppedObject.GetComponent<SouvenirUI>().LeSouvenir.Equiped = false;
            EquipedSouvenir.Remove(e.DroppedObject.GetComponent<SouvenirUI>().LeSouvenir);
            ModifStat(e.DroppedObject.GetComponent<SouvenirUI>().LeSouvenir, false);
            NbSlotsEquiped -= e.DroppedObject.GetComponent<SouvenirUI>().LeSouvenir.Slots;
        }
        e.DroppedObject.GetComponent<ReorderableListElement>().IsTransferable = true;
    }

    #endregion Equiped

    #region End

    public void End()
    {
        StatTemp.ListSouvenir = EquipedSouvenir;
        GameManager.instance.playerStat = StatTemp;
        StartCoroutine(GameManager.instance.pmm.EndMenuStat());
    }

    #endregion End

    #region Arbre de Competence

    public void afficherArbre()
    {
        Menu.SetActive(false);
        ArbreCompetence = Instantiate(ArbreCompetencePrefab, Canvas.transform);
        ArbreCompetence.GetComponentInChildren<RetourArbre>().Act = NonAfficherArbre;
        ArbreCompetence.GetComponent<ArbreManager>().StartArbre(StatTemp);
    }

    public void NonAfficherArbre()
    {
        Menu.SetActive(true);
        Destroy(ArbreCompetence);
    }

    #endregion Arbre de Competence
}
