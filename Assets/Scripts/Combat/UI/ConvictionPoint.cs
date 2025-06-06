
using UnityEngine;
using UnityEngine.UI;

public class ConvictionPoint : Point
{
    [SerializeField]
    private Image _convictionSprite;
    [SerializeField]
    private Sprite _convictionPositive;
    [SerializeField]
    private Sprite _convictionNegative;
    [SerializeField]
    private Sprite _convictionEmpty;

    [SerializeField]
    private Gradient _positiveBloomGradient;
    [SerializeField]
    private Gradient _negativeBloomGradient;

    [SerializeField]
    private Material _positiveMaterial;
    [SerializeField]
    private Material _negativeMaterial;


    void Start()
    {
        _convictionSprite.sprite = _convictionEmpty;
    }

    public void Positive()
    {
        var tempMaterial = Instantiate(_positiveMaterial);
        _convictionSprite.sprite = _convictionPositive;
        _convictionSprite.material = tempMaterial;
        _bloomVolonteComponent.BloomGradient = _positiveBloomGradient;
        _bloomVolonteComponent.bloomMaterial = tempMaterial;
    }
    public void Negative()
    {
        var tempMaterial = Instantiate(_negativeMaterial);
        _convictionSprite.sprite = _convictionNegative;
        _convictionSprite.material = tempMaterial;
        _bloomVolonteComponent.BloomGradient = _negativeBloomGradient;
        _bloomVolonteComponent.bloomMaterial = tempMaterial;
    }
    public override void Empty()
    {
        _convictionSprite.sprite = _convictionEmpty;
    }

    public override void Full()
    {
    }
}