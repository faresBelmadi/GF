using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Honte : Emotion
{
    public Honte()
    {
        EmotionName = "Honte";
        EmotionTypeEnum = EmotionTypeEnum.Honte;
    }
    
}
//Lors de chaque affrontement, tant que vous attaquez uniquement des adversaires qui ont des
//intentions agressives(attaque directe et application de débuff), vous gagnez un 1 point de
//Conviction et XX% de Radiance max. par attaque effectuée (si la chaine est rompue, les bonus
//sont perdus). //tant qu'on sais que l'ennemie vas attaquer, alors faire
