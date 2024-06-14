using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Image = UnityEngine.UI.Image;

public class AutelManager : MonoBehaviour
{
    public GameObject MenuUiPanel;
    public GameObject LevelUpUiPanel;
    public GameObject ShopUiPanel;
    public GameObject StatUiPanel;
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
    public bool Loot = false;
    public GameObject ButtonChoix1, ButtonChoix2, ButtonChoix3, ButtonReturn;
    public TextMeshProUGUI TextCoutChoix1, TextCoutChoix2, TextCoutChoix3;
    public TextMeshProUGUI TextDescriptionChoix3;
    public List<int> CoutChoix1, CoutChoix2, CoutChoix3, CoutStatChoix3;
    public List<LootRarity> LootRarityForChoix1;
    public GameObject SpawnSouvenirChoix3, SouvenirChoix3;
    public GameObject SouvenirPrefab;
    public List<Souvenir> listAllSouvenir;

    private void Start()
    {
        stats = GameManager.instance.playerStat;

        SetUpStatsDescription();
    }
    void FixedUpdate()
    {
        EssenceText.text = "Essence : " + stats.Essence;
        SetUpStatsDescription();
        if (ShopUiPanel.activeInHierarchy == true)
        {
            UpdateCoutChoix();
            if (stats.Essence < CoutChoix1[Etage - 1])
            {
                ButtonChoix1.GetComponentInChildren<Button>().interactable = false;
            }
            if (stats.Essence < CoutChoix2[Etage - 1])
            {
                ButtonChoix2.GetComponentInChildren<Button>().interactable = false;
            }
            if (stats.Essence < CoutChoix3[Etage - 1])
            {
                ButtonChoix3.GetComponentInChildren<Button>().interactable = false;
            }
            switch (Etage)
            {
                case 1:
                    if (stats.Calme < CoutStatChoix3[Etage - 1])
                    {
                        ButtonChoix3.GetComponentInChildren<Button>().interactable = false;
                    }
                    break;
                case 2:
                    if (stats.Radiance < CoutStatChoix3[Etage - 1])
                    {
                        ButtonChoix3.GetComponentInChildren<Button>().interactable = false;
                    }
                    break;
                case 3:
                    if (stats.Clairvoyance < CoutStatChoix3[Etage - 1])
                    {
                        ButtonChoix3.GetComponentInChildren<Button>().interactable = false;
                    }
                    break;
            }

        }
    }

    public void ShowMenuUiPanel()
    {
        DescriptionSpellText.text = string.Empty;
        emptyStat();
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
            if (ShouldIgnoreCapaLier(capa.IDLvl))
            {
                AllSpellsIcon[capa.Spell.IDSpell].GetComponent<Image>().color = Color.gray;
                capa.isBuyable = false;
            }
            //if (capa.IDLvl == 13 || capa.IDLvl == 14)
            //{
            //    AllSpellsIcon[capa.Spell.IDSpell].GetComponent<Image>().sprite = capa.Spell.Sprite;
            //    capa.isBuyable = true;
            //}
            if (capa.Spell?.Sprite)
            {
                AllSpellsIcon[capa.Spell.IDSpell].GetComponent<Image>().sprite = capa.Spell.Sprite;

                //DEMO ONLY
                capa.isBuyable = true;
            }

            if (capa.Bought)
            {
                var CheckMark = AllSpellsIcon[capa.Spell.IDSpell].transform.GetChild(0);
                CheckMark.gameObject.SetActive(true);
            }
            //if (!capa.isBuyable)
            //    AllSpellsIcon[capa.Spell.IDSpell].GetComponent<Image>().color = Color.gray;

        }

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
            colorTxt = modifStat.Valeur < 0 ? new Color(0.71f,0.11f,0.02f) : new Color(0.28f, 0.55f, 0.17f);

            //vert : 071,139,42 rouge : 180,29,4
            switch (modifStat.StatModif)
            {
                case StatModif.Calme:
                    ModifCalme.text = value;
                    colorTxt = modifStat.Valeur > 0 ? new Color(0.71f, 0.11f, 0.02f) : new Color(0.28f, 0.55f, 0.17f);
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
    public void emptyStat()
    {
        ModifCalme.text = string.Empty;
        ModifClairvoyance.text = string.Empty;
        ModifConscience.text = string.Empty;
        ModifConviction.text = string.Empty;
        ModifFA.text = string.Empty;
        ModifRadiance.text = string.Empty;
        ModifResilience.text = string.Empty;
        ModifVolonter.text = string.Empty;
        ModifVitesse.text = string.Empty;
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

        var CheckMark = AllSpellsIcon[capa.Spell.IDSpell].transform.GetChild(0);
        CheckMark.gameObject.SetActive(true);

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
        //if (capa.IDLvl == 15)
        //{
        //    capa.IDLier.Add(14);
        //    capa.IDLier.Add(13);
        //}
        foreach (var id in capa.IDLier)
        {
            var capaLier = GameManager.instance.classSO.Competences.FirstOrDefault(x => x.IDLvl == id);
            if (capaLier != null && !ShouldIgnoreCapaLier(capaLier.IDLvl))
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

    private bool ShouldIgnoreCapaLier(int id)
    {
        if (id == 16 || id == 9 || id == 5 || id == 7 || id == 8)
            return true;
        return false;
    }

    public void RetourMap()
    {
        GameManager.instance.playerStat.ListSouvenir = stats.ListSouvenir;
        StartCoroutine(GameManager.instance.pmm.EndAutel(Loot));
    }




    public void SetUpShop()
    {
        GetSouvenirs();
        initChoix3();
    }

    public void GetSouvenirs()
    {
        listAllSouvenir = GameManager.instance.CopyAllSouvenir.OrderBy(a => UnityEngine.Random.value).ToList();
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
                        stats.RadianceMax -= CoutStatChoix3[1];
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
        listAllSouvenir = listAllSouvenir.OrderBy(a => UnityEngine.Random.value).ToList();
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
        TextDescriptionChoix3.text += SouvenirChoix3.GetComponent<SouvenirUI>().LeSouvenir.Description;
    }

    public void UpdateCoutChoix()
    {
        switch (Etage)
        {
            case 1:
                TextCoutChoix1.text = "Cout : " + CoutChoix1[0].ToString() + " essence";
                TextCoutChoix2.text = "Cout : " + CoutChoix2[0].ToString() + " essence";
                TextCoutChoix3.text = "Cout : " + CoutChoix3[0].ToString() + " essence\n" + CoutStatChoix3[0].ToString() + " point de Calme";
                break;
            case 2:
                TextCoutChoix1.text = "Cout : " + CoutChoix1[1].ToString() + " essence";
                TextCoutChoix2.text = "Cout : " + CoutChoix2[1].ToString() + " essence";
                TextCoutChoix3.text = "Cout : " + CoutChoix3[1].ToString() + " essence\n" + CoutStatChoix3[1].ToString() + " point de Radiance";
                break;
            case 3:
                TextCoutChoix1.text = "Cout : " + CoutChoix1[2].ToString() + " essence";
                TextCoutChoix2.text = "Cout : " + CoutChoix2[2].ToString() + " essence";
                TextCoutChoix3.text = "Cout : " + CoutChoix3[2].ToString() + " essence\n" + CoutStatChoix3[2].ToString() + " point de Clairvoyance";
                break;
        }
    }

    public void Choix1()
    {
        CoutChoix(1);
        stats.Conscience += 2;
        int random = UnityEngine.Random.Range(0, 101);
        Debug.Log("Loot : " + random);
        if (random > 51)
        {
            RetourMap();
            return;
        }
        LootRarityForChoix1.Sort((x, y) => x.Pourcentage.CompareTo(y.Pourcentage));
        int PourcentageTotal = 0;
        for (int i = 0; i < LootRarityForChoix1.Count; i++)
        {
            PourcentageTotal += LootRarityForChoix1[i].Pourcentage;
        }
        random = UnityEngine.Random.Range(0, PourcentageTotal + 1);
        Debug.Log("Rarity : " + random);
        for (int i = 0; i < LootRarityForChoix1.Count; i++)
        {
            if (random <= LootRarityForChoix1[i].Pourcentage && listAllSouvenir.FirstOrDefault(c => c.Rarete == LootRarityForChoix1[i].rareter) != null)
            {
                string NameLoot = listAllSouvenir.FirstOrDefault(c => c.Rarete == LootRarityForChoix1[i].rareter).Nom;
                stats.ListSouvenir.Add(Instantiate(listAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot)));
                //listAllSouvenir.Remove(listAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot));
                Loot = true;
                RetourMap();
                return;
            }
            else
            {
                random -= LootRarityForChoix1[i].Pourcentage;
            }
        }
        RetourMap();
    }

    public void Choix2()
    {
        CoutChoix(2);
        stats.Conscience += 3;
        string NameLoot;
        switch (Etage)
        {
            case 1:
                if (listAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Rare) == null)
                {
                    NameLoot = listAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Mythique).Nom;
                    stats.ListSouvenir.Add(Instantiate(listAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot)));
                    //listAllSouvenir.Remove(listAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot));
                }
                else
                {
                    NameLoot = listAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Rare).Nom;
                    stats.ListSouvenir.Add(Instantiate(listAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot)));
                    //listAllSouvenir.Remove(listAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot));
                }
                break;
            case 2:
                if (listAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Rare) == null)
                {
                    NameLoot = listAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Mythique).Nom;
                    stats.ListSouvenir.Add(Instantiate(listAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot)));
                    //listAllSouvenir.Remove(listAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot));
                }
                else
                {
                    NameLoot = listAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Rare).Nom;
                    stats.ListSouvenir.Add(Instantiate(listAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot)));
                    //listAllSouvenir.Remove(listAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot));
                }
                break;
            case 3:
                if (listAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Mythique) == null)
                {
                    NameLoot = listAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Legendaire).Nom;
                    stats.ListSouvenir.Add(Instantiate(listAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot)));
                    //listAllSouvenir.Remove(listAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot));
                }
                else
                {
                    NameLoot = listAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Mythique).Nom;
                    stats.ListSouvenir.Add(Instantiate(listAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot)));
                    //listAllSouvenir.Remove(listAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot));
                }
                break;
        }
        Loot = true;
        RetourMap();
    }

    public void Choix3()
    {
        CoutChoix(3);
        stats.Conscience += 3;
        stats.ListSouvenir.Add(SouvenirChoix3.GetComponent<SouvenirUI>().LeSouvenir);
        //listAllSouvenir.Remove(listAllSouvenir.FirstOrDefault(c => c.Nom == SouvenirChoix3.GetComponent<SouvenirUI>().LeSouvenir.Nom));
        Loot = true;
        RetourMap();
    }
}