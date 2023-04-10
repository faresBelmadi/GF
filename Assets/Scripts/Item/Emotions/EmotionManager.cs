public class EmotionManager
{
    public Emotion CreateEmotion(EmotionTypeEnum emotionType)
    {
        switch (emotionType)
        {
            case EmotionTypeEnum.Joie:
                return new Joie();
            case EmotionTypeEnum.Espoir:
                return new Emotion();
            case EmotionTypeEnum.Fierte:
                return new Emotion();
            case EmotionTypeEnum.Honte:
                return new Emotion();
            case EmotionTypeEnum.Nostalgie:
                return new Emotion();
            case EmotionTypeEnum.Rancune:
                return new Emotion();
            case EmotionTypeEnum.Serenite:
                return new Emotion();
            case EmotionTypeEnum.frustration:
                return new Emotion();
            default:
                return null;
        }
    }

}
