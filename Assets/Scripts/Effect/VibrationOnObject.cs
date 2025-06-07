using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationOnObject : MonoBehaviour
{
    [SerializeField] float speed = 5.0f;
    [SerializeField] float intensity = 0.1f;

    [field: SerializeField] public bool IsVibrating;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsVibrating)
        {
            transform.localPosition = intensity * new Vector3(
                Mathf.PerlinNoise(speed * Time.time, 1),
                Mathf.PerlinNoise(speed * Time.time, 2),
                Mathf.PerlinNoise(speed * Time.time, 3));
        }
    }
}
