using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickPopUpAccesor : MonoBehaviour
{
    [Header("Componanats")]
    [SerializeField] public GameObject backGroundObject;
    [SerializeField] public GameObject effectObject;
    [SerializeField] public GameObject textObject;

    public void SetText(string text)
    {
        textObject.GetComponent<TextMeshProUGUI>().text = text;
    }
    public void SetTextColor(Color color) 
    {
        textObject.GetComponent<TextMeshProUGUI>().color = color;
    }
    public void SetEffectSprite(Sprite sprite)
    {
        if (sprite != null)
        {
            effectObject.GetComponent<Image>().sprite = sprite;
        }
        else
        {
            effectObject.GetComponent<Image>().color = new Color(1f,1f,1f,0f);
        }
    }public void SetBackbroundSprite(Sprite sprite)
    {
        if (sprite != null)
        {
            backGroundObject.GetComponent<Image>().sprite = sprite;
        }
    }
    public void SetPopUpAlpha(float alpha)
    {
        Color textCol = textObject.GetComponent<TextMeshProUGUI>().color;
        textCol.a = alpha;
        textObject.GetComponent<TextMeshProUGUI>().color = textCol;
        float currentEffectAlpha = effectObject.GetComponent<Image>().color.a;
        effectObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, Mathf.Min(currentEffectAlpha, alpha));
        backGroundObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, alpha);
    }
}
