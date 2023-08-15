using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Unity.Jobs;
using UnityEditor.Experimental.GraphView;

public class NewAutelManager : MonoBehaviour
{
    public GameObject MenuUiPanel;
    public GameObject LevelUpUiPanel;

    public TextMeshProUGUI EssenceText;
    public TextMeshProUGUI DescriptionSpellText;
    public TextMeshProUGUI CostCapaText;

    public List<GameObject> AllSpellsIcon;
    public List<GameObject> AllLink;

    public Button BuyButton;

    void FixedUpdate()
    {
        EssenceText.text = "Essence : " + GameManager.instance.playerStat.Essence;
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
    }

    public void SelectSpell(int Id)
    {
        var listOfCompetences = GameManager.instance.classSO.Competences;

        foreach (var capa in listOfCompetences)
        {
            if (capa.Spell?.IDSpell == Id)
            {

                DescriptionSpellText.text = capa.Spell.Description;
                CostCapaText.text = "cout : " + capa.EssenceCost;
                if (capa.EssenceCost <= GameManager.instance.playerStat.Essence)
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
        capa.Bought = true;
        capa.Equiped = true;
        GameManager.instance.playerStat.ListSpell.Add(capa.Spell);
    }
}
