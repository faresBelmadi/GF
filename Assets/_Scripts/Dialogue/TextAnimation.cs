using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextAnimation : MonoBehaviour
{
    TextMeshProUGUI TextAttached;
    Animation toPlay;
    private void OnEnable() {
        TextAttached = transform.GetComponent<TextMeshProUGUI>();
        toPlay = transform.GetComponent<Animation>();
    }

    public void LaunchAnim()
    {
        toPlay.Play();
    }
}
