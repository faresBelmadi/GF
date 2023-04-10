using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Emotion", menuName = "Remake/Item/Create New Emotion", order = 11)]
public class Joie : Emotion
{
    public Joie()
    {
        EmotionName = "Joie";
        EmotionTypeEnum = EmotionTypeEnum.Joie;
    }
}
//observable list dans character stat
//en tant que joie je subcribe aux event de buff


//en tant que fierté je subcribe aux event de debut combat et j'applique les effets

//gerer les emotions en sriptable, ou en objet? pas tant que sa , peut etre objet plus simple ?