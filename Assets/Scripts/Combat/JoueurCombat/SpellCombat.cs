﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellCombat : MonoBehaviour
{ 
    public bool isTurn;
    public Spell Action;
    public Action<Spell> Act;

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
        //texte.text = Action.Nom;
        TexteDescription.text = Action.Nom + "\nCost : ";
        foreach (var item in Action.Costs)
        {
            if (item.typeCost == TypeCostSpell.volonte)
                TexteDescription.text += item.Value + "V ";
            if (item.typeCost == TypeCostSpell.radiance)
                TexteDescription.text += item.Value + "R ";
            if (item.typeCost == TypeCostSpell.conscience)
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
        var stat = GameManager.instance.BattleMan.player.Stat;
        foreach (var price in Action.Costs)
        {
            switch (price.typeCost)
            {
                case TypeCostSpell.conscience:
                    if (stat.Conscience < price.Value)
                        return false;
                    break;
                case TypeCostSpell.radiance:
                    if (stat.Radiance < price.Value)
                        return false;
                    break;
                case TypeCostSpell.volonte:
                    if (stat.Volonter < price.Value)
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