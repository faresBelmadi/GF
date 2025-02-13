using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabOptionButton : MonoBehaviour
{
    [SerializeField]
    private GameObject _select;
    [SerializeField]
    private GameObject _gray;

   
    public void Select()
    {
        _select.SetActive(true);
        _gray.SetActive(false);
        
    }
    public void Unselect()
    {
        _select.SetActive(false);
        _gray.SetActive(true);
    }

}
