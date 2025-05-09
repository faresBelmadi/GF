using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DialogBuffEffectComponent : MonoBehaviour
{
    [SerializeField]
    private Image _targetImage;
    [SerializeField]
    private Image _effectImage;

    private List<Sprite> _targets;
    // Start is called before the first frame update
    void Start()
    {
        
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
}
