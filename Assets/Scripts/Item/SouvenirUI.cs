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
        string DescTemp = "";
        switch (LeSouvenir.Emotion.EmotionTypeEnum)
        {
            case EmotionTypeEnum.Joie:
                DescTemp = "Emotion : Joie";
                break;
            case EmotionTypeEnum.Fierte:    
                DescTemp = "Emotion : Fierte";
                break;
            case EmotionTypeEnum.Serenite:
                DescTemp = "Emotion : Serenite";
                break;
            case EmotionTypeEnum.Espoir:
                DescTemp = "Emotion : Espoir";
                break;
            case EmotionTypeEnum.Rancune:
                DescTemp = "Emotion : Rancune";
                break;
            case EmotionTypeEnum.Frustration:
                DescTemp = "Emotion : frustration";
                break;
            case EmotionTypeEnum.Honte:
                DescTemp = "Emotion : Honte";
                break;
            case EmotionTypeEnum.Nostalgie:
                DescTemp = "Emotion : Nostalgie";
                break;
        }
        return DescTemp;
    }
}
