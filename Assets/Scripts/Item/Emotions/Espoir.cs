using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Espoir : Emotion
{
    public Espoir()
    {
        EmotionName = "Espoir";
        EmotionTypeEnum = EmotionTypeEnum.Espoir;
    }

    public void AmplifyStats() // a appeler apres chaque combat, si le joeur possede le souvenir avec de l'espoir?
    {
        Debug.Log("Espoir trigered - not implemented yet");
    }
    
}
//Après chaque affrontement (mais avant d’obtenir les récompenses), si vous avez plus de 50%
//de votre Radiance max., les statistiques de X souvenir(s) aléatoire(s) sont amplifiées(les
//statistiques apportées par le souvenir passent au palier supérieur : vitesse + 2 passe à +4 par
//exemple et dans le sens inverse si c’est un malus).