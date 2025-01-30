using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 0;

    private bool _rotate;
    // Start is called before the first frame update
    void Start()
    {
        _rotate = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_rotate)
            transform.Rotate(0f,0f,_rotationSpeed);
    }
    public void StartRotate()
    {
        _rotate = true;
    }
    public void StopRotate()
    {
        _rotate = false;
    }
}
