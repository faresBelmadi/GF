using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField]
    private float _startingSpeed = 0;
    [SerializeField]
    private float _selectedSpeed = 0;
    [SerializeField]
    private float _scaleUpDuration = 2;
    [SerializeField]
    private Color _activeColor;

    private float _rotationSpeed;
    private bool _rotate;
    private SpriteRenderer _spriteRender;
    // Start is called before the first frame update
    void Start()
    {
        _rotate = true;
        _rotationSpeed = _startingSpeed;
        _spriteRender = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_rotate)
            transform.Rotate(0f, 0f, _rotationSpeed);
    }
    public void StartRotate()
    {
        StartCoroutine(SpeedUpRotation());
        StartCoroutine(ChangeColor());
        _rotate = true;
    }
    public void StopRotate()
    {
        _rotate = false;
    }

    private IEnumerator SpeedUpRotation()
    {
        Debug.Log("Speed");
        float time = 0;
        while (time < _scaleUpDuration)
        {
           _rotationSpeed = Mathf.Lerp(_startingSpeed, _selectedSpeed, time/_scaleUpDuration);
            time += Time.deltaTime;
            yield return null;
        }
        _rotationSpeed = _selectedSpeed;
    }
    private IEnumerator ChangeColor()
    {
        Debug.Log("Color)");
        float time = 0;
        while (time < _scaleUpDuration)
        {
            _spriteRender.material.color = Color.Lerp(Color.white, _activeColor, time / _scaleUpDuration);
            time += Time.deltaTime;
            yield return null;
        }
        _spriteRender.material.color = _activeColor;
    }
    private IEnumerator ScaleUp()
    {
        Debug.Log("Scale");
        float time = 0;
        while (time < _scaleUpDuration)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, time / _scaleUpDuration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localScale = Vector3.one;
    }
}
