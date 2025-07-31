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
    [SerializeField]
    private GameObject _backgroundSky;
    public int Etage = 1;
    public PlayerStatsHandler stats;
    [SerializeField]
    private string _idTradCout;
    [SerializeField]
    private Animator _autelAnimator;
    [SerializeField]
    private Transform _autelArm;
    [SerializeField]
    private Transform _autelLeftPivot;
    [SerializeField]
    private Transform _autelRightPivot;

    [Header("Arbre")]
    public TextMeshProUGUI EssenceText;
    [SerializeField]
    private string _idTradEssence;
    [SerializeField]
    private GameObject _descriptionPanel;
    public TextMeshProUGUI DescriptionSpellText;
    [SerializeField]
    private GameObject _costPanel;
    public TextMeshProUGUI CostCapaText;
    [SerializeField]
    private GameObject _nbSpellPanel;
    [SerializeField]
    private TMP_Text _nbSpellText;

    public List<GameObject> AllSpellsIcon;
    public List<GameObject> AllLink;
    [SerializeField]
    private Color _selectedColor = Color.green;

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
    [SerializeField]
    private string _idTradNameCalmPoint;
    [SerializeField]
    private string _idTradNameRadPoint;
    [SerializeField]
    private string _idTradNameInsightPoint;
    public bool Loot = false;
    public GameObject ButtonChoix1, ButtonChoix2, ButtonChoix3, ButtonReturn;
    public TextMeshProUGUI TextCoutChoix1, TextCoutChoix2, TextCoutChoix3;
    [SerializeField]
    private string _idTradChoix3;
    [SerializeField]
    private float _indentChoice3;
    public TextMeshProUGUI TextDescriptionChoix3;
    public List<int> CoutChoix1, CoutChoix2, CoutChoix3, CoutStatChoix3;
    public List<LootRarity> LootRarityForChoix1;
    public GameObject SpawnSouvenirChoix3, SouvenirChoix3;
    public GameObject SouvenirPrefab;
    public List<Souvenir> listAllSouvenir;

    public bool isOn = false;
    [SerializeField]
    private GameObject _explicationPanelAutel;
    [SerializeField]
    private GameObject _explicationPanelShop;
    [SerializeField]
    private GameObject _explicationPanelLvlUp;
    private bool _isFirstVisitAutel = true;
    private bool _isFirstVisitShop = true;
    private bool _isFirstVisitLvlUp = true;
    private void OnEnable()
    {
        ResetPositionBalance();
        
        GameManager.OnStartAutel += InitAutel;
        TradManager.OnRefreshTranslation += RefreshTextChoice3;
    }
    private void OnDisable()
    {
        GameManager.OnStartAutel -= InitAutel;
        TradManager.OnRefreshTranslation -= RefreshTextChoice3;
    }

    void FixedUpdate()
    {
        if (!isOn)
            return;
        EssenceText.text = stats.Essence + GameManager.Instance.StatIcons.EssenceSpriteTMP;
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
        _nbSpellText.text = $"Spells {GameManager.Instance.playerStatHandler.ListSpell.Count}/12";
    }

    public void InitAutel()
    {
        stats = GameManager.Instance.playerStatHandler;
        RetourButton.onClick.RemoveAllListeners();
        //RetourButton.onClick.AddListener(delegate{SceneManager.LoadScene("Monde")});
        RetourButton.onClick.AddListener(delegate { ShowMenuUiPanel(); });
        _descriptionPanel.SetActive(false);
        _costPanel.SetActive(false);
        _nbSpellPanel.SetActive(false);
        SetUpStatsDescription();
        ShowMenuUiPanel();
        isOn = true;
    }
    public void ShowExplicationShopPanel()
    {
        _explicationPanelShop.SetActive(true);
    }
    public void HideExplicationShopPanel()
    {
        _explicationPanelShop.SetActive(false);
    }
    public void ShowExplicationAutelPanel()
    {
        _explicationPanelAutel.SetActive(true);
    }
    public void HideExplicationAutelPanel()
    {
        _explicationPanelAutel.SetActive(false);
    }
    public void ShowExplicationLevelUpPanel()
    {
        _explicationPanelLvlUp.SetActive(true);
    }
    public void HideExplicationLevelUpPanel()
    {
        _explicationPanelLvlUp.SetActive(false);
    }
    public void ShowMenuUiPanel()
    {
        if (_isFirstVisitAutel)
        {
            ShowExplicationAutelPanel();
            _isFirstVisitAutel = false;
        }
        else
        {
            HideExplicationAutelPanel();
        }
        EssenceText.color = Color.black;
        CostCapaText.text = "";
        DescriptionSpellText.text = string.Empty;
        emptyStat();
        ShopUiPanel.SetActive(false);
        LevelUpUiPanel.SetActive(false);
        MenuUiPanel.SetActive(true);
        _backgroundSky.SetActive(false);
        _descriptionPanel.SetActive(false);
        _costPanel.SetActive(false);
        _nbSpellPanel.SetActive(false);
    }
    private void ResetPositionBalance()
    {
        _autelArm.rotation = Quaternion.identity;
        _autelLeftPivot.rotation = Quaternion.identity;
        _autelRightPivot.rotation = Quaternion.identity;
    }
    public void SetShopActive()
    {
        if (_isFirstVisitShop)
        {
            ShowExplicationShopPanel();
            _isFirstVisitShop = false;
        }
        else
        {
            HideExplicationShopPanel();
        }
        //EssenceText.color = Color.white;
        BackHover();
        ResetPositionBalance();
        ShopUiPanel.SetActive(true);
        MenuUiPanel.SetActive(false);
        _backgroundSky.SetActive(true);
        SetUpShop();
    }

    public void SetLvlUpActive()
    {
        if (_isFirstVisitLvlUp)
        {
            ShowExplicationLevelUpPanel();
            _isFirstVisitLvlUp = false;
        }
        else
        {
            HideExplicationLevelUpPanel();
        }
        //EssenceText.color = Color.white;
        _nbSpellPanel.SetActive(true);
        BackHover();
        ResetPositionBalance();
        LevelUpUiPanel.SetActive(true);
        MenuUiPanel.SetActive(false);
        _backgroundSky.SetActive(true);
        SetUpAllSpells();
    }

    private void SetUpAllSpells()
    {
        var listOfCompetences = GameManager.Instance.classSO.Competences;

        foreach (var capa in listOfCompetences)
        {
            //if (ShouldIgnoreCapaLier(capa.IDLvl))
            //{
            //    AllSpellsIcon[capa.Spell.IDSpell].GetComponent<Image>().color = Color.gray;
            //    capa.isBuyable = false;
            //}
            //if (capa.IDLvl == 13 || capa.IDLvl == 14)
            //{
            //    AllSpellsIcon[capa.Spell.IDSpell].GetComponent<Image>().sprite = capa.Spell.Sprite;
            //    capa.isBuyable = true;
            //}
            if (capa.Spell?.Sprite)
            {
                AllSpellsIcon[capa.Spell.IDSpell].GetComponent<Image>().sprite = capa.Spell.RoundSprite;

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
        ClearButtonColor();
    }

    public void SetUpStatsDescription()
    {
        stats = GameManager.Instance.playerStatHandler;
        ValeurRadiance.text = stats.RadianceMax.ToString();
        ValeurFA.text = stats.ForceAme.ToString();
        ValeurVitesse.text = stats.Vitesse.ToString();
        ValeurConviction.text = stats.Conviction.ToString();
        ValeurResilience.text = stats.Resilience.ToString();
        ValeurCalme.text = stats.Calme.ToString();
        ValeurVolonter.text = stats.VolonteMax.ToString();
        ValeurConscience.text = stats.ConscienceMax.ToString();
        ValeurClairvoyance.text = stats.Clairvoyance.ToString();

        //ModifRadiance.text = ModifFA.text = ModifVitesse.text = ModifConviction.text = ModifResilience.text =
        //    ModifCalme.text = ModifVolonter.text = ModifConscience.text = ModifClairvoyance.text = "";
    }

    public void SelectSpell(int Id)
    {
        BuyButton.onClick.RemoveAllListeners();
        var listOfCompetences = GameManager.Instance.classSO.Competences;

        foreach (var capa in listOfCompetences)
        {
            if (capa.Spell?.IDSpell == Id)
            {
                string name = capa.Spell.TitleId.Text;
                string description = capa.Spell.DescriptionId.Text;
                _descriptionPanel.SetActive(true);
                _costPanel.SetActive(true);
                DescriptionSpellText.text = $"<allcaps><b> {name} </b></allcaps> \n{ description}";
                CostCapaText.text = TradManager.instance.GetTranslation(_idTradCout, "Cout") + " : " + capa.EssenceCost;
                ClearButtonColor();
                AllSpellsIcon[Id].GetComponent<Image>().color = _selectedColor;
                ModifStatCapa(capa);
                if (capa.EssenceCost <= GameManager.Instance.playerStatHandler.Essence && !capa.Bought && capa.isBuyable)
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
    private void ClearButtonColor()
    {
        foreach (var icon in AllSpellsIcon)
        {
            icon.GetComponent<Image>().color = Color.white;
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
        if (GameManager.Instance.playerStatHandler.ListSpell.Count > 11)
        {
            Debug.Log("Max capa buy");
            return;
        }
        Debug.Log("capa acheté");

        GameManager.Instance.playerStatHandler.Essence -= capa.EssenceCost;
        var capaClassSO = GameManager.Instance.classSO.Competences.FirstOrDefault(x => x.IDLvl == capa.IDLvl);
        capaClassSO.Bought = true;
        capaClassSO.Equiped = true;
        capa.Bought = true;
        capa.Equiped = true;
        GameManager.Instance.playerStatHandler.ListSpell.Add(capa.Spell);
        SetStatBougthCapa(capa);
        CheckLinkCapa(capa);

        var CheckMark = AllSpellsIcon[capa.Spell.IDSpell].transform.GetChild(0);
        CheckMark.gameObject.SetActive(true);

        RetourButton.onClick.RemoveAllListeners();
        //RetourButton.onClick.AddListener(delegate{SceneManager.LoadScene("Monde")});
        RetourButton.onClick.AddListener(delegate { RetourMap(); });

        BuyButton.onClick.RemoveAllListeners();
        BuyButton.GetComponent<Image>().color = Color.gray;
    }

    public void SetStatBougthCapa(Competence capa)
    {
        JoueurStat modifJoueurStat = ScriptableObject.CreateInstance<JoueurStat>();
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
        GameManager.Instance.BattleMan.player.Stat.UpdateStat(modifJoueurStat);
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
            var capaLier = GameManager.Instance.classSO.Competences.FirstOrDefault(x => x.IDLvl == id);
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
        if (Loot == true) GameManager.Instance.GamePanelMngr.HideAutel();
        GameManager.Instance.playerStatHandler.ListSouvenir = stats.ListSouvenir;
        StartCoroutine(GameManager.Instance.pmm.EndAutel(Loot));
    }




    public void SetUpShop()
    {
        GetSouvenirs();
        initChoix3();
    }

    public void GetSouvenirs()
    {
        listAllSouvenir = GameManager.Instance.CopyAllSouvenir.OrderBy(a => UnityEngine.Random.value).ToList();
    }

    public void CoutChoix(int Choix)
    {
        JoueurStat modifStat = ScriptableObject.CreateInstance<JoueurStat>();
        switch (Etage)
        {
            case 1:
                switch (Choix)
                {
                    case 1:
                        modifStat.Essence -= CoutChoix1[0];
                        break;
                    case 2:
                        modifStat.Essence -= CoutChoix2[0];
                        break;
                    case 3:
                        modifStat.Essence -= CoutChoix3[0];
                        modifStat.Calme -= CoutStatChoix3[0];
                        break;
                }
                break;
            case 2:
                switch (Choix)
                {
                    case 1:
                        modifStat.Essence -= CoutChoix1[1];
                        break;
                    case 2:
                        modifStat.Essence -= CoutChoix2[1];
                        break;
                    case 3:
                        modifStat.Essence -= CoutChoix3[1];
                        modifStat.RadianceMax -= CoutStatChoix3[1];
                        break;
                }
                break;
            case 3:
                switch (Choix)
                {
                    case 1:
                        modifStat.Essence -= CoutChoix1[2];
                        break;
                    case 2:
                        modifStat.Essence -= CoutChoix2[2];
                        break;
                    case 3:
                        modifStat.Essence -= CoutChoix3[2];
                        modifStat.Clairvoyance -= CoutStatChoix3[2];
                        break;
                }
                break;
        }
        stats.UpdateStat(modifStat);
    }

    public void initChoix3()
    {
        SouvenirChoix3 = Instantiate(SouvenirPrefab, SpawnSouvenirChoix3.transform);
        listAllSouvenir = listAllSouvenir.OrderBy(a => UnityEngine.Random.value).ToList();
        List<Souvenir> souvList = Etage switch
        {
            3 => GameManager.Instance.CopyAllSouvenir.Where(c => c.Rarete >= Rarity.Legendaire && c.IsClass).ToList(),
            2 => GameManager.Instance.CopyAllSouvenir.Where(c => c.Rarete >= Rarity.Mythique && c.IsClass).ToList(),
            1 => GameManager.Instance.CopyAllSouvenir.Where(c => c.Rarete >= Rarity.Rare && c.IsClass).ToList(),
            _ => GameManager.Instance.CopyAllSouvenir
        };

        int ind = Random.Range(0, souvList.Count);
        SouvenirChoix3.GetComponent<SouvenirUI>().LeSouvenir = Instantiate(souvList[ind]);
        SouvenirChoix3.GetComponent<SouvenirUI>().StartUp();

        RefreshTextChoice3();
    }

    private void RefreshTextChoice3()
    {
        if (SouvenirChoix3 == null) return;
        TextDescriptionChoix3.text =$"<line-indent={_indentChoice3}%>{TradManager.instance.GetTranslation(_idTradChoix3)} : {SouvenirChoix3.GetComponent<SouvenirUI>().LeSouvenir.SouvenirDesc}";
    }
    public void UpdateCoutChoix()
    {
        if (Etage <= 0 || Etage > CoutChoix1.Count || Etage > CoutChoix2.Count || Etage > CoutChoix3.Count)
        {
            Debug.LogError("Nombre d'étage trop faible ou trop important");
        }
        
        TextCoutChoix1.text = $"{CoutChoix1[Etage - 1]} {GameManager.Instance.StatIcons.EssenceSpriteTMP}";
        TextCoutChoix2.text = $"{CoutChoix2[Etage - 1]} {GameManager.Instance.StatIcons.EssenceSpriteTMP}";
        
        string spriteTMPEtage = Etage switch
        {
            3 => GameManager.Instance.StatIcons.StatClairvoyanceSpriteTMP,
            2 => GameManager.Instance.StatIcons.StatRadianceSpriteTMP,
            _ => GameManager.Instance.StatIcons.StatCalmeSpriteTMP,
        };

        TextCoutChoix3.text = $"{CoutChoix3[Etage - 1]} {GameManager.Instance.StatIcons.EssenceSpriteTMP}\n" +
                    $"{CoutStatChoix3[Etage - 1]} {spriteTMPEtage}";

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
                string NameLoot = listAllSouvenir.FirstOrDefault(c => c.Rarete == LootRarityForChoix1[i].rareter).SouvenirName;
                stats.ListSouvenir.Add(Instantiate(listAllSouvenir.FirstOrDefault(c => c.SouvenirName == NameLoot)));
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

        List<Souvenir> souvList = Etage switch
        {
            3 => GameManager.Instance.CopyAllSouvenir.Where(c => c.Rarete >= Rarity.Legendaire && c.IsClass).ToList(),
            2 => GameManager.Instance.CopyAllSouvenir.Where(c => c.Rarete >= Rarity.Mythique && c.IsClass).ToList(),
            1 => GameManager.Instance.CopyAllSouvenir.Where(c => c.Rarete >= Rarity.Rare && c.IsClass).ToList(),
            _ => GameManager.Instance.CopyAllSouvenir
        };

        int ind = Random.Range(0, souvList.Count);
        NameLoot = souvList[ind].SouvenirName;
        stats.ListSouvenir.Add(Instantiate(souvList[ind]));
     
        Loot = true;
        RetourMap();
    }

    public void Choix3()
    {
        CoutChoix(3);
        stats.Conscience += 3;
        stats.ListSouvenir.Add(SouvenirChoix3.GetComponent<SouvenirUI>().LeSouvenir);
        Loot = true;
        RetourMap();
    }

    #region Animation

    public void HoverShop()
    {
        _autelAnimator.SetTrigger("ChooseShop");
    }
    public void HoverTree()
    {
        _autelAnimator.SetTrigger("ChooseTree");
    }
    public void BackHover()
    {
        _autelAnimator.SetTrigger("Back");
    }

    #endregion
}