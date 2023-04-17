using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SouvenirUI : MonoBehaviour
{
    public Souvenir LeSouvenir;
    public GameObject SouvenirObject;
    public TextMeshProUGUI TexteDescription;

    public void StartUp()
    {
        SouvenirObject = this.gameObject;
        SouvenirObject.GetComponent<Image>().sprite = LeSouvenir.Icon;
        TexteDescription.text = LeSouvenir.Nom + "\n" + DescriptionEmotion() + "\n" + "Slots : " + LeSouvenir.Slots.ToString() + "\n" + LeSouvenir.Description;
    }

    private string DescriptionEmotion()
    {
        string DescTemp = LeSouvenir.Emotion.EmotionName;
        switch (LeSouvenir.Emotion.EmotionTypeEnum)
        {
            case EmotionTypeEnum.Joie:
                DescTemp += "EmotionTypeEnum : Joie";
                break;
            case EmotionTypeEnum.Fierte:
                DescTemp += "EmotionTypeEnum : Fierte";
                break;
            case EmotionTypeEnum.Serenite:
                DescTemp += "EmotionTypeEnum : Serenite";
                break;
            case EmotionTypeEnum.Espoir:
                DescTemp += "EmotionTypeEnum : Espoir";
                break;
            case EmotionTypeEnum.Rancune:
                DescTemp += "EmotionTypeEnum : Rancune";
                break;
            case EmotionTypeEnum.Frustration:
                DescTemp += "EmotionTypeEnum : Frustration";
                break;
            case EmotionTypeEnum.Honte:
                DescTemp += "EmotionTypeEnum : Honte";
                break;
            case EmotionTypeEnum.Nostalgie:
                DescTemp += "EmotionTypeEnum : Nostalgie";
                break;
        }
        return DescTemp;
    }
}
