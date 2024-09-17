using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[System.Serializable]
public class SouvenirUI : MonoBehaviour
{
    [HideInInspector]
    public Souvenir LeSouvenir;
    public TextMeshProUGUI TexteDescription;
    [SerializeField]
    private GameObject SouvenirImageGameObject;
    [Tooltip("Set the rarity border, starting with 0 = most common")]
    [SerializeField]
    private List<GameObject> _rarityBorders;

    public void StartUp()
    {
        SouvenirImageGameObject.GetComponent<Image>().sprite = LeSouvenir.Icon;
        TexteDescription.text = LeSouvenir.SouvenirName + "\n" + DescriptionEmotion() + "\n" + "Slots : " + LeSouvenir.Slots.ToString() + "\n" + LeSouvenir.SouvenirDesc;
        SetRarityBorder(LeSouvenir.Rarete);
    }

    private void SetRarityBorder(Rarity rarity)
    {
        for (int i=0; i < _rarityBorders.Count;i++)
        {
            _rarityBorders[i].SetActive(false);
        }
        if ((int)rarity>= _rarityBorders.Count)
        {
            Debug.LogError($"Rarity {LeSouvenir.Rarete} for souvenir {LeSouvenir.name} not supported");
            _rarityBorders[0].SetActive(true);
        }
        else
        {
            _rarityBorders[(int)LeSouvenir.Rarete].SetActive(true);
        }
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
