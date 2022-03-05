using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class spellCombat : MonoBehaviour
{
    public bool isTurn;
    public Spell Action;
    public Action<Spell> Act;

    public Button button;
    public TextMeshProUGUI texte;

    public void StartUp()
    {
        button = this.GetComponent<Button>();
        texte = this.GetComponentInChildren<TextMeshProUGUI>();
        button.GetComponent<Image>().sprite = Action.IconSprite;
        button.onClick.AddListener(ClickAction);
        //texte.text = Action.Name; 
    }

    private void Update() {
        if(isTurn)
            button.interactable = CheckPrice();
        else
            button.interactable = false;
    }

    private bool CheckPrice()
    {
        var stat = GameManager.instance.BattleMan.player.stat;
        foreach(var price in Action.Costs)
        {
            switch (price.typeCost)
            {
                case TypeCostSpell.Conscience:
                if(stat.Conscience < price.Value)
                    return false;
                break;
                case TypeCostSpell.Radiance:
                if(stat.HP < price.Value)
                    return false;
                break;
                case TypeCostSpell.Volonté:
                if(stat.Volonté < price.Value)
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
