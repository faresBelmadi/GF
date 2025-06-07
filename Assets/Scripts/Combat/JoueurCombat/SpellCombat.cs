using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellCombat : MonoBehaviour
{
    public bool isTurn;
    private bool isClicable = true;
    public bool isUsable = true;
    public Spell Action;
    public Action<Spell> Act;
    public GameObject selectedSpell;
    public Button button;
    public TextMeshProUGUI texte;
    public GameObject DescriptionObject;
    public TextMeshProUGUI TexteDescription;

    public void StartUp()
    {
        button = this.GetComponent<Button>();
        texte = this.GetComponentInChildren<TextMeshProUGUI>();
        button.GetComponent<Image>().sprite = Action.Sprite;
        button.onClick.AddListener(ClickAction);
        //texte.text = Action.Nom;
        UpdateDescription();
    }

    //private void Update()
    //{
    //    if (isTurn && isUsable)
    //        button.interactable = CheckPrice();
    //    else
    //        button.interactable = false;
    //}

    public bool CheckPrice()
    {
        JoueurStat stat;
        if (GameManager.Instance != null)
            stat = GameManager.Instance.BattleMan?.player.Stat;
        else
            stat = TutoManager.Instance.BattleManager?.player.Stat;
        if (stat != null)
        {
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
        else
        {
            return false;
        }
    }

    public void ClickAction()
    {
        if (isClicable)
        {
            Act(Action);
            isClicable = false;
            Invoke("PreventInstaDoubleClick", 0.01f);
        }

    }

    //bug : le clic est detecté plusieur fois donc le spell est lancé plusieurs fois. On a jamais eu ce bug avant a cause du systeme de ciblage, mais vu qu'on le bypass pour les buffs, il devient visible.
    public void PreventInstaDoubleClick()
    {
        isClicable = true;
    }

    public void UpdateDescription()
    {
        string spellName;
        string spellDescription;
        if (!string.IsNullOrEmpty(Action.idTradName) && !string.IsNullOrEmpty(Action.idTradDescription))
        {
            List<float> variableValues = new List<float>();

            foreach (Effet e in Action.ActionEffet)
            {
                if (e.ValeurBrut != 0)
                    variableValues.Add(Mathf.Abs(e.ValeurBrut));
                if (e.Pourcentage != 0)
                    variableValues.Add(Mathf.Abs((float)e.Pourcentage));
            }

            foreach (BuffDebuff b in Action.ActionBuffDebuff)
            {
                foreach (Effet e in b.Effet)
                {
                    if (e.ValeurBrut != 0)
                        variableValues.Add(Mathf.Abs(e.ValeurBrut));
                    if (e.Pourcentage != 0)
                        variableValues.Add(Mathf.Abs((float)e.Pourcentage));
                }
            }
            spellName = TradManager.instance.GetTranslation(Action.idTradName, Action.name);
            spellDescription = TradManager.instance.GetTranslation(Action.idTradDescription, Action.Description);
        }
        else
        {
            if (string.IsNullOrEmpty(Action.idTradName))
                Debug.Log("IdTradName est null/empty pour " + Action.name);
            if (string.IsNullOrEmpty(Action.idTradDescription))
                Debug.Log("idTradDescription est null/empty pour " + Action.name);
            spellName = Action.name;
            spellDescription = Action.Description;
        }

        TexteDescription.text = "<color=white><size=150%>" + spellName + "</size></color>\n<u>Cout :</u> ";
        foreach (var item in Action.Costs)
        {
            if (item.typeCost == TypeCostSpell.volonte)
                TexteDescription.text += item.Value + "<sprite name=\"" +
                    ((GameManager.Instance != null) ? GameManager.Instance.StatIcons.StatVolonte.name : TutoManager.Instance.StatIcons.StatVolonte.name) + "\">";
            if (item.typeCost == TypeCostSpell.radiance)
                TexteDescription.text += item.Value + "<sprite name=\"" +
                    ((GameManager.Instance != null) ? GameManager.Instance.StatIcons.StatRadiance.name : TutoManager.Instance.StatIcons.StatRadiance.name) + "\">";
            if (item.typeCost == TypeCostSpell.conscience)
                TexteDescription.text += item.Value + "<sprite name=\"" +
                    ((GameManager.Instance != null) ? GameManager.Instance.StatIcons.StatConscience.name : TutoManager.Instance.StatIcons.StatConscience.name) + "\">";
        }

        TexteDescription.text += "\n";

        TexteDescription.text += spellDescription;
    }
}