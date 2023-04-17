using System.Collections.Generic;
using UnityEngine;

public class Emotion
{
    public string EmotionName;
    public EmotionTypeEnum EmotionTypeEnum;
    public List<Effet> EmotionEffets;
    public List<BuffDebuff> EmotionBuffDebuffs;
    public List<Passif> EmotionPassifs;
}