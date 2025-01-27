using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[System.Serializable]
public class SouvenirUI : MonoBehaviour
{
   // [HideInInspector]
    public Souvenir LeSouvenir;
    [SerializeField]
    private GameObject _descriptionGO;
    public TextMeshProUGUI TexteDescription;
    [SerializeField]
    private SpriteRenderer _souvenirImageRenderer;
    [Tooltip("Set the rarity border, starting with 0 = most common")]
    [SerializeField]
    private List<Sprite> _rarityBorders;
    [SerializeField]
    private SpriteRenderer _rarityBordersRenderer;

    private const string EMOTIONID      = "Souv1Desc1";
    private const string SLOTID         = "Souv1Desc2";
    private const string JOYID          = "SE1";
    private const string PRIDEID        = "SE2";
    private const string SERENITYID     = "SE3";
    private const string HOPEID         = "SE4";
    private const string GRUDGEID       = "SE5";
    private const string FRUSTRATIONID  = "SE6";
    private const string SHAMEID        = "SE7";
    private const string NOSTALGIAID    = "SE8";

    private DescriptionHoverVisibility rectVisibility = new DescriptionHoverVisibility();
    public void StartUp()
    {
        _souvenirImageRenderer.sprite = LeSouvenir.Icon;
        TexteDescription.text = LeSouvenir.SouvenirName + "\n" + DescriptionEmotion() + "\n" + TradManager.instance.GetTranslation("Souv1Desc2", "Slots") + " : " + LeSouvenir.Slots.ToString() + "\n" + LeSouvenir.SouvenirDesc;
        SetRarityBorder(LeSouvenir.Rarete);
        HideDescription();
    }

    private void SetRarityBorder(Rarity rarity)
    {
        _rarityBordersRenderer.sprite = _rarityBorders[(int)LeSouvenir.Rarete];
    }

    private string DescriptionEmotion()
    {
        string DescTemp = "";
        switch (LeSouvenir.Emotion)
        {
            case Emotion.Joie:
                DescTemp = TradManager.instance.GetTranslation(EMOTIONID, "émotion") + " : " + TradManager.instance.GetTranslation(JOYID, "Joie");
                break;
            case Emotion.Fierte:
                DescTemp = TradManager.instance.GetTranslation(EMOTIONID, "émotion") + " : " + TradManager.instance.GetTranslation(PRIDEID, "fierte");
                break;
            case Emotion.Serenite:
                DescTemp = TradManager.instance.GetTranslation(EMOTIONID, "émotion") + " : " + TradManager.instance.GetTranslation(SERENITYID, "serenite");
                break;
            case Emotion.Espoir:
                DescTemp = TradManager.instance.GetTranslation(EMOTIONID, "émotion") + " : " + TradManager.instance.GetTranslation(HOPEID, "espoir");
                break;
            case Emotion.Rancune:
                DescTemp = TradManager.instance.GetTranslation(EMOTIONID, "émotion") + " : " + TradManager.instance.GetTranslation(GRUDGEID, "rancune");
                break;
            case Emotion.frustration:
                DescTemp = TradManager.instance.GetTranslation(EMOTIONID, "émotion") + " : " + TradManager.instance.GetTranslation(FRUSTRATIONID, "frustration");
                break;
            case Emotion.Honte:
                DescTemp = TradManager.instance.GetTranslation(EMOTIONID, "émotion") + " : " + TradManager.instance.GetTranslation(SHAMEID, "honte");
                break;
            case Emotion.Nostalgie:
                DescTemp = TradManager.instance.GetTranslation(EMOTIONID, "émotion") + " : " + TradManager.instance.GetTranslation(NOSTALGIAID, "nostalgie");
                break;
        }
        return DescTemp;
    }
    public void ShowDescription()
    {
        _descriptionGO.SetActive(true);
       
        rectVisibility.CheckVisibilityAndAdjustPosition(_descriptionGO.GetComponent<RectTransform>(), FindAnyObjectByType<Camera>());
    }
    public void HideDescription()
    {
        _descriptionGO.SetActive(false);
    }

    private void OnMouseEnter()
    {
        ShowDescription();
    }
    private void OnMouseExit()
    {
        HideDescription();
    }
}
