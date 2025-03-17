using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JeanneBehaviour : BossBehaviour
{
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
}
