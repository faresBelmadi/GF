using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fierte : Emotion
{
    public static int nbCapacity;
    public Fierte()
    {
        EmotionName = "Fierte";
        EmotionTypeEnum = EmotionTypeEnum.Fierte;
        GameManager.instance.BattleMan.capacityUsedEvent.AddListener(CapacityUsed);
    }

    public void CapacityUsed()
    {
        nbCapacity++;
        if (nbCapacity == 3)
        {
            Debug.Log("3 capacité Utilisé");
        }
    }
    
}
//Lors de chaque affrontement, les 3 premières capacités utilisées (offensives ou non) obtiennent
  //  un bonus aux dégâts infligés équivalent à XX% de votre Radiance actuelle pour chaque pièce
//visitée de l’étage actuel.
