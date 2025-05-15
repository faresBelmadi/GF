using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Image = UnityEngine.UI.Image;
using static UnityEditor.Progress;

public class TreasureManager : MonoBehaviour
{
    public JoueurStat stats;
    public int ratioSouvenir;
    public int percentageSouvenirRare;
    public int percentageSouvenirEpique;
    public int percentageSouvenirLegendaire;
    //public int ratioEssence;
    public bool isOn = false;
    public GameObject SouvenirUiHolder;
    public GameObject SouvenirPrefab;
    public GameObject SouvenirAnchor;
    public GameObject EssenceUiHolder;
    public GameObject LootButton;
    public TextMeshProUGUI EssenceTexte;
    public int EssenceValue;
    public bool Loot;
    private List<Souvenir> souvenirRare;
    private List<Souvenir> souvenirMythique;
    private List<Souvenir> souvenirLegendaire;

    private void OnEnable()
    {
        souvenirRare = GameManager.Instance.CopyAllSouvenir.Where(x => x.Rarete == Rarity.Rare).ToList();
        souvenirMythique = GameManager.Instance.CopyAllSouvenir.Where(x => x.Rarete == Rarity.Mythique).ToList();
        souvenirLegendaire = GameManager.Instance.CopyAllSouvenir.Where(x => x.Rarete == Rarity.Legendaire).ToList();
        //GameManager.OnStartTreasure += InitTreasure;
    }

    private void OnDisable()
    {
        SouvenirUiHolder.SetActive(false);
        EssenceUiHolder.SetActive(false);
        //GameManager.OnStartTreasure -= InitTreasure;
    }

    //public void StartLoot()
    //{

    //}

    public void LootTreasure() // rajouter un boutton pour lancer le loot ?
    {
        Debug.Log(GameManager.Instance.playerStat.Essence);

        LootButton.SetActive(false);
        isOn = true;
        stats = GameManager.Instance.playerStat;
        var rand = Random.Range(0f, 100f);
        if (rand <= ratioSouvenir)
            SetUpLootSouvenir();
        else
            SetUpLootEssence();
    }

    public void SetUpLootSouvenir()
    {
        Loot = true;
        var lootSouvenir = RandSouvenir();
        Debug.Log("souvenir : " + lootSouvenir.SouvenirName);
        var SouvenirGO = Instantiate(SouvenirPrefab, SouvenirAnchor.transform);
        SouvenirGO.GetComponent<SouvenirUI>().LeSouvenir = lootSouvenir;
        SouvenirGO.GetComponent<SouvenirUI>().StartUp();
        stats.ListSouvenir.Add(lootSouvenir);
        SouvenirUiHolder.gameObject.SetActive(true);
    }

    private Souvenir RandSouvenir()
    {
        var rand = Random.Range(0, 100);
        Souvenir lootSouvenir = new Souvenir();
        if (rand <= percentageSouvenirRare)
            lootSouvenir = SelectSouvenir(souvenirRare);
        else if (rand <= percentageSouvenirEpique)
            lootSouvenir = SelectSouvenir(souvenirMythique);
        else
            lootSouvenir = SelectSouvenir(souvenirLegendaire);
        if (stats.ListSouvenir.Contains(lootSouvenir))
            RandSouvenir();
        return lootSouvenir;
    }

    private Souvenir SelectSouvenir(List<Souvenir> souvenirs)
    {
        var rand = Random.Range(0, souvenirs.Count);
        return souvenirs[rand];
    }

    public void SetUpLootEssence()
    {
        Loot = false;
        EssenceTexte.text = EssenceValue + " essences";
        stats.Essence += EssenceValue;
        EssenceUiHolder.gameObject.SetActive(true);
    }

    public void Exit()
    {
        //SouvenirUiHolder.gameObject.SetActive(false);
        //EssenceUiHolder.gameObject.SetActive(false);
        GameManager.Instance.GamePanelMngr.HideTreasure();
        GameManager.Instance.playerStat = stats;
        Debug.Log(GameManager.Instance.playerStat.Essence);
        LootButton.SetActive(true);
        StartCoroutine(GameManager.Instance.pmm.EndTreasure(Loot));
    }
}