using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nostalgie : Emotion
{
    public Nostalgie()
    {
        EmotionName = "Nostalgie";
        EmotionTypeEnum = EmotionTypeEnum.Nostalgie;
    }
    
}
//A la fin d’une Phase, si vous avez dépensé un nombre de points de Volonté supérieur ou égal
//au nombre de souvenirs que vous possédez, vous aurez +XX points de Vitesse et +YY points
//de Clairvoyance lors de la prochaine Phase. Dans le cas contraire, vous récupérez ZZ% de
//votre Radiance manquante par souvenirs que vous possédez.