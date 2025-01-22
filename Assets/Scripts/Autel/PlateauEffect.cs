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
    private void OnEnable()
    {
        _spriteRenderer.material = new Material(_sourceMaterial);
        _bloomSystem = GetComponent<PulseBloom_System>();

        StartBloom();
        _spriteRenderer.color = Color.white;
    }
    private void OnDisable()
    {
        StopBloom();
    }
    // Start is called before the first frame update
    void Start()
    {
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
    public void HoverOn()
    {
        _spriteRenderer.color = Color.gray;
    }
    public void HoverOff()
    {
        _spriteRenderer.color = Color.white;
    }
}
