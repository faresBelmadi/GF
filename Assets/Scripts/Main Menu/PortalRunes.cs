using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalRunes : MonoBehaviour
{
    [SerializeField]
    private PulseBloom_System _pulseSystem;

    // Start is called before the first frame update
    void Start()
    {
        _pulseSystem.bloomMaterial = GetComponent<SpriteRenderer>().material;
    }

    public void StartBloom()
    {
        _pulseSystem.TriggerBloom();
    }
}
