using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using UnityEngine;

public class JeanneBehaviour : BossBehaviour
{
    public override void OnDestroy()
    {
        Stat.OnCustomStatModification -= UpdateDivin;
        base.OnDestroy();
    }
    public override void ChooseNextAction()
    {
        bool colere = false;
        foreach (var item in Stat.ListBuffDebuff)
        {
            foreach (var effect in item.Effet)
            {
                if (effect.TypeEffet == TypeEffet.Colere)
                {
                    UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
                    var temp = UnityEngine.Random.Range(0, 100);
                    if (temp <= effect.Pourcentage)
                        colere = true;
                }
            }
        }

        if (Spells == null)
            CreateSpellList();

        nextAction = Spells.First();

        foreach (var item in Spells)
        {
            if (nextAction.IsAttaque && colere)
            {

            }
            else if (item.Weight < nextAction.Weight)
            {

                nextAction = item;
            }
        }

        if (nextAction.Effet.FirstOrDefault(c => c.TypeEffet == TypeEffet.UltimeJeanne))
        {
            if (Stat.Divin < 70)
            {
                var temp = Spells.First();
                foreach (var item in Spells)
                {
                    if (nextAction != item && temp.Weight > item.Weight)
                    {
                        temp = item;
                    }
                }
                nextAction = temp;
            }
        }


        nextAction.Weight += nextAction.AddedWeight;
        foreach (var item in Spells)
        {
            if (item != nextAction)
                item.Weight--;
        }

        NextActionType();
        UpdateIntention();
    }
    public override void SetUp()
    {
        base.SetUp();

        Stat.OnCustomStatModification += UpdateDivin;
    }
    private void UpdateDivin()
    {
        if (GameManager.Instance.BattleMan.IsCombatOn)
        {
            //if (!Stat.ListBuffDebuff.Any(x => x.Nom == TradManager.instance.GetTranslation(GameManager.Instance.passifRules.CurrentDivin.idTradName)))
            //{
            //    // _rules.CurrentDivin.Description = behavior.Stat.Divin.ToString();
            //    AddBuffDebuff(GameManager.Instance.passifRules.CurrentDivin, Stat);
            //}

            //var currentDivin = ListBuffDebuffGO.FirstOrDefault(x => x.GetComponent<BuffDebuffComponant>().buffName == TradManager.instance.GetTranslation(GameManager.Instance.passifRules.CurrentDivin.idTradName));
            //currentDivin.GetComponent<BuffDebuffComponant>().popUpPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text += "(Divin : " + Stat.Divin + ")";
        }
    }
}
