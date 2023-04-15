using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

//TODO : Remettre les lignes en commentaire qui retire la possibilité de loot le souvneir loot
public class AutelManager : MonoBehaviour
{
    public bool Loot = false;
    public JoueurStat Stat;
    public TextMeshProUGUI EssenceText;
    public int Etage = 1;

    public GameObject ButtonLvlup, ButtonItem;
    public GameObject ButtonChoix1, ButtonChoix2, ButtonChoix3, ButtonReturn;
    public TextMeshProUGUI TextCoutChoix1, TextCoutChoix2, TextCoutChoix3;
    public List<int> CoutChoix1, CoutChoix2, CoutChoix3, CoutStatChoix3;
    public List<LootRarity> LootRarityForChoix1;
    public GameObject SpawnSouvenirChoix3, SouvenirChoix3;
    public GameObject SouvenirPrefab;

    public GameObject RetourButton;

    public GameObject ArbreCompetencePrefab;
    public GameObject ArbreCompetence, Canvas, AutelMenu;

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
                if(GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Rare) == null)
                {
                    SouvenirChoix3.GetComponent<SouvenirUI>().LeSouvenir = Instantiate(GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Mythique));
                }
                else
                {
                    SouvenirChoix3.GetComponent<SouvenirUI>().LeSouvenir = Instantiate(GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Rare));
                }
                SouvenirChoix3.GetComponent<SouvenirUI>().StartUp();
                break;
            case 2:
                if (GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Mythique) == null)
                {
                    SouvenirChoix3.GetComponent<SouvenirUI>().LeSouvenir = Instantiate(GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Rare));
                }
                else
                {
                    SouvenirChoix3.GetComponent<SouvenirUI>().LeSouvenir = Instantiate(GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Mythique));
                }
                SouvenirChoix3.GetComponent<SouvenirUI>().StartUp();
                break;
            case 3:
                if (GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Legendaire) == null)
                {
                    SouvenirChoix3.GetComponent<SouvenirUI>().LeSouvenir = Instantiate(GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Mythique));
                }
                else
                {
                    SouvenirChoix3.GetComponent<SouvenirUI>().LeSouvenir = Instantiate(GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Legendaire));
                }
                SouvenirChoix3.GetComponent<SouvenirUI>().StartUp();
                break;
        }
    }

    public void EndAutel()
    {
        GameManager.instance.playerStat = Stat;
        StartCoroutine(GameManager.instance.pmm.EndAutel(Loot));
    }

    #endregion Start & End

    #region Update

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

    public void Update()
    {
        UpdateCoutChoix();
        EssenceText.text = "Essence : " + Stat.Essence;
        if (Stat.Essence < CoutChoix1[Etage-1])
        {
            ButtonChoix1.GetComponentInChildren<Button>().interactable = false;
        }
        if (Stat.Essence < CoutChoix2[Etage-1])
        {
            ButtonChoix2.GetComponentInChildren<Button>().interactable = false;
        }
        if (Stat.Essence < CoutChoix3[Etage-1])
        {
            ButtonChoix3.GetComponentInChildren<Button>().interactable = false;
        }
        switch (Etage)
        {
            case 1:
                if (Stat.Calme < CoutStatChoix3[Etage - 1])
                {
                    ButtonChoix3.GetComponentInChildren<Button>().interactable = false;
                }
                break;
            case 2:
                if (Stat.Radiance < CoutStatChoix3[Etage - 1])
                {
                    ButtonChoix3.GetComponentInChildren<Button>().interactable = false;
                }
                break;
            case 3:
                if (Stat.Clairvoyance < CoutStatChoix3[Etage - 1])
                {
                    ButtonChoix3.GetComponentInChildren<Button>().interactable = false;
                }
                break;
        }
    }

    #endregion Update

    #region LvlUp

    public void Lvlup()
    {
        //Set Active prefab arbre de compétence

        ArbreCompetencePrefab.SetActive(true);
        //ArbreCompetence = Instantiate(ArbreCompetencePrefab, Canvas.transform);
        //ArbreCompetence.GetComponentInChildren<RetourArbre>().gameObject.GetComponent<Button>().onClick.AddListener(NonAfficherArbre);
        //ArbreCompetence.GetComponent<ArbreManager>().StartArbre(Stat);
        AutelMenu.SetActive(false);
    }

    public void NonAfficherArbre()
    {
        Stat = ArbreCompetence.GetComponent<ArbreManager>().Stat;
        ArbreCompetence.GetComponent<ArbreManager>().ReordableSpell();
        Stat.ListSpell.Clear();
        Stat.ListSpell = ArbreCompetence.GetComponent<ArbreManager>().SpellEquiped;
        GameManager.instance.classSO.Competences = ArbreCompetence.GetComponent<ArbreManager>().Class;
        AutelMenu.SetActive(true);
        Destroy(ArbreCompetence);
        EndAutel();
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
        ArbreCompetencePrefab.SetActive(false);
        AutelMenu.SetActive(true);
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
        CoutChoix(1);
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
        CoutChoix(2);
        Stat.Conscience += 3;
        string NameLoot;
        switch (Etage)
        {
            case 1:
                if(GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Rare) == null)
                {
                    NameLoot = GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Mythique).Nom;
                    Stat.ListSouvenir.Add(Instantiate(GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot)));
                    //GameManager.instance.CopyAllSouvenir.Remove(GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot));
                }
                else
                {
                    NameLoot = GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Rare).Nom;
                    Stat.ListSouvenir.Add(Instantiate(GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot)));
                    //GameManager.instance.CopyAllSouvenir.Remove(GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot));
                }
                break;
            case 2:
                if (GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Rare) == null)
                {
                    NameLoot = GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Mythique).Nom;
                    Stat.ListSouvenir.Add(Instantiate(GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot)));
                    //GameManager.instance.CopyAllSouvenir.Remove(GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot));
                }
                else
                {
                    NameLoot = GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Rare).Nom;
                    Stat.ListSouvenir.Add(Instantiate(GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot)));
                    //GameManager.instance.CopyAllSouvenir.Remove(GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot));
                }
                break;
            case 3:
                if (GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Mythique) == null)
                {
                    NameLoot = GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Legendaire).Nom;
                    Stat.ListSouvenir.Add(Instantiate(GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot)));
                    //GameManager.instance.CopyAllSouvenir.Remove(GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot));
                }
                else
                {
                    NameLoot = GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Rarete == Rarity.Mythique).Nom;
                    Stat.ListSouvenir.Add(Instantiate(GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot)));
                    //GameManager.instance.CopyAllSouvenir.Remove(GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Nom == NameLoot));
                }
                break;
        }
        Loot = true;
        EndAutel();
    }

    public void Choix3()
    {
        CoutChoix(3);
        Stat.Conscience += 3;
        Stat.ListSouvenir.Add(SouvenirChoix3.GetComponent<SouvenirUI>().LeSouvenir);
        //GameManager.instance.CopyAllSouvenir.Remove(GameManager.instance.CopyAllSouvenir.FirstOrDefault(c => c.Nom == SouvenirChoix3.GetComponent<SouvenirUI>().LeSouvenir.Nom));
        Loot = true;
        EndAutel();
    }

    public void CoutChoix(int Choix)
    {
        switch (Etage)
        {
            case 1:
                switch (Choix)
                {
                    case 1:
                        Stat.Essence -= CoutChoix1[0];
                        break;
                    case 2:
                        Stat.Essence -= CoutChoix2[0];
                        break;
                    case 3:
                        Stat.Essence -= CoutChoix3[0];
                        Stat.Calme -= CoutStatChoix3[0];
                        break;
                }
                break;
            case 2:
                switch (Choix)
                {
                    case 1:
                        Stat.Essence -= CoutChoix1[1];
                        break;
                    case 2:
                        Stat.Essence -= CoutChoix2[1];
                        break;
                    case 3:
                        Stat.Essence -= CoutChoix3[1];
                        Stat.Radiance -= CoutStatChoix3[1];
                        break;
                }
                break;
            case 3:
                switch (Choix)
                {
                    case 1:
                        Stat.Essence -= CoutChoix1[2];
                        break;
                    case 2:
                        Stat.Essence -= CoutChoix2[2];
                        break;
                    case 3:
                        Stat.Essence -= CoutChoix3[2];
                        Stat.Clairvoyance -= CoutStatChoix3[2];
                        break;
                }
                break;
        }
    }

    #endregion Les Choix

    #region Temp Test

    public void Etage1()
    {
        Etage = 1;
    }

    public void Etage2()
    {
        Etage = 2;
    }

    public void Etage3()
    {
        Etage = 3;
    }

    #endregion Temp Test
}
