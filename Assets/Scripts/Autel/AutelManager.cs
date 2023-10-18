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
    public GameObject ShopUiPanel;
    public int Etage = 1;
    public JoueurStat stats;

    [Header("Arbre")]
    public TextMeshProUGUI EssenceText;
    public TextMeshProUGUI DescriptionSpellText;
    public TextMeshProUGUI CostCapaText;

    public List<GameObject> AllSpellsIcon;
    public List<GameObject> AllLink;

    public Button BuyButton;

    public Button RetourButton;

    public TextMeshProUGUI ValeurRadiance,
        ValeurFA,
        ValeurVitesse,
        ValeurConviction,
        ValeurResilience,
        ValeurCalme,
        ValeurVolonter,
        ValeurConscience,
        ValeurClairvoyance;

    public TextMeshProUGUI ModifRadiance,
        ModifFA,
        ModifVitesse,
        ModifConviction,
        ModifResilience,
        ModifCalme,
        ModifVolonter,
        ModifConscience,
        ModifClairvoyance;

    public Sprite checkMark;


    [Header("Shop")]
    public GameObject ButtonChoix1, ButtonChoix2, ButtonChoix3, ButtonReturn;
    public List<int> CoutChoix1, CoutChoix2, CoutChoix3, CoutStatChoix3;
    public List<LootRarity> LootRarityForChoix1;
    public GameObject SpawnSouvenirChoix3, SouvenirChoix3;
    public GameObject SouvenirPrefab;
    public List<Souvenir> listAllSouvenir;


    void FixedUpdate()
    {
        EssenceText.text = "Essence : " + GameManager.instance.playerStat.Essence;
        SetUpStatsDescription();
    }

    public void ShowMenuUiPanel()
    {
        ShopUiPanel.SetActive(false);
        LevelUpUiPanel.SetActive(false);
        MenuUiPanel.SetActive(true);
    }

    public void SetShopActive()
    {
        ShopUiPanel.SetActive(true);
        MenuUiPanel.SetActive(false);
        SetUpShop();
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
            if (!capa.isBuyable)
                AllSpellsIcon[capa.Spell.IDSpell].GetComponent<Image>().color = Color.gray;
            //spellIcon.gameObject.GetComponent<Image>().sprite = GameManager.instance.playerStat.ListSpell[0].Sprite;
        }

        SetUpStatsDescription();
    }

    public void SetUpStatsDescription()
    {
        stats = GameManager.instance.playerStat;
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

                DescriptionSpellText.text = $"<allcaps><b> {capa.Spell.Nom} </b></allcaps> \n" + capa.Spell.Description;
                CostCapaText.text = "cout : " + capa.EssenceCost;
                ModifStatCapa(capa);
                if (capa.EssenceCost <= GameManager.instance.playerStat.Essence && !capa.Bought && capa.isBuyable)
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
        Debug.Log("capa acheté");

        GameManager.instance.playerStat.Essence -= capa.EssenceCost;
        var capaClassSO = GameManager.instance.classSO.Competences.FirstOrDefault(x => x.IDLvl == capa.IDLvl);
        capaClassSO.Bought = true;
        capaClassSO.Equiped = true;
        capa.Bought = true;
        capa.Equiped = true;
        GameManager.instance.playerStat.ListSpell.Add(capa.Spell);
        SetStatBougthCapa(capa);
        CheckLinkCapa(capa);

        var test = AllSpellsIcon[capa.Spell.IDSpell].GetComponentsInChildren<Image>();
        test[1].sprite = checkMark;
        test[1].rectTransform.localScale = new Vector3(1,1,1);

        RetourButton.onClick.RemoveAllListeners();
        //RetourButton.onClick.AddListener(delegate{SceneManager.LoadScene("Monde")});
        RetourButton.onClick.AddListener(delegate { StartCoroutine(GameManager.instance.pmm.EndAutel(false)); });

        BuyButton.onClick.RemoveAllListeners();
        BuyButton.GetComponent<Image>().color = Color.gray;
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
            if (capaLier != null)
            {
                if (capa.lvlCapa == 1)
                {
                    capaLier.EssenceCost -= capaLier.EssenceCost / 100 * 25;
                }
                else if (capa.lvlCapa == 2)
                {
                    capaLier.EssenceCost -= capaLier.EssenceCost / 100 * 50;
                }
                else
                {
                    Debug.Log("lvl capa ni a 1 ni a 2");
                }

                capaLier.isBuyable = true;
                AllSpellsIcon[capaLier.Spell.IDSpell].GetComponent<Image>().color = Color.white;
            }
        }
    }

    public void RetourMap()
    {
        StartCoroutine(GameManager.instance.pmm.EndAutel(false));
    }




    public void SetUpShop()
    {
        SetUpStatsDescription();
    }

    public void GetSouvenirs()
    {
        listAllSouvenir = GameManager.instance.CopyAllSouvenir;
    }

    public void CoutChoix(int Choix)
    {
        switch (Etage)
        {
            case 1:
                switch (Choix)
                {
                    case 1:
                        stats.Essence -= CoutChoix1[0];
                        break;
                    case 2:
                        stats.Essence -= CoutChoix2[0];
                        break;
                    case 3:
                        stats.Essence -= CoutChoix3[0];
                        stats.Calme -= CoutStatChoix3[0];
                        break;
                }
                break;
            case 2:
                switch (Choix)
                {
                    case 1:
                        stats.Essence -= CoutChoix1[1];
                        break;
                    case 2:
                        stats.Essence -= CoutChoix2[1];
                        break;
                    case 3:
                        stats.Essence -= CoutChoix3[1];
                        stats.Radiance -= CoutStatChoix3[1];
                        break;
                }
                break;
            case 3:
                switch (Choix)
                {
                    case 1:
                        stats.Essence -= CoutChoix1[2];
                        break;
                    case 2:
                        stats.Essence -= CoutChoix2[2];
                        break;
                    case 3:
                        stats.Essence -= CoutChoix3[2];
                        stats.Clairvoyance -= CoutStatChoix3[2];
                        break;
                }
                break;
        }
    }

    public void initChoix3()
    {
        SouvenirChoix3 = Instantiate(SouvenirPrefab, SpawnSouvenirChoix3.transform);
        switch (Etage)
        {
            case 1:
                if(listAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Rare) == null)
                {
                    SouvenirChoix3.GetComponent<SouvenirUI>().LeSouvenir = Instantiate(listAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Mythique));
                }
                else
                {
                    SouvenirChoix3.GetComponent<SouvenirUI>().LeSouvenir = Instantiate(listAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Rare));
                }
                SouvenirChoix3.GetComponent<SouvenirUI>().StartUp();
                break;
            case 2:
                if (listAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Mythique) == null)
                {
                    SouvenirChoix3.GetComponent<SouvenirUI>().LeSouvenir = Instantiate(listAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Rare));
                }
                else
                {
                    SouvenirChoix3.GetComponent<SouvenirUI>().LeSouvenir = Instantiate(listAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Mythique));
                }
                SouvenirChoix3.GetComponent<SouvenirUI>().StartUp();
                break;
            case 3:
                if (listAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Legendaire) == null)
                {
                    SouvenirChoix3.GetComponent<SouvenirUI>().LeSouvenir = Instantiate(listAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Mythique));
                }
                else
                {
                    SouvenirChoix3.GetComponent<SouvenirUI>().LeSouvenir = Instantiate(listAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Legendaire));
                }
                SouvenirChoix3.GetComponent<SouvenirUI>().StartUp();
                break;
        }
    }
}