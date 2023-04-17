using System;
using UnityEngine;
using UnityEngine.Rendering;

public class EmotionManager
{
    public Emotion CreateEmotion(EmotionTypeEnum emotionType)
    {
        switch (emotionType)
        {
            case EmotionTypeEnum.Joie:
                return new Joie();
            case EmotionTypeEnum.Espoir:
                return new Espoir();
            case EmotionTypeEnum.Fierte:
                return new Fierte();
            case EmotionTypeEnum.Honte:
                return new Honte();
            case EmotionTypeEnum.Nostalgie:
                return new Nostalgie();
            case EmotionTypeEnum.Rancune:
                return new Rancune();
            case EmotionTypeEnum.Serenite:
                return new Serenite();
            case EmotionTypeEnum.Frustration:
                return new Frustration();
            default:
                return null;
        }
    }
}
