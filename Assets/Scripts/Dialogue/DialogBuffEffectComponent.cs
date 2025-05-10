using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class DialogBuffEffectComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image _targetImage;
    [SerializeField]
    private Image _effectImage;

    private string _name;
    private string _description;

    private List<Sprite> _targets;

    private void OnEnable()
    {
        CombatBehavior<CharacterStat>.OnUpdateUI += UpdateUI;
    }
    private void OnDisable()
    {
        CombatBehavior<CharacterStat>.OnUpdateUI -= UpdateUI;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTargetsSprite(List<Sprite> targetSprites) => SetSprites(null, targetSprites);
    public void SetEffectSprite(Sprite effectSprite) => SetSprites(effectSprite, null);
    public void SetSprites(Sprite effectSprite, List<Sprite> targetSprites)
    {
        if (effectSprite != null)
            _effectImage.sprite = effectSprite;

        if (targetSprites != null)
        {
            _targets = new List<Sprite>(targetSprites);
            _targetImage.sprite = _targets.FirstOrDefault();
        }
    }

    public void SetNameAdDescriptionText(string name, string desc)
    {
        _name = name;
        _description = desc;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.DialManager.ShopPopup(_name, _description);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.DialManager.HidePopup();
    }
    public void UpdateUI()
    {
        // buffDescriptionLabel.text = TradManager.instance.GetTranslation(_buffDebuff.idTradDescription, "Missing description");
    }
}
