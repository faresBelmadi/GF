using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextAnimDegats : MonoBehaviour
{
    public bool isSoin;
    public int Value
    {
        get
        {
            return _value;
        }

        set
        {
            _value = value;
            StartAnim();
        }
    }
    private int _value;

    private void StartAnim()
    {
        string t;
        if(isSoin)
            t = "+" + Mathf.Abs(_value);
        else
            t = "-" + Mathf.Abs(_value);
        this.GetComponentInChildren<TextMeshProUGUI>().text = t;
        this.GetComponent<Animator>().SetBool("CanRun",true);

        Invoke("DestroyGO",5f); 
    }

    private void DestroyGO()
    {
        Destroy(this.gameObject);
    }
}
