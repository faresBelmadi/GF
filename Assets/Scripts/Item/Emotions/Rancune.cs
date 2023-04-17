using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rancune : Emotion
{
    public Rancune()
    {
        EmotionName = "Rancune";
        EmotionTypeEnum = EmotionTypeEnum.Rancune;
    }
    
}
//Quand au moins un désavantage (débuff) vous affecte, le nombre de points de Conscience
//nécessaires pour utiliser une capacité est réduit de 1. Les compétences dont le coût en
//Conscience n’est pas annulé par cet effet obtiennent un bonus aux dégâts infligés équivalent à
//XX% de votre Force d’âme.