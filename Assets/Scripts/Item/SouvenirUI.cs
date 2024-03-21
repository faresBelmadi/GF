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
        switch (LeSouvenir.Emotion)
        {
            case Emotion.Joie:
                DescTemp = "Emotion : Joie";
                break;
            case Emotion.Fierte:
                DescTemp = "Emotion : Fierte";
                break;
            case Emotion.Serenite:
                DescTemp = "Emotion : Serenite";
                break;
            case Emotion.Espoir:
                DescTemp = "Emotion : Espoir";
                break;
            case Emotion.Rancune:
                DescTemp = "Emotion : Rancune";
                break;
            case Emotion.frustration:
                DescTemp = "Emotion : frustration";
                break;
            case Emotion.Honte:
                DescTemp = "Emotion : Honte";
                break;
            case Emotion.Nostalgie:
                DescTemp = "Emotion : Nostalgie";
                break;
        }
        return DescTemp;
    }
}
