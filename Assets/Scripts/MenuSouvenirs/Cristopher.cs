using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cristopher : MonoBehaviour, IDropZone
{
    [SerializeField]
    private MenuStatManager _menuStatManager;
    [SerializeField]
    private List<Transform> _freeSlots;
    [SerializeField]
    private List<SpriteRenderer> _cristopherParts;
    private GameObject _prefab;
    [SerializeField]
    [Range(0f,1f)]
    private float _unactiveAlpha;
    [SerializeField]
    [Range(0f, 1f)]
    private float _activeAlpha;
    [SerializeField]
    private float _lerpDuration = 1f;

    private List<SouvenirSlot> _slots;
    private int _currentFreeSlot = 0;

    public event Action OnHoverOn;
    public event Action OnHoverOff;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var part in _cristopherParts)
        {
            part.color = new Color(part.color.r, part.color.g, part.color.b, _unactiveAlpha);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.G))
        {
            Instantiate(_prefab, _freeSlots[0]);
        }
    }

    public Transform GetDropZone()
    {
       
        return _freeSlots[_currentFreeSlot]; ;
    }
    private void OnMouseEnter()
    {
        Debug.Log("enter Cristophe");
        OnHoverOn?.Invoke();
    }
    private void OnMouseExit()
    {
        Debug.Log("exit Cristophe");
        OnHoverOff?.Invoke();
    }

    public void EquipSouvenir(int price)
    {
        for (int i = 0; i < price ; i++)
        {
            Color startColor = _cristopherParts[_currentFreeSlot + i].color;
            Color endColor = new Color(_cristopherParts[_currentFreeSlot +i].color.r, _cristopherParts[_currentFreeSlot + i].color.g, _cristopherParts[_currentFreeSlot + i].color.b, _activeAlpha);
            StartCoroutine(FadePart(_cristopherParts[_currentFreeSlot + i], startColor, endColor));
        }
        _currentFreeSlot += price;
    }
    public void UnequipSouvenir(int price)
    {
        for (int i = 0; i < price; i++)
        {
            Color startColor = _cristopherParts[_currentFreeSlot - i - 1].color;
            Color endColor = new Color(_cristopherParts[_currentFreeSlot - i - 1].color.r, _cristopherParts[_currentFreeSlot - i - 1].color.g, _cristopherParts[_currentFreeSlot - i - 1].color.b, _unactiveAlpha);
            StartCoroutine(FadePart(_cristopherParts[_currentFreeSlot - i - 1], startColor, endColor));
        }
        _currentFreeSlot -= price;
    }
    private IEnumerator FadePart(SpriteRenderer spriteToFade, Color startValue, Color endValue)
    {
        float time = 0;

        while (time < _lerpDuration)
        {
            spriteToFade.color = Color.Lerp(startValue, endValue, time / _lerpDuration);
            time += Time.deltaTime;
            yield return null;
        }
        spriteToFade.color = endValue;
    }

}
