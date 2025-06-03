using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class DamageEffectOnScreen : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve _curve;
    [SerializeField]
    private AnimationCurve _curve2;
    [SerializeField]
    private Image _damageImage1;
    [SerializeField]
    private Image _damageImage2;

    private JoueurStat _stat;

    public void Init(JoueurStat Stat)
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
        float alphaValue1 = Mathf.Clamp01(_curve.Evaluate(ratio));
        float alphaValue2 = Mathf.Clamp01(_curve2.Evaluate(ratio));

        Debug.Log($"Alpha1 : {alphaValue1}, Alpha2 : {alphaValue2}");
        Color newColor = new Color(_damageImage1.material.color.r, _damageImage1.material.color.g, _damageImage1.material.color.b, alphaValue1);
        Color newColor2 = new Color(_damageImage2.material.color.r, _damageImage2.material.color.g, _damageImage2.material.color.b, alphaValue2);
        _damageImage1.color = newColor;
        _damageImage2.color = newColor2;

    }
}
