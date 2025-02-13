using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpriteHolder : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _rendererLightOff;
    [SerializeField]
    private SpriteRenderer _rendererLightOn;
    [SerializeField]
    [Range(0, 1)]
    private float _alpha;
    [SerializeField]
    private float _duration;


    
    public void InitSprite()
    {
        _rendererLightOff.color = new Color(_rendererLightOff.color.r, _rendererLightOff.color.g, _rendererLightOff.color.b, _alpha);
        _rendererLightOn.color = new Color(_rendererLightOn.color.r, _rendererLightOn.color.g, _rendererLightOn.color.b, 0f);

    }
    public void LightOn()
    {
        StartCoroutine(FadeImage());
    }
    private IEnumerator FadeImage()
    {
        float time = 0;
        Color initialOffColor = _rendererLightOff.material.color;
        Color initialOnColor = _rendererLightOn.material.color;
       
        while (time < _duration)
        {
            float alphaCroissant = Mathf.Lerp(0, _alpha, time / _duration);
            float alphaDecroissant = Mathf.Lerp(_alpha, 0, time / _duration);
            _rendererLightOff.color = new Color(initialOffColor.r, initialOffColor.g, initialOffColor.b, alphaDecroissant);
            _rendererLightOn.color = new Color(initialOnColor.r, initialOnColor.g, initialOnColor.b, alphaCroissant);
           
            time += Time.deltaTime;
            yield return null;
        }
        _rendererLightOff.color = new Color(initialOffColor.r, initialOffColor.g, initialOffColor.b, 0);
        _rendererLightOn.color = new Color(initialOnColor.r, initialOnColor.g, initialOnColor.b, _alpha);

    }
}
