using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Serenite : Emotion
{
    public Serenite()
    {
        EmotionName = "Serenite";
        EmotionTypeEnum = EmotionTypeEnum.Serenite;
    }
    
}
//Tant que votre jauge de Tension n’est pas pleine, si vous n’infligez pas de dégâts directs durant
//votre Tour, votre Force d’âme est augmentée de XX% jusqu’à la fin de l’affrontement. Si vous
//avez moins de 25% de votre Radiance max., les gains de Tension sont doublés.
//Joueur behaviorf in de tour si existe
