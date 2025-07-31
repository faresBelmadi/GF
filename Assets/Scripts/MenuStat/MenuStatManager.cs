using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI.Extensions;
using static UnityEngine.UI.Extensions.ReorderableList;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Rendering;
using System;

public class MenuStatManager : MonoBehaviour
{
    public PlayerStatsHandler  Stat, StatTemp;
    public GameObject SouvenirPrefab;
    [SerializeField]
    private GameObject SouvenirPrefab2;
    public GameObject SouvenirSpawnEquiped;
    public GameObject SouvenirSpawnUnEquiped;
    public List<GameObject> Souvenir;
    public List<Souvenir> EquipedSouvenir;

    public TextMeshProUGUI ValeurRadiance, ValeurFA, ValeurVitesse, ValeurConviction, ValeurResilience, ValeurCalme, ValeurVolonter, ValeurConscience, ValeurClairvoyance;
    public TextMeshProUGUI ModifRadiance, ModifFA, ModifVitesse, ModifConviction, ModifResilience, ModifCalme, ModifVolonter, ModifConscience, ModifClairvoyance;

    private int NbSlotsEquiped;
    public TextMeshProUGUI NbSlots;
    [SerializeField]
    private Image _slotEquiped;
    [SerializeField]
    private Image _overPoweredSlot;

    public GameObject ArbreCompetencePrefab;
    public GameObject ArbreCompetence, Canvas, Menu;

    public List<SouvenirUI> ListSouvenirUIEquipped { get; private set; } = new List<SouvenirUI>();

    public bool IsExplicationVisible { get; private set; } = false;
    [SerializeField]
    private GameObject _explicationPanel;
    private bool _isFirsVisit = true;
    #region Start

    public void OnEnable/*MenuStat*/()
    {
        TutoManager.OnEndTuto += ResetStat;
        foreach (var item in Souvenir)
        {
            Destroy(item);
        }

        if (_isFirsVisit && !GameManager.Instance.IsTuto)
        {
            ShowExplicationPanel();
            _isFirsVisit = false;
        }
        else
        {
            HideExplicationPanel();
        }

        if (!GameManager.Instance.IsTuto)
            Stat = GameManager.Instance.playerStatHandler;
        else
            Stat = null;
        StatTemp = new PlayerStatsHandler(Stat);
        SouvenirSpawnEquiped.GetComponent<Cristopher>().InitCristopher();
        ListSouvenirUIEquipped.Clear();
        foreach (var item in StatTemp.ListSouvenir)
        {
            GameObject temp;
            if (item.Equiped == true)
            {
                ResetStatEnter(item);
                NbSlotsEquiped += item.Slots;
                temp = Instantiate(SouvenirPrefab, SouvenirSpawnEquiped.GetComponent<Cristopher>().GetDropZone());
                ListSouvenirUIEquipped.Add(temp.GetComponent<SouvenirUI>());
            }
            else if (GameManager.Instance.IsTuto)
            {
                temp = Instantiate(SouvenirPrefab, SouvenirSpawnUnEquiped.transform);
                Destroy(temp.GetComponent<DraggableElement>());
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
        SouvenirSpawnEquiped.GetComponent<Cristopher>().RearrangeSouvenir();
        SouvenirSpawnEquiped.GetComponent<Cristopher>().ActivateCurrentSlot();
        UpdateStatUI();
    }
    private void OnDisable()
    {
        TutoManager.OnEndTuto -= ResetStat;
    }
    //Bon Pour delete
    public void Loot()
    {
        var ind = UnityEngine.Random.Range(0, GameManager.Instance.CopyAllSouvenir.Count());
        var newSouvenir = GameManager.Instance.CopyAllSouvenir[ind];
        GameManager.Instance.playerStatHandler.ListSouvenir.Add(Instantiate(newSouvenir));
        GameManager.Instance.CopyAllSouvenir.Remove(newSouvenir);

        Stat = GameManager.Instance.playerStatHandler;
        StatTemp = new PlayerStatsHandler(Stat);
        SouvenirSpawnEquiped.GetComponent<Cristopher>().InitCristopher();
        ListSouvenirUIEquipped.Clear();
     
            GameObject temp;
           
                temp = Instantiate(SouvenirPrefab, SouvenirSpawnUnEquiped.transform);
            
            temp.GetComponent<SouvenirUI>().LeSouvenir = newSouvenir;
            temp.GetComponent<SouvenirUI>().StartUp();
           
            Souvenir.Add(temp);
        
        SouvenirSpawnEquiped.GetComponent<Cristopher>().RearrangeSouvenir();
        SouvenirSpawnEquiped.GetComponent<Cristopher>().ActivateCurrentSlot();
        UpdateStatUI();
    }
    public void ShowExplicationPanel()
    {
        IsExplicationVisible = true;
        _explicationPanel.SetActive(true);
    }
    public void HideExplicationPanel()
    {
        IsExplicationVisible = false;
        _explicationPanel.SetActive(false);
    }
    public void ResetStatEnter(Souvenir LeSouvenir)
    {
        JoueurStat modifStat = ScriptableObject.CreateInstance<JoueurStat>();
        foreach (var item in LeSouvenir.ModificationStat)
        {
            switch (item.StatModif)
            {
                case StatModif.RadianceMax:
                    modifStat.RadianceMax -= item.ParametreModifStat.ValeurModifier;
                    break;
                case StatModif.ForceAme:
                    modifStat.ForceAme -= item.ParametreModifStat.ValeurModifier;
                    break;
                case StatModif.Calme:
                    modifStat.Calme -= item.ParametreModifStat.ValeurModifier;
                    break;
                case StatModif.Clairvoyance:
                    modifStat.Clairvoyance -= item.ParametreModifStat.ValeurModifier;
                    break;
                case StatModif.ConscienceMax:
                    modifStat.ConscienceMax -= item.ParametreModifStat.ValeurModifier;
                    break;
                case StatModif.Conviction:
                    modifStat.Conviction -= item.ParametreModifStat.ValeurModifier;
                    break;
                case StatModif.Resilience:
                    modifStat.Resilience -= item.ParametreModifStat.ValeurModifier;
                    break;
                case StatModif.Vitesse:
                    modifStat.Vitesse -= item.ParametreModifStat.ValeurModifier;
                    break;
                case StatModif.VolonterMax:
                    modifStat.VolonterMax -= item.ParametreModifStat.ValeurModifier;
                    break;
            }
        }
        Stat.UpdateStat(modifStat);
        ScriptableObject.Destroy(modifStat);
    }

    #endregion Start

    #region Update

    void Update()
    {
        UpdateStatUI();
    }
    public void ResetStat()
    {
        Stat = GameManager.Instance.playerStatHandler;
    }
    public void UpdateStatUI()
    {
        ValeurRadiance.text = StatTemp.Radiance.ToString() + "/" + StatTemp.RadianceMax;
        ValeurFA.text = StatTemp.ForceAme.ToString();
        ValeurVitesse.text = StatTemp.Vitesse.ToString();
        ValeurConviction.text = StatTemp.Conviction.ToString();
        ValeurResilience.text = StatTemp.Resilience.ToString();
        ValeurCalme.text = StatTemp.Calme.ToString();
        ValeurVolonter.text = StatTemp.Volonte.ToString() + "/" + StatTemp.VolonteMax.ToString();
        ValeurConscience.text = StatTemp.Conscience.ToString() + "/" + StatTemp.ConscienceMax.ToString();
        ValeurClairvoyance.text = StatTemp.Clairvoyance.ToString();

        int radiance = 0, forcedame = 0, conviction = 0, vitesse = 0, resilience = 0, calme = 0, conscience = 0, volonte = 0, clairvoyance = 0;
        
        foreach (Souvenir memory in StatTemp.ListSouvenir)
        {
            if (memory.Equiped == false)
                continue;
            foreach (var item in memory.ModificationStat)
            {
                switch (item.StatModif)
                {
                    case StatModif.RadianceMax:
                        radiance += item.ParametreModifStat.ValeurModifier;
                        break;
                    case StatModif.ForceAme:
                        forcedame += item.ParametreModifStat.ValeurModifier;
                        break;
                    case StatModif.Vitesse:
                        vitesse += item.ParametreModifStat.ValeurModifier;
                        break;
                    case StatModif.Resilience:
                        resilience += item.ParametreModifStat.ValeurModifier;
                        break;
                    case StatModif.Conviction:
                        conviction += item.ParametreModifStat.ValeurModifier;
                        break;
                    case StatModif.ConscienceMax:
                        conscience += item.ParametreModifStat.ValeurModifier;
                        break;
                    case StatModif.Clairvoyance:
                        clairvoyance += item.ParametreModifStat.ValeurModifier;
                        break;
                    case StatModif.Calme:
                        calme += item.ParametreModifStat.ValeurModifier;
                        break;
                    case StatModif.VolonterMax:
                        volonte += item.ParametreModifStat.ValeurModifier;
                        break;
                }
            }
        }

        ModifTempsReel(radiance, ModifRadiance);
        ModifTempsReel(forcedame, ModifFA);
        ModifTempsReel(vitesse, ModifVitesse);
        ModifTempsReel(conviction, ModifConviction);
        ModifTempsReel(resilience, ModifResilience);
        ModifTempsReel(calme, ModifCalme);
        ModifTempsReel(volonte, ModifVolonter);
        ModifTempsReel(conscience, ModifConscience);
        ModifTempsReel(clairvoyance, ModifClairvoyance);

        NbSlots.text = NbSlotsEquiped + "/" + StatTemp.SlotsSouvenir;
        _slotEquiped.fillAmount = (NbSlotsEquiped * StatTemp.SlotsSouvenir) / 100f;
        if (NbSlotsEquiped > StatTemp.SlotsSouvenir)
        {
            _overPoweredSlot.fillAmount = (NbSlotsEquiped % StatTemp.SlotsSouvenir) * 4 / 100f;
        }
        else
            _overPoweredSlot.fillAmount = 0;
    }
    public void ModifTempsReel(int modifValue, TextMeshProUGUI Text)
    {
        if (modifValue < 0)
        {
            Text.color = Color.red;
            Text.text = "(";
        }
        else if (modifValue > 0)
        {
            Text.color = Color.green;
            Text.text = "(+";
        }
        else 
        {
            Text.color = Color.grey;
            //Text.text = "(";
            Text.text = "";
            return;         // we display nothing when there are no modifications
        }
        Text.text += (modifValue).ToString() + ")";
    }

    #endregion Update

    #region Application Souvenir

    public void ModifStat(Souvenir LeSouvenir, bool Equiped)
    {
        JoueurStat modifStat = ScriptableObject.CreateInstance<JoueurStat>();
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
                        modifStat.RadianceMax += Temp;
                        modifStat.Radiance += Temp;
                        item.ParametreModifStat.ValeurModifier = Temp;
                    }
                    else
                    {
                        modifStat.RadianceMax -= item.ParametreModifStat.ValeurModifier;
                        modifStat.Radiance -= item.ParametreModifStat.ValeurModifier;
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
                        modifStat.ForceAme += Temp;
                        item.ParametreModifStat.ValeurModifier = Temp;
                    }
                    else
                    {
                        modifStat.ForceAme -= item.ParametreModifStat.ValeurModifier;
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
                        modifStat.Calme += Temp;
                        item.ParametreModifStat.ValeurModifier = Temp;
                    }
                    else
                    {
                        modifStat.Calme -= item.ParametreModifStat.ValeurModifier;
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
                        modifStat.Clairvoyance += Temp;
                        item.ParametreModifStat.ValeurModifier = Temp;
                    }
                    else
                    {
                        modifStat.Clairvoyance -= item.ParametreModifStat.ValeurModifier;
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
                        modifStat.ConscienceMax += Temp;
                        item.ParametreModifStat.ValeurModifier = Temp;
                    }
                    else
                    {
                        modifStat.ConscienceMax -= item.ParametreModifStat.ValeurModifier;
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
                        modifStat.Conviction += Temp;
                        item.ParametreModifStat.ValeurModifier = Temp;
                    }
                    else
                    {
                        modifStat   .Conviction -= item.ParametreModifStat.ValeurModifier;
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
                        modifStat.ResiliencePassif = 0;
                        modifStat.Resilience += Temp;
                        modifStat.ResiliencePassif = tempResiliencePassif;
                        item.ParametreModifStat.ValeurModifier = Temp;
                    }
                    else
                    {
                        var tempResiliencePassif = StatTemp.ResiliencePassif;
                        modifStat.ResiliencePassif = 0;
                        modifStat.Resilience -= item.ParametreModifStat.ValeurModifier;
                        modifStat.ResiliencePassif = tempResiliencePassif;
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
                        modifStat.Vitesse += Temp;
                        item.ParametreModifStat.ValeurModifier = Temp;
                    }
                    else
                    {
                        modifStat.Vitesse -= item.ParametreModifStat.ValeurModifier;
                    }
                    break;
                case StatModif.VolonterMax:
                    if (Equiped == true)
                    {
                        var Temp = 0;
                        if (item.ParametreModifStat.ParametreStat == ParametreStat.Pourcentage)
                        {
                            Temp = Mathf.FloorToInt((item.ParametreModifStat.Valeur / 100f) * Stat.VolonteMax);
                        }
                        else if (item.ParametreModifStat.ParametreStat == ParametreStat.ValeurBrut)
                        {
                            Temp = item.ParametreModifStat.Valeur;
                        }
                        modifStat.VolonterMax += Temp;
                        modifStat.Volonter += Temp;
                        item.ParametreModifStat.ValeurModifier = Temp;
                    }
                    else
                    {
                        modifStat.VolonterMax -= item.ParametreModifStat.ValeurModifier;
                        modifStat.Volonter -= item.ParametreModifStat.ValeurModifier;
                    }
                    break;
            }
        }
        StatTemp.UpdateStat(modifStat);
        ScriptableObject.Destroy(modifStat);
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

    [Obsolete]
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
    public bool Equiped(SouvenirUI souv)
    {
        if (souv.LeSouvenir.Equiped == false && NbSlotsEquiped + souv.LeSouvenir.Slots <= StatTemp.SlotsSouvenir)
        {
            souv.LeSouvenir.Equiped = true;
            EquipedSouvenir.Add(souv.LeSouvenir);
            ListSouvenirUIEquipped.Add(souv);
            ModifStat(souv.LeSouvenir, true);
            NbSlotsEquiped += souv.LeSouvenir.Slots;
            if (GameManager.Instance.CopyAllSouvenir.Contains(souv.LeSouvenir))
            {
                GameManager.Instance.CopyAllSouvenir.Remove(souv.LeSouvenir);
            }
            return true;
        }
        return false;
    }

    [Obsolete]
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
    public bool UnEquiped(SouvenirUI souv)
    {
        if (souv.LeSouvenir.Equiped == true)
        {
            souv.LeSouvenir.Equiped = false;
            GameManager.Instance.CopyAllSouvenir.Add(souv.LeSouvenir);
            EquipedSouvenir.Remove(souv.LeSouvenir);
            ListSouvenirUIEquipped.Remove(souv);
            ModifStat(souv.LeSouvenir, false);
            NbSlotsEquiped -= souv.LeSouvenir.Slots;
            return true;
        }
        return false;
    }
   

    #endregion Equiped

    #region End

    public void End()
    {

        GameManager.Instance.CopyAllSouvenir.AddRange(EquipedSouvenir.Where(c => !c.Equiped));
        EquipedSouvenir.RemoveAll(c => !c.Equiped);
        StatTemp.ListSouvenir = EquipedSouvenir.ToList();
        NbSlotsEquiped = 0;
        GameManager.Instance.playerStatHandler = StatTemp;
        GameManager.Instance.pmm.EndMenuStat();
        EquipedSouvenir.Clear();
    }

    #endregion End
}