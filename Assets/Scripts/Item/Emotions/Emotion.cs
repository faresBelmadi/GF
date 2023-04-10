using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Emotion", menuName = "Remake/Item/Create New Emotion", order = 11)]
public class Emotion : ScriptableObject
{
    public string EmotionName;
    public EmotionTypeEnum EmotionTypeEnum;
    public List<Effet> EmotionEffets;
    public List<BuffDebuff> EmotionBuffDebuffs;
    public List<Passif> EmotionPassifs;
}