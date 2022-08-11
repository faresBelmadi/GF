using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

//TODO : Remettre les lignes en commentaire qui retire la possibilité de loot le souvneir loot
public class AutelManager : MonoBehaviour
{
    public bool Use = false;
    public bool Loot = false;
    public JoueurStat Stat;
    public TextMeshProUGUI EssenceText;
    public int CoutLvlUp = 1000;
    public int Etage = 1;

    public GameObject ButtonLvlup, ButtonItem;
    public GameObject ButtonChoix1, ButtonChoix2, ButtonChoix3, ButtonReturn;
    public TextMeshProUGUI TextCoutChoix1, TextCoutChoix2, TextCoutChoix3, TextCoutLvlUp;
    public int CoutChoix1, CoutChoix2, CoutChoix3;
    public List<LootRarity> LootRarityForChoix1;
    public GameObject SpawnSouvenirChoix3, SouvenirChoix3;
    public GameObject SouvenirPrefab;

    #region Start & End

    public void StartAutel()
    {
        Stat = GameManager.instance.playerStat;
        InstantiateSouvenirChoix3();
        UpdateCoutChoix();
    }

    public void InstantiateSouvenirChoix3()
    {
        SouvenirChoix3 = Instantiate(SouvenirPrefab, SpawnSouvenirChoix3.transform);
        switch (Etage)
        {
            case 1:
                SouvenirChoix3.GetComponent<SouvenirUI>().LeSouvenir = Instantiate(GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Rare));
                SouvenirChoix3.GetComponent<SouvenirUI>().StartUp();
                break;
            case 2:
                SouvenirChoix3.GetComponent<SouvenirUI>().LeSouvenir = Instantiate(GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Mythique));
                SouvenirChoix3.GetComponent<SouvenirUI>().StartUp();
                break;
            case 3:
                SouvenirChoix3.GetComponent<SouvenirUI>().LeSouvenir = Instantiate(GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Legendaire));
                SouvenirChoix3.GetComponent<SouvenirUI>().StartUp();
                break;
        }
        
    }

    public void EndAutel()
    {
        GameManager.instance.playerStat = Stat;
        StartCoroutine(GameManager.instance.pmm.EndAutel(Use, Loot));
    }

    #endregion Start & End

    #region Update

    public void UpdateCoutChoix()
    {
        TextCoutLvlUp.text = "Cout : " + CoutLvlUp.ToString() + " essence";
        CoutChoix1 = 0;
        TextCoutChoix1.text = "Cout : 0 essence";
        CoutChoix2 = Mathf.FloorToInt(CoutLvlUp / 3);
        TextCoutChoix2.text = "Cout : " + CoutChoix2.ToString() + " essence";
        CoutChoix3 = Mathf.FloorToInt(CoutLvlUp / 4);
        switch (Etage)
        {
            case 1:
                TextCoutChoix3.text = "Cout : " + CoutChoix3.ToString() + " essence\n1 point de Calme";
                break;
            case 2:
                TextCoutChoix3.text = "Cout : " + CoutChoix3.ToString() + " essence\n100 point de Radiance";
                break;
            case 3:
                TextCoutChoix3.text = "Cout : " + CoutChoix3.ToString() + " essence\n10 point de Clairvoyance";
                break;
        }

    }

    public void Update()
    {
        EssenceText.text = "Essence : " + Stat.Essence;
        if (Stat.Essence < CoutLvlUp)
        {
            ButtonLvlup.GetComponentInChildren<Button>().interactable = false;
        }
        if (Stat.Essence < CoutChoix1)
        {
            ButtonChoix1.GetComponentInChildren<Button>().interactable = false;
        }
        if (Stat.Essence < CoutChoix2)
        {
            ButtonChoix2.GetComponentInChildren<Button>().interactable = false;
        }
        if (Stat.Essence < CoutChoix3)
        {
            ButtonChoix3.GetComponentInChildren<Button>().interactable = false;
        }
    }

    #endregion Update

    #region LvlUp

    public void Lvlup()
    {
        Stat.Essence -= CoutLvlUp;
        Stat.Lvl += 1;
        Use = true;
        ButtonItem.GetComponent<Button>().interactable = false;
    }

    #endregion LvlUp

    #region Item

    public void Item()
    {
        ButtonLvlup.SetActive(false);
        ButtonItem.SetActive(false);
        ButtonChoix1.SetActive(true);
        ButtonChoix2.SetActive(true);
        ButtonChoix3.SetActive(true);
        ButtonReturn.SetActive(true);
    }

    public void Return()
    {
        ButtonLvlup.SetActive(true);
        ButtonItem.SetActive(true);
        ButtonChoix1.SetActive(false);
        ButtonChoix2.SetActive(false);
        ButtonChoix3.SetActive(false);
        ButtonReturn.SetActive(false);
    }

    #endregion Item

    #region Les Choix

    public void Choix1()
    {
        Stat.Essence -= CoutChoix1;
        Stat.Conscience += 2;
        int random = UnityEngine.Random.Range(0, 101);
        Debug.Log("Loot : " + random);
        if (random > 51)
        {
            EndAutel();
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
            if (random <= LootRarityForChoix1[i].Pourcentage && GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Rarete == LootRarityForChoix1[i].rareter) != null)
            {
                string NameLoot = GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Rarete == LootRarityForChoix1[i].rareter).Nom;
                Stat.ListSouvenir.Add(Instantiate(GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot)));
                //GameManager.instance.CopyAllSouvenir.Remove(GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot));
                Use = true;
                Loot = true;
                EndAutel();
                return;
            }
            else
            {
                random -= LootRarityForChoix1[i].Pourcentage;
            }
        }
        EndAutel();
    }

    public void Choix2()
    {
        Stat.Essence -= CoutChoix2;
        Stat.Conscience += 3;
        string NameLoot;
        switch (Etage)
        {
            case 1:
                NameLoot = GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Rare).Nom;
                Stat.ListSouvenir.Add(Instantiate(GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot)));
                //GameManager.instance.CopyAllSouvenir.Remove(GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot));
                break;
            case 2:
                NameLoot = GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Rare).Nom;
                Stat.ListSouvenir.Add(Instantiate(GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot)));
                //GameManager.instance.CopyAllSouvenir.Remove(GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot));
                break;
            case 3:
                NameLoot = GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Mythique).Nom;
                Stat.ListSouvenir.Add(Instantiate(GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot)));
                //GameManager.instance.CopyAllSouvenir.Remove(GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot));
                break;
        }
        Use = true;
        Loot = true;
        EndAutel();
    }

    public void Choix3()
    {
        Stat.Essence -= CoutChoix3;
        switch (Etage)
        {
            case 1:
                Stat.Calme -= 1;
                break;
            case 2:
                Stat.Radiance -= 100;
                break;
            case 3:
                Stat.Clairvoyance -= 10;
                break;
        }
        Stat.Conscience += 3;
        Stat.ListSouvenir.Add(SouvenirChoix3.GetComponent<SouvenirUI>().LeSouvenir);
        //GameManager.instance.CopyAllSouvenir.Remove(GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Nom == SouvenirChoix3.GetComponent<SouvenirUI>().LeSouvenir.Nom));
        Use = true;
        Loot = true;
        EndAutel();
    }

    #endregion Les Choix
}
