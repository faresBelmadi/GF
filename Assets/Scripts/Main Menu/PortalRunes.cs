using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalRunes : MonoBehaviour
{
    [SerializeField]
    private PulseBloom_System _pulseSystem;

    private Material _initialMaterial;

    // Start is called before the first frame update
    void Start()
    {
        _initialMaterial = new Material( GetComponent<SpriteRenderer>().material);
    }

    public void StartBloom()
    {
        _pulseSystem.bloomMaterial = GetComponent<SpriteRenderer>().material;
        _pulseSystem.TriggerBloom();
    }
    public void StopBloom()
    {
        GetComponent<SpriteRenderer>().material = new Material( _initialMaterial);
        _pulseSystem.StopAllCoroutines();
    }
}
