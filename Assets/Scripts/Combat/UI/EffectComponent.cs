using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class EffectComponent : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _descriptionText;
    [SerializeField]
    private TMP_Text _nameText;
    [SerializeField]
    private GameObject _popupPanel;
    

    
    private void OnEnable()
    {
        CombatBehavior.OnUpdateUI += UpdateUI;
    }
    private void OnDisable()
    {
        CombatBehavior.OnUpdateUI -= UpdateUI;
    }
  

    public void SetSprite(Sprite sprite)
    {
        GetComponent<Image>().sprite = sprite;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {

        _popupPanel.SetActive(true);

        GameObject posGO = GameObject.FindGameObjectsWithTag("TooltipPosition")[0];
        if (posGO != null)
        {
            _popupPanel.transform.position = posGO.transform.position;
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("exit");
        _popupPanel.SetActive(false);
    }
    public void UpdateUI()
    {
        // buffDescriptionLabel.text = TradManager.instance.GetTranslation(_buffDebuff.idTradDescription, "Missing description");
    }
 
}
