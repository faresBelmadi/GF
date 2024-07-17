using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnflateSystem : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] float amplitude;
    [SerializeField] AnimationCurve curve;

    private Vector3 baseLocalScale;
    private Coroutine inflateRoutine = null;
    private void Awake()
    {
        baseLocalScale = transform.localScale;
    }

    public void TriggerInflation()
    {
        if( inflateRoutine != null ) StopCoroutine( inflateRoutine );
        inflateRoutine = StartCoroutine(InflateCoroutine());
    }

    IEnumerator InflateCoroutine()
    {
        float time = 0f;
        while (time < duration)
        {
            yield return null;
            time += Time.deltaTime;
            transform.localScale = baseLocalScale +(Vector3.one * curve.Evaluate(time/duration) * amplitude);
        }
        transform.localScale = baseLocalScale;
        inflateRoutine = null;
    }
}
