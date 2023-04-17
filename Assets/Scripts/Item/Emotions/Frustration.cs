using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frustration : Emotion
{
    public Frustration()
    {
        EmotionName = "Frustration";
        EmotionTypeEnum = EmotionTypeEnum.Frustration;
    }
    
}
//Au début de votre Tour, s’il vous reste moins de 2 points de Conscience, la première capacité
//utilisée obtient un bonus aux dégâts infligés de XX%. A la fin de votre Tour, s’il vous reste au
//moins 1 point de Volonté, vous gagnez YY points de Tension.

