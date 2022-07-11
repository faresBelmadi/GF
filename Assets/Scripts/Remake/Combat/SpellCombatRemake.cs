using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellCombatRemake : MonoBehaviour
{ 
    public bool isTurn;
    public SpellRemake Action;
    public Action<SpellRemake> Act;

    public Button button;
    public TextMeshProUGUI texte;
    public GameObject Description;
    public TextMeshProUGUI TexteDescription;

    public void StartUp()
    {
        button = this.GetComponent<Button>();
        texte = this.GetComponentInChildren<TextMeshProUGUI>();
        button.GetComponent<Image>().sprite = Action.Sprite;
        button.onClick.AddListener(ClickAction);
        //texte.text = Action.Name;
        TexteDescription.text = Action.Nom + "\nCost : ";
        foreach (var item in Action.Costs)
        {
            if (item.typeCost == TypeCostSpellRemake.volonte)
                TexteDescription.text += item.Value + "V ";
            if (item.typeCost == TypeCostSpellRemake.radiance)
                TexteDescription.text += item.Value + "R ";
            if (item.typeCost == TypeCostSpellRemake.conscience)
                TexteDescription.text += item.Value + "C ";
        }
        TexteDescription.text += "\n";

        TexteDescription.text += Action.Description;
    }

    private void Update()
    {
        if (isTurn)
            button.interactable = CheckPrice();
        else
            button.interactable = false;
    }

    private bool CheckPrice()
    {
        var stat = GameManager.instance.BattleMan.player.stat;
        foreach (var price in Action.Costs)
        {
            switch (price.typeCost)
            {
                case TypeCostSpellRemake.conscience:
                    if (stat.Conscience < price.Value)
                        return false;
                    break;
                case TypeCostSpellRemake.radiance:
                    if (stat.HP < price.Value)
                        return false;
                    break;
                case TypeCostSpellRemake.volonte:
                    if (stat.Volonté < price.Value)
                        return false;
                    break;
            }
        }
        return true;
    }

    public void ClickAction()
    {
        Act(Action);
    }
}
