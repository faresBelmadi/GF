using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;

public class ToolTipSystem : MonoBehaviour
{
    [SerializeField] private GameObject[] instantOffObjects;
    [SerializeField] private GameObject[] instantActivationObjects;
    [SerializeField] private GameObject[] delayedOffObjects;
    [SerializeField] private GameObject[] delayedActivationObjects;

    [SerializeField] float delay;

    Coroutine startDelayCoroutine = null;
    // Start is called before the first frame update
    public void CursorEnter()
    {

        // Shgow and hide instant tooltips
        TriggerInstant(true);

        if(startDelayCoroutine == null)
        {
            startDelayCoroutine = StartCoroutine(EnterRoutine());
        }
    }

    private void TriggerInstant(bool isEnter)
    {
        foreach (GameObject thisGO in instantOffObjects)
        {
            thisGO.SetActive(!isEnter);
        }
        foreach (GameObject thisGO in instantActivationObjects)
        {
            thisGO.SetActive(isEnter);
        }
    }
    private void TriggerDelayed(bool isEnter)
    {
        foreach (GameObject thisGO in delayedOffObjects)
        {
            thisGO.SetActive(!isEnter);
        }
        foreach (GameObject thisGO in delayedActivationObjects)
        {
            thisGO.SetActive(isEnter);
        }
    }

    public void CursorExit()
    {
        if(startDelayCoroutine != null)
        {
            StopCoroutine(startDelayCoroutine);
        }
        startDelayCoroutine = null;
        TriggerInstant(false);
        TriggerDelayed(false);
    }

    IEnumerator EnterRoutine () 
    {
        yield return new WaitForSeconds(delay);
        TriggerDelayed(true);
        startDelayCoroutine = null;
    }
}
