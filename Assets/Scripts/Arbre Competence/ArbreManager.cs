using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class ArbreManager : MonoBehaviour
{
    public JoueurStat Stat;
    public List<Competence> Class;
    public int NbMaxSpell;
    public int ReductionCout;

    public TextMeshProUGUI ValeurRadiance, ValeurFA, ValeurVitesse, ValeurConviction, ValeurResilience, ValeurCalme, ValeurVolonter, ValeurConscience, ValeurClairvoyance, ValeurEssence;
    public TextMeshProUGUI SpellEquipedText;
    public List<GameObject> PanelCompetence, SpellEquipedGO;
    public List<Spell> SpellEquiped;

    public GameObject SpellPrefab, SpellsSpawn;

    #region Start&End

    public void StartArbre(JoueurStat _Stat)
    {
        Stat = _Stat;
        Class = GameManager.instance.classSO.Competences;
        NbMaxSpell = GameManager.instance.classSO.NbMaxSpell;
        InstantiateSpell();
        InstantiateArbre();
        TextStat();
        EnabledCompetence();
    }

    private void OnEnable()
    {
        StartArbreMenuStat(GameManager.instance.playerStat);
    }

    public void StartArbreMenuStat(JoueurStat _Stat)
    {
        Stat = _Stat;
        Class = GameManager.instance.classSO.Competences;
        NbMaxSpell = GameManager.instance.classSO.NbMaxSpell;
        InstantiateSpell();
        InstantiateArbre();
        TextStat();
        //for (int i = 0; i < PanelCompetence.Count; i++)
        //{
        //    PanelCompetence[i].GetComponent<ContainerCompetence>().Consultation();
        //}
        //this.GetComponentInChildren<ReorderableList>().IsDraggable = false;

    }

    public void InstantiateSpell()
    {
        foreach (var item in Stat.ListSpell)
        {
            var temp = Instantiate(SpellPrefab, SpellsSpawn.transform);
            temp.GetComponent<SpellCombat>().Action = item;
            temp.GetComponent<SpellCombat>().StartUp();
            temp.GetComponent<SpellCombat>().isTurn = false;
            var Colors = temp.GetComponent<Button>().colors;
            Colors.disabledColor = Color.white;
            temp.GetComponent<Button>().colors = Colors;

            SpellEquipedGO.Add(temp);
            SpellEquiped.Add(temp.GetComponent<SpellCombat>().Action);
        }
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
        //PanelCompetence[0].GetComponent<ContainerCompetence>().LaCompetence = Class.First(c => c.Spell.name == i);
        //PanelCompetence[0].GetComponent<ContainerCompetence>().Affichage();
        //ContainerCompetence temp = PanelCompetence[0].GetComponent<ContainerCompetence>();
        //PanelCompetence[0].GetComponent<ContainerCompetence>().ButtonBuy.onClick.AddListener(() => { Buy(temp); });
        for (int i = 0; i < PanelCompetence.Count; i++)
        {
            PanelCompetence[i].GetComponent<ContainerCompetence>().LaCompetence = Class.First(c => c.IDLvl == i);
            PanelCompetence[i].GetComponent<ContainerCompetence>().Affichage();
            ContainerCompetence temp = PanelCompetence[i].GetComponent<ContainerCompetence>();
            //PanelCompetence[i].GetComponent<ContainerCompetence>().ButtonBuy.onClick.AddListener(() => { Buy(PanelCompetence[i].GetComponent<ContainerCompetence>()); });
            PanelCompetence[i].GetComponent<ContainerCompetence>().ButtonBuy.onClick.AddListener(() => { Buy(temp); });
            //PanelCompetence[i].GetComponent<ContainerCompetence>().ButtonEquip.onClick.AddListener(() => { Equip(temp); });
            //PanelCompetence[i].GetComponent<ContainerCompetence>().ButtonUnEquip.onClick.AddListener(() => { UnEquip(temp); });
        }
        UpdateCout();
    }

    public void ReordableSpell()
    {
        SpellEquiped.Clear();
        var temp = this.GetComponentInChildren<ReorderableListContent>().gameObject;
        for (int i = 0; i < temp.transform.childCount; i++)
        {
            SpellEquiped.Add(temp.transform.GetChild(i).gameObject.GetComponent<SpellCombat>().Action);
        }
    }

    #endregion Start&End

    #region Update

    public void EnabledCompetence()
    {
        for (int i = 1; i < PanelCompetence.Count; i++)
        {
            PanelCompetence[i].GetComponent<ContainerCompetence>().ButtonBuy.interactable = false;
            if (Stat.Essence >= PanelCompetence[i].GetComponent<ContainerCompetence>().LaCompetence.Essence)
            {
                for (int y = 0; y < PanelCompetence[i].GetComponent<ContainerCompetence>().LaCompetence.IDLier.Count; y++)
                {
                    if (Class.First(c => c.IDLvl == PanelCompetence[i].GetComponent<ContainerCompetence>().LaCompetence.IDLier[y]).Bought == true)
                    {
                        PanelCompetence[i].GetComponent<ContainerCompetence>().ButtonBuy.interactable = true;
                    }
                }
            }
        }
    }

    public void UpdateCout()
    {
        foreach (var item in PanelCompetence)
        {
            item.GetComponent<ContainerCompetence>().LaCompetence.Essence = item.GetComponent<ContainerCompetence>().LaCompetence.EssenceOriginal;
            for (int i = 0; i < item.GetComponent<ContainerCompetence>().LaCompetence.IDLier.Count; i++)
            {
                if (Class.First(c => c.IDLvl == item.GetComponent<ContainerCompetence>().LaCompetence.IDLier[i]).Bought == true && Class.First(c => c.IDLvl == item.GetComponent<ContainerCompetence>().LaCompetence.IDLier[i]).IDLvl != 0)
                {
                    item.GetComponent<ContainerCompetence>().LaCompetence.Essence -= ReductionCout;
                }
            }
            if (item.GetComponent<ContainerCompetence>().LaCompetence.IDLvl != 0)
            {
                item.GetComponent<ContainerCompetence>().Affichage();
            }
        }
    }

    public void Update()
    {
        //if (SpellEquiped.Count == NbMaxSpell)
        //{
        //    for (int i = 0; i < PanelCompetence.Count; i++)
        //    {
        //        PanelCompetence[i].GetComponent<ContainerCompetence>().ButtonEquip.interactable = false;
        //    }
        //}
        //else
        //{
        //    for (int i = 0; i < PanelCompetence.Count; i++)
        //    {
        //        PanelCompetence[i].GetComponent<ContainerCompetence>().ButtonEquip.interactable = true;
        //    }
        //}
        //if (SpellEquiped == null)
        //    return;
        //SpellEquipedText.text = "Sort Equiper : " + SpellEquiped.Count + "/" + NbMaxSpell;
    }

    #endregion Update

    #region Interraction Competence

    public void Buy(ContainerCompetence Competence)
    {
        Stat.Essence -= Competence.LaCompetence.Essence;
        Competence.LaCompetence.Bought = true;
        Competence.AffichageAchat.GetComponent<Image>().sprite = Competence.Acheter;
        Competence.ButtonBuy.gameObject.SetActive(false);
        Competence.ButtonEquip.gameObject.SetActive(true);
        ModifStat(Competence.LaCompetence);
        UpdateCout();
        TextStat();
        EnabledCompetence();
        Equip(Competence);
    }

    public void Equip(ContainerCompetence Competence)
    {
        Competence.LaCompetence.Equiped = true;
        var temp = Instantiate(SpellPrefab, SpellsSpawn.transform);
        temp.GetComponent<SpellCombat>().Action = Competence.LaCompetence.Spell;
        temp.GetComponent<SpellCombat>().StartUp();
        temp.GetComponent<SpellCombat>().isTurn = false;
        var Colors = temp.GetComponent<Button>().colors;
        Colors.disabledColor = Color.white;
        temp.GetComponent<Button>().colors = Colors;

        SpellEquipedGO.Add(temp);
        SpellEquiped.Add(temp.GetComponent<SpellCombat>().Action);

        Competence.ButtonEquip.gameObject.SetActive(false);
        Competence.ButtonUnEquip.gameObject.SetActive(true);

        GameManager.instance.playerStat.ListSpell.Add(Competence.LaCompetence.Spell);

        UiMondeManager ui = GameManager.instance.rm.gameObject.GetComponent<UiMondeManager>();
        ui.EnableMonde();
    }

    public void UnEquip(ContainerCompetence Competence)
    {
        Competence.LaCompetence.Equiped = false;
        int PosSpell = 0;
        for (int i = 0; i < SpellEquipedGO.Count; i++)
        {
            if (SpellEquipedGO[i].GetComponent<SpellCombat>().Action.Nom == SpellEquiped.First(c => c.Nom == Competence.LaCompetence.Spell.Nom).Nom)
            {
                PosSpell = i;
            }
        }
        Destroy(SpellEquipedGO[PosSpell]);
        SpellEquipedGO.Remove(SpellEquipedGO[PosSpell]);
        SpellEquiped.Remove(SpellEquiped.First(c => c.Nom == Competence.LaCompetence.Spell.Nom));


        Competence.ButtonEquip.gameObject.SetActive(true);
        Competence.ButtonUnEquip.gameObject.SetActive(false);
    }

    public void ModifStat(Competence LaCompetence)
    {
        foreach (var item in LaCompetence.ModifStat)
        {
            switch (item.StatModif)
            {
                case StatModif.RadianceMax:
                    Stat.RadianceMax += item.Valeur;
                    Stat.Radiance += item.Valeur;
                    break;
                case StatModif.ForceAme:
                    Stat.ForceAme += item.Valeur;
                    break;
                case StatModif.Calme:
                    Stat.Calme += item.Valeur;
                    break;
                case StatModif.Clairvoyance:
                    Stat.Clairvoyance += item.Valeur;
                    break;
                case StatModif.ConscienceMax:
                    Stat.ConscienceMax += item.Valeur;
                    break;
                case StatModif.Conviction:
                    Stat.Conviction += item.Valeur;
                    break;
                case StatModif.Resilience:
                    Stat.Resilience += item.Valeur;
                    break;
                case StatModif.Vitesse:
                    Stat.Vitesse += item.Valeur;
                    break;
                case StatModif.VolonterMax:
                    Stat.VolonterMax += item.Valeur;
                    break;
            }
        }
        Stat.RectificationStat();
    }

    #endregion Interraction Competence


}
