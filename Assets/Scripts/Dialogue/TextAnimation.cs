using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextAnimation : MonoBehaviour
{
    string originalTexte;
    TextMeshProUGUI TextAttached;
    int index;
    public float delai = 0.02f;

    private void OnEnable() {
        TextAttached = transform.GetComponent<TextMeshProUGUI>();
        originalTexte = TextAttached.text;
        index = originalTexte.IndexOf(':');
        if(index == -1)
        {
            index = 0;
        }
        TextAttached.text = originalTexte.Substring(0, index);
        StartCoroutine(ShowLetterByLetter());
    }

    IEnumerator ShowLetterByLetter()
    {
        for(int i=index; i <= originalTexte.Length; i++)
        {
            TextAttached.text = originalTexte.Substring(0, i);
            yield return new WaitForSeconds(delai);
        }
    }
}
