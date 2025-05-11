using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogBuffEffectComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image _targetImage;
    [SerializeField]
    private Image _effectImage;
    [SerializeField]
    private float _timebetweenTarget;
    [SerializeField]
    private float _rotTime;

    private string _name;
    private string _description;

    private List<Sprite> _targets;
    private List<UIEnnemi> _enemyUIList;
    private JoueurBehavior _player = null;

    private int _currentIndex = 0;
    private float _currentTime = 0;
    private float _currentRotTime = 0;
    private bool _isSpinning = false;
    private float _currentRot;
    private void Start()
    {
        _currentIndex = 0;
        _currentTime = 0;
        _isSpinning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_targets.Count > 1)
        {
            _currentTime += Time.deltaTime;
            if (!_isSpinning && _currentTime >= _timebetweenTarget)
            {
                _isSpinning = true;
                _currentTime = 0f;
                StartCoroutine(SpinAndSwap());
            }
        }
    }
    // Coroutine pour tourner et swap le sprite du cadre de cible
    private IEnumerator SpinAndSwap()
    {
        _currentRotTime = 0f;
        Quaternion startRot = Quaternion.Euler(0f, 0f, 0f);
        Quaternion endRot = Quaternion.Euler(0f, 90f, 0f);
        while (_currentRotTime < _rotTime)
        {
            _targetImage.transform.parent.rotation = Quaternion.Lerp(startRot, endRot, _currentRotTime / _rotTime);
            _currentRotTime += Time.deltaTime;
            yield return null;
        }

        _targetImage.transform.parent.rotation = Quaternion.Euler(0f, 270f, 0f); ;
        startRot = _targetImage.transform.parent.rotation;
        endRot = Quaternion.Euler(0f, 360f, 0f);
        ChangeTargetSprite();
        _currentRotTime = 0f;
        while (_currentRotTime < _rotTime)
        {
            _targetImage.transform.parent.rotation = Quaternion.Lerp(startRot, endRot, _currentRotTime / _rotTime);
            _currentRotTime += Time.deltaTime;
            yield return null;
        }
        _targetImage.transform.parent.rotation = Quaternion.Euler(0f, 0f, 0f);
        _isSpinning = false;
        _currentTime = 0f;
    }
    private void ChangeTargetSprite()
    {
        _currentIndex = (_currentIndex + 1) % _targets.Count;
        _targetImage.sprite = _targets[_currentIndex];
    }
    public void SetEnemyUI(List<UIEnnemi> enemyList)
    {
        _enemyUIList = new List<UIEnnemi>(enemyList);
    }
    public void SetPlayer(JoueurBehavior player)
    {
        _player = player;
    }
    public void SetTargetsSprite(List<Sprite> targetSprites) => SetSprites(null, targetSprites);
    public void SetEffectSprite(Sprite effectSprite) => SetSprites(effectSprite, null);
    public void SetSprites(Sprite effectSprite, List<Sprite> targetSprite)
    {
        if (effectSprite != null)
            _effectImage.sprite = effectSprite;

        if (targetSprite != null)
        {
            _targets = new List<Sprite>(targetSprite);
            _targetImage.sprite = _targets.FirstOrDefault();
        }
    }

    public void SetNameAdDescriptionText(string name, string desc)
    {
        _name = name;
        _description = desc;
    }
    public void ShowEnemyTargeting()
    {
        foreach(var enemyUI in _enemyUIList)
        {
            enemyUI.ShowTargeting();
        }

        if (_player != null)
        {
            _player.ShowTargeting();
        }
    }
    public void HideEnemyTargeting()
    {
        foreach (var enemyUI in _enemyUIList)
        {
            enemyUI.HideTargeting();
        }

        if (_player != null)
        {
            _player.HideTargeting();
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.DialManager.ShopPopup(_name, _description);
        ShowEnemyTargeting();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.DialManager.HidePopup();
        HideEnemyTargeting();
    }
   
}
