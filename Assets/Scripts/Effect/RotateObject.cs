using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 0;
    [SerializeField]
    private float _scaleUpDuration = 2;

    private bool _rotate;
    // Start is called before the first frame update
    void Start()
    {
        _rotate = false;
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (_rotate)
            transform.Rotate(0f,0f,_rotationSpeed);
    }
    public void StartRotate()
    {
        StartCoroutine(ScaleUp());
        _rotate = true;
    }
    public void StopRotate()
    {
        _rotate = false;
    }

    private IEnumerator ScaleUp()
    {
        float time = 0;
        while (time <_scaleUpDuration)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, time / _scaleUpDuration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localScale = Vector3.one;
    }
}
