using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI.Extensions;
using static UnityEngine.UI.Extensions.ReorderableList;
using UnityEngine.UI;
using System.Linq;

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

    public void OnEnable/*MenuStat*/()
    {
        foreach (var item in Souvenir)
        {
            Destroy(item);
        }
        Stat = GameManager.Instance.playerStat;
        StatTemp = Instantiate(Stat);
        foreach (var item in StatTemp.ListSouvenir)
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
                    Stat.RadianceMax -= item.ParametreModifStat.ValeurModifier;
                    break;
                case StatModif.ForceAme:
                    Stat.ForceAme -= item.ParametreModifStat.ValeurModifier;
                    break;
                case StatModif.Calme:
                    Stat.Calme -= item.ParametreModifStat.ValeurModifier;
                    break;
                case StatModif.Clairvoyance:
                    Stat.Clairvoyance -= item.ParametreModifStat.ValeurModifier;
                    break;
                case StatModif.ConscienceMax:
                    Stat.ConscienceMax -= item.ParametreModifStat.ValeurModifier;
                    break;
                case StatModif.Conviction:
                    Stat.Conviction -= item.ParametreModifStat.ValeurModifier;
                    break;
                case StatModif.Resilience:
                    Stat.Resilience -= item.ParametreModifStat.ValeurModifier;
                    break;
                case StatModif.Vitesse:
                    Stat.Vitesse -= item.ParametreModifStat.ValeurModifier;
                    break;
                case StatModif.VolonterMax:
                    Stat.VolonterMax -= item.ParametreModifStat.ValeurModifier;
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
                        var Temp = 0;
                        if(item.ParametreModifStat.ParametreStat == ParametreStat.Pourcentage)
                        {
                            Temp = Mathf.FloorToInt((item.ParametreModifStat.Valeur / 100f) * Stat.RadianceMax);
                        }
                        else if (item.ParametreModifStat.ParametreStat == ParametreStat.ValeurBrut)
                        {
                            Temp = item.ParametreModifStat.Valeur;
                        }
                        StatTemp.RadianceMax += Temp;
                        StatTemp.Radiance += Temp;
                        item.ParametreModifStat.ValeurModifier = Temp;
                    }
                    else
                    {
                        StatTemp.RadianceMax -= item.ParametreModifStat.ValeurModifier;
                        StatTemp.Radiance -= item.ParametreModifStat.ValeurModifier;
                    }
                    break;
                case StatModif.ForceAme:
                    if (Equiped == true)
                    {
                        var Temp = 0;
                        if (item.ParametreModifStat.ParametreStat == ParametreStat.Pourcentage)
                        {
                            Temp = Mathf.FloorToInt((item.ParametreModifStat.Valeur / 100f) * Stat.ForceAme);
                        }
                        else if (item.ParametreModifStat.ParametreStat == ParametreStat.ValeurBrut)
                        {
                            Temp = item.ParametreModifStat.Valeur;
                        }
                        StatTemp.ForceAme += Temp;
                        item.ParametreModifStat.ValeurModifier = Temp;
                    }
                    else
                    {
                        StatTemp.ForceAme -= item.ParametreModifStat.ValeurModifier;
                    }
                    break;
                case StatModif.Calme:
                    if (Equiped == true)
                    {
                        var Temp = 0;
                        if (item.ParametreModifStat.ParametreStat == ParametreStat.Pourcentage)
                        {
                            Temp = Mathf.FloorToInt((item.ParametreModifStat.Valeur / 100f) * Stat.Calme);
                        }
                        else if (item.ParametreModifStat.ParametreStat == ParametreStat.ValeurBrut)
                        {
                            Temp = item.ParametreModifStat.Valeur;
                        }
                        StatTemp.Calme += Temp;
                        item.ParametreModifStat.ValeurModifier = Temp;
                    }
                    else
                    {
                        StatTemp.Calme -= item.ParametreModifStat.ValeurModifier;
                    }
                    break;
                case StatModif.Clairvoyance:
                    if (Equiped == true)
                    {
                        var Temp = 0;
                        if (item.ParametreModifStat.ParametreStat == ParametreStat.Pourcentage)
                        {
                            Temp = Mathf.FloorToInt((item.ParametreModifStat.Valeur / 100f) * Stat.Clairvoyance);
                        }
                        else if (item.ParametreModifStat.ParametreStat == ParametreStat.ValeurBrut)
                        {
                            Temp = item.ParametreModifStat.Valeur;
                        }
                        StatTemp.Clairvoyance += Temp;
                        item.ParametreModifStat.ValeurModifier = Temp;
                    }
                    else
                    {
                        StatTemp.Clairvoyance -= item.ParametreModifStat.ValeurModifier;
                    }
                    break;
                case StatModif.ConscienceMax:
                    if (Equiped == true)
                    {
                        var Temp = 0;
                        if (item.ParametreModifStat.ParametreStat == ParametreStat.Pourcentage)
                        {
                            Temp = Mathf.FloorToInt((item.ParametreModifStat.Valeur / 100f) * Stat.ConscienceMax);
                        }
                        else if (item.ParametreModifStat.ParametreStat == ParametreStat.ValeurBrut)
                        {
                            Temp = item.ParametreModifStat.Valeur;
                        }
                        StatTemp.ConscienceMax += Temp;
                        item.ParametreModifStat.ValeurModifier = Temp;
                    }
                    else
                    {
                        StatTemp.ConscienceMax -= item.ParametreModifStat.ValeurModifier;
                    }
                    break;
                case StatModif.Conviction:
                    if (Equiped == true)
                    {
                        var Temp = 0;
                        if (item.ParametreModifStat.ParametreStat == ParametreStat.Pourcentage)
                        {
                            Temp = Mathf.FloorToInt((item.ParametreModifStat.Valeur / 100f) * Stat.Conviction);
                        }
                        else if (item.ParametreModifStat.ParametreStat == ParametreStat.ValeurBrut)
                        {
                            Temp = item.ParametreModifStat.Valeur;
                        }
                        StatTemp.Conviction += Temp;
                        item.ParametreModifStat.ValeurModifier = Temp;
                    }
                    else
                    {
                        StatTemp.Conviction -= item.ParametreModifStat.ValeurModifier;
                    }
                    break;
                case StatModif.Resilience:
                    if (Equiped == true)
                    {
                        var Temp = 0;
                        if (item.ParametreModifStat.ParametreStat == ParametreStat.Pourcentage)
                        {
                            Temp = Mathf.FloorToInt((item.ParametreModifStat.Valeur / 100f) * Stat.Resilience);
                        }
                        else if (item.ParametreModifStat.ParametreStat == ParametreStat.ValeurBrut)
                        {
                            Temp = item.ParametreModifStat.Valeur;
                        }

                        var tempResiliencePassif = StatTemp.ResiliencePassif;
                        StatTemp.ResiliencePassif = 0;
                        StatTemp.Resilience += Temp;
                        StatTemp.ResiliencePassif = tempResiliencePassif;
                        item.ParametreModifStat.ValeurModifier = Temp;
                    }
                    else
                    {
                        var tempResiliencePassif = StatTemp.ResiliencePassif;
                        StatTemp.ResiliencePassif = 0;
                        StatTemp.Resilience -= item.ParametreModifStat.ValeurModifier;
                        StatTemp.ResiliencePassif = tempResiliencePassif;
                    }
                    break;
                case StatModif.Vitesse:
                    if (Equiped == true)
                    {
                        var Temp = 0;
                        if (item.ParametreModifStat.ParametreStat == ParametreStat.Pourcentage)
                        {
                            Temp = Mathf.FloorToInt((item.ParametreModifStat.Valeur / 100f) * Stat.Vitesse);
                        }
                        else if (item.ParametreModifStat.ParametreStat == ParametreStat.ValeurBrut)
                        {
                            Temp = item.ParametreModifStat.Valeur;
                        }
                        StatTemp.Vitesse += Temp;
                        item.ParametreModifStat.ValeurModifier = Temp;
                    }
                    else
                    {
                        StatTemp.Vitesse -= item.ParametreModifStat.ValeurModifier;
                    }
                    break;
                case StatModif.VolonterMax:
                    if (Equiped == true)
                    {
                        var Temp = 0;
                        if (item.ParametreModifStat.ParametreStat == ParametreStat.Pourcentage)
                        {
                            Temp = Mathf.FloorToInt((item.ParametreModifStat.Valeur / 100f) * Stat.VolonterMax);
                        }
                        else if (item.ParametreModifStat.ParametreStat == ParametreStat.ValeurBrut)
                        {
                            Temp = item.ParametreModifStat.Valeur;
                        }
                        StatTemp.VolonterMax += Temp;
                        StatTemp.Volonter += Temp;
                        item.ParametreModifStat.ValeurModifier = Temp;
                    }
                    else
                    {
                        StatTemp.VolonterMax -= item.ParametreModifStat.ValeurModifier;
                        StatTemp.Volonter -= item.ParametreModifStat.ValeurModifier;
                    }
                    break;
            }
        }
        StatTemp.RectificationStat();
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
            GameManager.Instance.CopyAllSouvenir.Add(e.DroppedObject.GetComponent<SouvenirUI>().LeSouvenir);
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

        GameManager.Instance.CopyAllSouvenir.AddRange(EquipedSouvenir.Where(c => !c.Equiped));
        EquipedSouvenir.RemoveAll(c => !c.Equiped);
        StatTemp.ListSouvenir = EquipedSouvenir.ToList();
        NbSlotsEquiped = 0;
        GameManager.Instance.playerStat = StatTemp;
        GameManager.Instance.pmm.EndMenuStat();
        EquipedSouvenir.Clear();
    }

    #endregion End

    #region Arbre de Competence

    public void afficherArbre()
    {
        Menu.SetActive(false);
        ArbreCompetence = Instantiate(ArbreCompetencePrefab, Canvas.transform);
        ArbreCompetence.GetComponentInChildren<RetourArbre>().gameObject.GetComponent<Button>().onClick.AddListener(NonAfficherArbre);
        ArbreCompetence.GetComponent<ArbreManager>().StartArbreMenuStat(StatTemp);
    }

    public void NonAfficherArbre()
    {
        Menu.SetActive(true);
        Destroy(ArbreCompetence);
    }

    #endregion Arbre de Competence
}
