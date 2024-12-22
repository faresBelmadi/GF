using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PulseBloom_System))]
public class PlateauEffect : MonoBehaviour
{
    [SerializeField]
    private Material _sourceMaterial;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    
    private PulseBloom_System _bloomSystem;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer.material = new Material(_sourceMaterial);
        _bloomSystem = GetComponent<PulseBloom_System>();
    }

    public void StartBloom()
    {
        _bloomSystem.bloomMaterial = _spriteRenderer.material;
        _bloomSystem.TriggerBloom(true);
    }
    public void StopBloom()
    {
        _bloomSystem.StopAllCoroutines();
        _spriteRenderer.material = new Material(_sourceMaterial);
    }

}
