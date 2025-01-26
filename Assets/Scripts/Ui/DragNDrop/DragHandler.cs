using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DragHandler : MonoBehaviour
{
    private enum DragState
    {
        None,
        Drag,
        Drop
    }
    [SerializeField]
    private GameObject _prefab;
    [SerializeField]
    private MenuStatManager _menuStatManager;
    [SerializeField]
    private GameObject _defaultPositionToDrop;
    [SerializeField]
    private Cristopher _targetZoneToDrop;

    private IDropZone _dropZone;
    private GameObject _target;
    private DraggableElement _draggedElement;
    private DragState _state;
    private bool _isOverDropZone = false;

    private void OnEnable()
    {
        DraggableElement.OnHoverDragElement += HoverDraggableElement;
        DraggableElement.OnDragElement += DragElement;
        _targetZoneToDrop.OnHoverOn += HoverEnterDropZone;
        _targetZoneToDrop.OnHoverOff += HoverExitDropZone;
    }
    private void OnDisable()
    {
        DraggableElement.OnHoverDragElement -= HoverDraggableElement;
        DraggableElement.OnDragElement -= DragElement;
        _targetZoneToDrop.OnHoverOn -= HoverEnterDropZone;
        _targetZoneToDrop.OnHoverOff -= HoverExitDropZone;
    }
    // Start is called before the first frame update
    void Start()
    {
        _state = DragState.None;
        _dropZone = _targetZoneToDrop.GetComponent<IDropZone>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_state == DragState.Drag)
        {
            _target.GetComponent<DraggableElement>().Drag();
            if (Input.GetMouseButtonUp(0))
            {
                _state = DragState.Drop;
                Drop();
            }
        }


        if (Input.GetKeyDown(KeyCode.J))
        {
            Instantiate(_prefab, Input.mousePosition, Quaternion.identity);
        }
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Drag");
        }

    }

    public void Drop()
    {
        if (_isOverDropZone && _menuStatManager.Equiped(_draggedElement.GetComponent<SouvenirUI>()))
        {
            Debug.Log("Equip");
           // _targetZoneToDrop.ExitDropZone();
            Transform tr = _dropZone.GetDropZone();
            _targetZoneToDrop.GetComponent<Collider2D>().enabled = false;
            _draggedElement.transform.SetParent(tr);
            _draggedElement.transform.localPosition = Vector3.zero;
            //equip
            _dropZone.EquipSouvenir(_draggedElement.GetComponent<SouvenirUI>());
        }
        else
        {
            if (_draggedElement.GetComponent<SouvenirUI>().LeSouvenir.Equiped)
            {
                if (_menuStatManager.UnEquiped(_draggedElement.GetComponent<SouvenirUI>()))
                {
                    Debug.Log("Unequip");
                    _dropZone.UnequipSouvenir(_draggedElement.GetComponent<SouvenirUI>());

                }
            }
            _draggedElement.transform.SetParent(_defaultPositionToDrop.transform);
        }
        _draggedElement.Drop();
        _draggedElement.gameObject.GetComponent<Collider2D>().enabled = true;
        _state = DragState.None;
    }
    public void HoverDraggableElement(GameObject target)
    {
        if (_state == DragState.None)
        { 
            Debug.Log("Drag element : " +  target);
            _target = target; 
        }
    }
    public void HoverEnterDropZone()
    {
        _isOverDropZone = true;
        //if (_state == DragState.Drag)             //TODO: Non concluant, mieux à faire
        //{
        //    _targetZoneToDrop.EnterDropZone();
        //}
    }
    public void HoverExitDropZone()
    {
        _isOverDropZone = false;
        //if (_state == DragState.Drag)
        //{
        //    _targetZoneToDrop.ExitDropZone();
        //}
    }
    public void DragElement()
    {
        switch (_state)
        {
            case DragState.None:
                _state = DragState.Drag;
                _targetZoneToDrop.GetComponent<Collider2D>().enabled = true;
                _draggedElement = _target.GetComponent<DraggableElement>();
                _target.gameObject.GetComponent<Collider2D>().enabled = false;
                _target.transform.SetParent(_target.transform.root);
               
                break;
        }
    }
}
