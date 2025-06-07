using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolontePoint : Point
{
    [SerializeField]
    private Image _volonteSprite;
    [SerializeField]
    private Sprite _volonteFull;
    [SerializeField]
    private Sprite _volonteEmpty;
    



    // Start is called before the first frame update
    void Start()
    {
        _volonteSprite.sprite = _volonteFull;
    }

    public override void Full()
    {
        _volonteSprite.sprite = _volonteFull;
    }
    public override void Empty()
    {
        _volonteSprite.sprite = _volonteEmpty;
    }
    
}
