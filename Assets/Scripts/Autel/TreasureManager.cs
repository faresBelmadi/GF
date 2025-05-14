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
    //public int ratioEssence;
    public bool isOn = false;
    public GameObject SouvenirUiHolder;
    public GameObject SouvenirPrefab;
    public GameObject SouvenirAnchor;
    public GameObject EssenceUiHolder;
    public TextMeshProUGUI EssenceTexte;
    public int EssenceValue;
    public bool Loot;

    private void OnEnable()
    {
        GameManager.OnStartTreasure += InitTreasure;
    }

    private void OnDisable()
    {
        SouvenirUiHolder.SetActive(false);
        EssenceUiHolder.SetActive(false);
        GameManager.OnStartTreasure -= InitTreasure;
    }
    
    public void InitTreasure() // rajouter un boutton pour lancer le loot ?
    {
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
        var rand = Random.Range(0, GameManager.Instance.CopyAllSouvenir.Count);
        var lootSouvenir = GameManager.Instance.CopyAllSouvenir[rand];
        Debug.Log("souvenir : " + lootSouvenir.SouvenirName);
        var SouvenirGO = Instantiate(SouvenirPrefab, SouvenirAnchor.transform);
        SouvenirGO.GetComponent<SouvenirUI>().LeSouvenir = lootSouvenir;
        SouvenirGO.GetComponent<SouvenirUI>().StartUp();
        stats.ListSouvenir.Add(lootSouvenir);
        SouvenirUiHolder.gameObject.SetActive(true);
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
        StartCoroutine(GameManager.Instance.pmm.EndTreasure(Loot));
    }
}