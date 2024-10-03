using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPanel : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
            Show();
        }

        if (Input.GetKeyUp(KeyCode.H))
        {
            Hide();
        }

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
