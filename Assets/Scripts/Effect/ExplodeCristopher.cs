using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeCristopher : MonoBehaviour
{
    [SerializeField]
    private Transform _centerOfExplosion;
    [SerializeField]
    private List<Rigidbody2D> _parts;
    [SerializeField]
    private float _explosionForce;
    [SerializeField]
    private float _torqueForce;
   
    [SerializeField]
    private float _intensity;
    [SerializeField]
    private float _speed;
    [field: SerializeField]
    public float VibrationDuration { get; private set; }


    public void DestroyCristopher()
    {
        StartCoroutine(Vibrate());
    }
    private IEnumerator Vibrate()
    {
        float currentTime = 0;
        while (currentTime < VibrationDuration)
        {
            float intensity = Mathf.Lerp(0, _intensity, currentTime / VibrationDuration);
            transform.localPosition = intensity * new Vector3(
                    Mathf.PerlinNoise(_speed * Time.time, 1),
                    Mathf.PerlinNoise(_speed * Time.time, 2),
                    Mathf.PerlinNoise(_speed * Time.time, 3));
            currentTime += Time.deltaTime;
            yield return null;

        }
        transform.localPosition = Vector3.zero;
        Explode();
        Destroy(gameObject, 10f);

    }

    private void Explode()
    {
        AudioManager.instance.SFX.PlaySFXClip(SFXType.EssenceConsuptionSFX);
        foreach (var part in _parts)
        {
            part.simulated = true;
            part.AddForce((part.transform.position - _centerOfExplosion.position) * _explosionForce, ForceMode2D.Impulse);
            float value = _torqueForce * Random.Range(-1f, 1f);
            part.AddTorque(value, ForceMode2D.Impulse);
        }
    }
}
