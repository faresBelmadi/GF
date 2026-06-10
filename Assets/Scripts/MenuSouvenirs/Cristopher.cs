using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Cristopher : MonoBehaviour, IDropZone
{
    [SerializeField]
    private MenuStatManager _menuStatManager;
    [SerializeField]
    private List<Transform> _freeSlots;
    [SerializeField]
    private List<Image> _cristopherParts;
   
    private GameObject _prefab;
    [SerializeField]
    [Range(0f,1f)]
    private float _unactiveAlpha;
    [SerializeField]
    [Range(0f, 1f)]
    private float _hoverAlpha;
    [SerializeField]
    [Range(0f, 1f)]
    private float _activeAlpha;
    [SerializeField]
    private float _lerpDuration = 1f;

    private List<SouvenirSlot> _slots;
    private List<Image> _rendererSlots;
    
    [SerializeField]
    private int _currentFreeSlot = 0;

    private Dictionary<int, bool> _usedSlot;
    public event Action OnHoverOn;
    public event Action OnHoverOff;

    private void OnEnable()
    {
        InitCristopher();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.G))
        {
            Instantiate(_prefab, _freeSlots[0]);
        }
    }
    public void InitCristopher()
    {
        _currentFreeSlot = GameManager.Instance.playerStatHandler.ListSouvenir.Where(s=> s.Equiped).Sum(s => s.Slots);
        _usedSlot = new Dictionary<int, bool>();
        _rendererSlots = new List<Image>(_freeSlots.Count);

        for (int i = 0; i < _freeSlots.Count; i++)
        {
            _rendererSlots.Add(_freeSlots[i].gameObject.GetComponent<Image>());
            _rendererSlots[i].color = new Color(_rendererSlots[i].color.r, _rendererSlots[i].color.g, _rendererSlots[i].color.b, 0);
            _usedSlot.Add(i, false);
        }
        foreach (var part in _cristopherParts)
        {
            part.color = new Color(part.color.r, part.color.g, part.color.b, _unactiveAlpha);
        }
        //SetCurrentSlot(); //For random test
        if (!GameManager.Instance.IsTuto)
        {
            if (_currentFreeSlot < _rendererSlots.Count)
                _rendererSlots[_currentFreeSlot].color = new Color(_rendererSlots[_currentFreeSlot].color.r, _rendererSlots[_currentFreeSlot].color.g, _rendererSlots[_currentFreeSlot].color.b, 1);
        }
        for (int i = 0; i < _currentFreeSlot; i++)
        {
            Color startColor = _cristopherParts[i].color;
            Color endColor = new Color(_cristopherParts[i].color.r, _cristopherParts[i].color.g, _cristopherParts[i].color.b, _activeAlpha);
            StartCoroutine(FadePart(_cristopherParts[i], startColor, endColor));
        }
    }
    public void ActivateCurrentSlot()
    {
        ActivateSlot(_currentFreeSlot);
    }
    private void SetCurrentSlot()
    {
        var listFreeSlot = _usedSlot.Where(x => x.Value == false).ToList();
        int slot = UnityEngine.Random.Range(0, listFreeSlot.Count);
        _currentFreeSlot = listFreeSlot[slot].Key;
    }
    public Transform GetDropZone()
    {
        return _currentFreeSlot<_freeSlots.Count?_freeSlots[_currentFreeSlot]: _freeSlots[_freeSlots.Count-1];
    }
    private void OnMouseEnter()
    {
        OnHoverOn?.Invoke();
    }
    private void OnMouseExit()
    {
        OnHoverOff?.Invoke();
    }
    private void ActivateSlot(int slotID)
    {
        foreach (var rendererSlots in _rendererSlots)
        {
            rendererSlots.color = new Color(rendererSlots.color.r, rendererSlots.color.g, rendererSlots.color.b, 0);
        }
        if (slotID < _rendererSlots.Count)
            _rendererSlots[slotID].color = new Color(_rendererSlots[slotID].color.r, _rendererSlots[slotID].color.g, _rendererSlots[slotID].color.b, 1);
    }
    public void EquipSouvenir(SouvenirUI souvenir)
    {
        for (int i = 0; i < souvenir.LeSouvenir.Slots; i++)
        {
            Color startColor = _cristopherParts[_currentFreeSlot + i].color;
            Color endColor = new Color(_cristopherParts[_currentFreeSlot +i].color.r, _cristopherParts[_currentFreeSlot + i].color.g, _cristopherParts[_currentFreeSlot + i].color.b, _activeAlpha);
            StartCoroutine(FadePart(_cristopherParts[_currentFreeSlot + i], startColor, endColor));
        }
        _currentFreeSlot += souvenir.LeSouvenir.Slots;
        ActivateSlot(_currentFreeSlot);
    }
    private void LightCristopher(int slot, float alpha, bool useSlot)
    {
            Color startColor = _cristopherParts[_currentFreeSlot].color;
            Color endColor = new Color(_cristopherParts[_currentFreeSlot].color.r, _cristopherParts[_currentFreeSlot].color.g, _cristopherParts[_currentFreeSlot].color.b, alpha);
        _usedSlot[slot] = useSlot;
            StartCoroutine(FadePart(_cristopherParts[_currentFreeSlot], startColor, endColor));

    }
    public void EquipSouvenirRandom(SouvenirUI souvenir) // random version
    {
        for (int i =0; i< souvenir.LeSouvenir.Slots; i++)
        {
            LightCristopher(_currentFreeSlot, _activeAlpha, true);
            SetCurrentSlot();
        }
        ActivateSlot(_currentFreeSlot);
    }
    public void UnequipSouvenir(SouvenirUI souvenir)
    {
        for (int i = 0; i < souvenir.LeSouvenir.Slots; i++)
        {
            Color startColor = _cristopherParts[_currentFreeSlot - i - 1].color;
            Color endColor = new Color(_cristopherParts[_currentFreeSlot - i - 1].color.r, _cristopherParts[_currentFreeSlot - i - 1].color.g, _cristopherParts[_currentFreeSlot - i - 1].color.b, _unactiveAlpha);
            StartCoroutine(FadePart(_cristopherParts[_currentFreeSlot - i - 1], startColor, endColor));
        }
        _currentFreeSlot -= souvenir.LeSouvenir.Slots;
        ActivateSlot(_currentFreeSlot);
        RearrangeSouvenir();
    }
    public void UnequipSouvenirRandom(SouvenirUI souvenir) //random test
    {
        for (int i = 0; i < souvenir.LeSouvenir.Slots; i++)
        {
            Color startColor = _cristopherParts[_currentFreeSlot - i - 1].color;
            Color endColor = new Color(_cristopherParts[_currentFreeSlot - i - 1].color.r, _cristopherParts[_currentFreeSlot - i - 1].color.g, _cristopherParts[_currentFreeSlot - i - 1].color.b, _unactiveAlpha);
            StartCoroutine(FadePart(_cristopherParts[_currentFreeSlot - i - 1], startColor, endColor));
        }
        _currentFreeSlot -= souvenir.LeSouvenir.Slots;
        ActivateSlot(_currentFreeSlot);
        RearrangeSouvenir();
    }
    public void RearrangeSouvenir()
    {
        int tempSlot = 0;
        foreach (var souvenirUI in _menuStatManager.ListSouvenirUIEquipped)
        {
            souvenirUI.transform.SetParent(_freeSlots[tempSlot]);
            souvenirUI.transform.localPosition = Vector3.zero;
            tempSlot += souvenirUI.LeSouvenir.Slots;
        }
    }
    private IEnumerator FadePart(Image spriteToFade, Color startValue, Color endValue)
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

    public void EnterDropZone()
    {
        for (int i = _currentFreeSlot; i < _cristopherParts.Count; i++)
        {
            Color startColor = _cristopherParts[i].color;
            Color endColor = new Color(_cristopherParts[i].color.r, _cristopherParts[i].color.g, _cristopherParts[i].color.b, _hoverAlpha);
            StartCoroutine(FadePart(_cristopherParts[i], startColor, endColor));
            //_cristopherParts[i].color = endColor;
        }
    }

    public void ExitDropZone()
    {
        for (int i = _currentFreeSlot; i < _cristopherParts.Count; i++)
        {
            Color startColor = _cristopherParts[i].color;
            Color endColor = new Color(_cristopherParts[i].color.r, _cristopherParts[i].color.g, _cristopherParts[i].color.b, _unactiveAlpha);
            //StartCoroutine(FadePart(_cristopherParts[i], startColor, endColor));
            _cristopherParts[i].color = endColor;
        }
    }
}
