using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class DamageEffectOnScreen : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve _curveDamageEffectLow;
    [SerializeField]
    private AnimationCurve _curveDamageEffectHigh;
    [SerializeField]
    private Image _damageImage1;
    [SerializeField]
    private Image _damageImage2;
    [SerializeField]
    private float _fadeTimer = 0.5f;
    [SerializeField]
    private float _pulseTimer = 0.5f;
    [SerializeField]
    private float _pulseRatio;
    [SerializeField]
    private Image _imageToPulse;

    private AbstractStatsHandler _stat;
    private float _currentTime = 0f;
    private bool _isScaleUp = true;
    private void Update()
    {
        if (_currentTime >= _pulseTimer)
        { 
            _isScaleUp = !_isScaleUp;
            _currentTime = 0f;
        }

        if (_isScaleUp)
        {
            _imageToPulse.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * _pulseRatio, _currentTime / _pulseTimer);
            _currentTime += Time.deltaTime;
        }
        else
        {
            _imageToPulse.transform.localScale = Vector3.Lerp(Vector3.one * _pulseRatio, Vector3.one, _currentTime / _pulseTimer);
            _currentTime += Time.deltaTime;
        }
    }

    public void Init(AbstractStatsHandler Stat)
    {
        Reset();
        _stat = Stat;
        _stat.OnRadianceChange += UpdateEffect;
    }
    public void Reset()
    {
        if (_stat != null) 
        {
            _stat.OnRadianceChange -= UpdateEffect;
        }
        _stat = null;
    }

    private void UpdateEffect()
    {
        float percent = (_stat.Radiance * 100f) / _stat.RadianceMax;
        float ratio = percent / 100f;
        float alphaValue1 = Mathf.Clamp01(_curveDamageEffectLow.Evaluate(ratio));
        float alphaValue2 = Mathf.Clamp01(_curveDamageEffectHigh.Evaluate(ratio));

        Debug.Log($"Alpha1 : {alphaValue1}, Alpha2 : {alphaValue2}");
        StartCoroutine(FadeImage(_damageImage1, _damageImage1.color.a, alphaValue1, _fadeTimer));
        StartCoroutine(FadeImage(_damageImage2, _damageImage2.color.a, alphaValue2, _fadeTimer));

    }

    private IEnumerator FadeImage(Image imageToFade, float startingAlpha, float endingAlpha, float timer)
    {
        float currentTime = 0f;
        Color startColor = imageToFade.color;
        startColor.a = startingAlpha;
        Color endColor = imageToFade.color;
        endColor.a = endingAlpha;
        while (currentTime < timer)
        {
            imageToFade.color = Color.Lerp(startColor, endColor, currentTime / timer);
            currentTime += Time.deltaTime;
            yield return null;
        }
        imageToFade.color = endColor;
    }
}
