using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPanel : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;



    private void OnEnable()
    {
        GameManager.OnHideMap += Hide;
        GameManager.OnShowMap += Show;
    }
    private void OnDisable()
    {
        GameManager.OnHideMap -= Hide;
        GameManager.OnShowMap -= Show;
    }



    public void Hide()
    {
        _animator.SetTrigger("Hide");
    }
    public void Show()
    {
        _animator.SetTrigger("Show");
    }
}
