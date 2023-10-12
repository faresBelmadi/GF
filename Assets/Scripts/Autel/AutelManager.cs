using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Unity.Jobs;
using UnityEditor.Experimental.GraphView;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;
using Image = UnityEngine.UI.Image;

public class AutelManager : MonoBehaviour
{
    public GameObject MenuUiPanel;
    public GameObject LevelUpUiPanel;

    [Header("Arbre")]
    public TextMeshProUGUI EssenceText;
    public TextMeshProUGUI DescriptionSpellText;
    public TextMeshProUGUI CostCapaText;

    public List<GameObject> AllSpellsIcon;
    public List<GameObject> AllLink;

    public Button BuyButton;

    public Button RetourButton;

    public TextMeshProUGUI ValeurRadiance, ValeurFA, ValeurVitesse, ValeurConviction, ValeurResilience, ValeurCalme, ValeurVolonter, ValeurConscience, ValeurClairvoyance;
    public TextMeshProUGUI ModifRadiance, ModifFA, ModifVitesse, ModifConviction, ModifResilience, ModifCalme, ModifVolonter, ModifConscience, ModifClairvoyance;


    [Header("Shop")]
    public TextMeshProUGUI TextComp1, TextComp2, TextComp3;
    public TextMeshProUGUI TextCoutComp1, TextCoutComp2, TextCoutComp3;

    public Image IconSouvenirComp3;

    public Button Comp1, Comp2, Comp3;


    void FixedUpdate()
    {
        EssenceText.text = "Essence : " + GameManager.instance.playerStat.Essence;
        SetUpStatsDescription();
    }

    public void ShowMenuUiPanel()
    {
        LevelUpUiPanel.SetActive(false);
        MenuUiPanel.SetActive(true);
    }

    public void SetLvlUpActive()
    {
        LevelUpUiPanel.SetActive(true);
        MenuUiPanel.SetActive(false);
        SetUpAllSpells();
    }

    private void SetUpAllSpells()
    {
        var listOfCompetences = GameManager.instance.classSO.Competences;

        foreach (var capa in listOfCompetences)
        {
            if (capa.Spell?.Sprite)
                AllSpellsIcon[capa.Spell.IDSpell].GetComponent<Image>().sprite = capa.Spell.Sprite;
            //spellIcon.gameObject.GetComponent<Image>().sprite = GameManager.instance.playerStat.ListSpell[0].Sprite;
        }

        SetUpStatsDescription();
    }

    public void SetUpStatsDescription()
    {
        JoueurStat stats = GameManager.instance.playerStat;
        ValeurRadiance.text = stats.RadianceMax.ToString();
        ValeurFA.text = stats.ForceAme.ToString(); 
        ValeurVitesse.text = stats.Vitesse.ToString(); 
        ValeurConviction.text = stats.Conviction.ToString(); 
        ValeurResilience.text = stats.Resilience.ToString(); 
        ValeurCalme.text = stats.Calme.ToString();
        ValeurVolonter.text = stats.VolonterMax.ToString();
        ValeurConscience.text = stats.ConscienceMax.ToString(); 
        ValeurClairvoyance.text = stats.Clairvoyance.ToString();

        //ModifRadiance.text = ModifFA.text = ModifVitesse.text = ModifConviction.text = ModifResilience.text =
        //    ModifCalme.text = ModifVolonter.text = ModifConscience.text = ModifClairvoyance.text = "";
    }

    public void SelectSpell(int Id)
    {
        BuyButton.onClick.RemoveAllListeners();
        var listOfCompetences = GameManager.instance.classSO.Competences;

        foreach (var capa in listOfCompetences)
        {
            if (capa.Spell?.IDSpell == Id)
            {

                DescriptionSpellText.text = capa.Spell.Description;
                CostCapaText.text = "cout : " + capa.EssenceCost;
                ModifStatCapa(capa);
                if (capa.EssenceCost <= GameManager.instance.playerStat.Essence && !capa.Bought)
                {
                    BuyButton.onClick.AddListener(delegate { BuyCapa(capa); });
                    BuyButton.GetComponent<Image>().color = Color.white;
                }
                else
                {
                    BuyButton.onClick.RemoveAllListeners();
                    BuyButton.GetComponent<Image>().color = Color.gray;
                }
            }
        }
    }

    public void ModifStatCapa(Competence capa)
    {
        ModifRadiance.text = ModifFA.text = ModifVitesse.text = ModifConviction.text = ModifResilience.text =
            ModifCalme.text = ModifVolonter.text = ModifConscience.text = ModifClairvoyance.text = "";
        foreach (var modifStat in capa.ModifStat)
        {
            var value = modifStat.Valeur > 0 ? "+" + modifStat.Valeur : modifStat.Valeur.ToString();
            Color colorTxt;
            colorTxt = modifStat.Valeur < 0 ? Color.red : Color.green;
            switch (modifStat.StatModif)
            {
                case StatModif.Calme:
                    ModifCalme.text = value;
                    colorTxt = modifStat.Valeur > 0 ? Color.red : Color.green;
                    ModifCalme.color = colorTxt;
                    ModifCalme.faceColor = colorTxt;
                    break;
                case StatModif.Clairvoyance:
                    ModifClairvoyance.text = value;
                    ModifClairvoyance.color = colorTxt;
                    ModifClairvoyance.faceColor = colorTxt;
                    break;
                case StatModif.ConscienceMax:
                    ModifConscience.text = value;
                    ModifConscience.color = colorTxt;
                    ModifConscience.faceColor = colorTxt;
                    break;
                case StatModif.Conviction:
                    ModifConviction.text = value;
                    ModifConviction.color = colorTxt;
                    ModifConviction.faceColor = colorTxt;
                    break;
                case StatModif.ForceAme:
                    ModifFA.text = value;
                    ModifFA.color = colorTxt;
                    ModifFA.faceColor = colorTxt;
                    break;
                case StatModif.RadianceMax:
                    ModifRadiance.text = value;
                    ModifRadiance.color = colorTxt;
                    ModifRadiance.faceColor = colorTxt;
                    break;
                case StatModif.Resilience:
                    ModifResilience.text = value;
                    ModifResilience.color = colorTxt;
                    ModifResilience.faceColor = colorTxt;
                    break;
                case StatModif.VolonterMax:
                    ModifVolonter.text = value;
                    ModifVolonter.color = colorTxt;
                    ModifVolonter.faceColor = colorTxt;
                    break;
                case StatModif.Vitesse:
                    ModifVitesse.text = value;
                    ModifVitesse.color = colorTxt;
                    ModifVitesse.faceColor = colorTxt;
                    break;
            }
                
        }
    }

    public void BuyCapa(Competence capa)
    {
        if (capa.Bought)
        {
            BuyButton.onClick.RemoveAllListeners();
            BuyButton.GetComponent<Image>().color = Color.gray;
            return;
        }
        Debug.Log("capa acheté");
        //if already bougth
        
        GameManager.instance.playerStat.Essence -= capa.EssenceCost;
        var capaClassSO = GameManager.instance.classSO.Competences.FirstOrDefault(x => x.IDLvl == capa.IDLvl);
        capaClassSO.Bought = true;
        capaClassSO.Equiped = true;
        capa.Bought = true;
        capa.Equiped = true;
         GameManager.instance.playerStat.ListSpell.Add(capa.Spell);
        SetStatBougthCapa(capa);
        CheckLinkCapa(capa);
        RetourButton.onClick.RemoveAllListeners();
        //RetourButton.onClick.AddListener(delegate{SceneManager.LoadScene("Monde")});
        RetourButton.onClick.AddListener(delegate { StartCoroutine(GameManager.instance.pmm.EndAutel(false)); });
    }

    public void SetStatBougthCapa(Competence capa)
    {
        JoueurStat modifJoueurStat = new JoueurStat();
        foreach (var modifStat in capa.ModifStat)
        {
            var value = modifStat.Valeur;
            switch (modifStat.StatModif)
            {
                case StatModif.Calme:
                    modifJoueurStat.Calme = value;
                    break;
                case StatModif.Clairvoyance:
                    modifJoueurStat.Clairvoyance = value;
                    break;
                case StatModif.ConscienceMax:
                    modifJoueurStat.ConscienceMax = value;
                    break;
                case StatModif.Conviction:
                    modifJoueurStat.Conviction = value;
                    break;
                case StatModif.ForceAme:
                    modifJoueurStat.ForceAme = value;
                    break;
                case StatModif.RadianceMax:
                    modifJoueurStat.RadianceMax = value;
                    modifJoueurStat.Radiance = value;
                    break;
                case StatModif.Resilience:
                    modifJoueurStat.Resilience = value; 
                    break;
                case StatModif.VolonterMax:
                    modifJoueurStat.VolonterMax = value;
                    break;
                case StatModif.Vitesse:
                    modifJoueurStat.Vitesse = value;
                    break;
            }
        }
        GameManager.instance.playerStat.ModifStateAll(modifJoueurStat);
    }

    public void CheckLinkCapa(Competence capa)
    {
        if (capa.IDLier == null)
            return;
        foreach (var id in capa.IDLier)
        {
            var capaLier = GameManager.instance.classSO.Competences.FirstOrDefault(x => x.IDLvl == id);
            if (capa.lvlCapa == 1)
            {
                capaLier.EssenceCost -= capaLier.EssenceCost / 100 * 25;
            }
            else if(capa.lvlCapa == 2)
            {
                capaLier.EssenceCost -= capaLier.EssenceCost / 100 * 50;
            }
            else
            {
                Debug.Log("lvl capa ni a 1 ni a 2");
            }
        }
    }

    public void RetourMap()
    {
        StartCoroutine(GameManager.instance.pmm.EndAutel(false));
    }




    public void GetSouvenirs()
    {

    }

    public void CoutChoix()
    {

    }
}