using System.Collections;
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
        index = originalTexte.Contains('>') ? originalTexte.LastIndexOf('>') + 1 : originalTexte.IndexOf(':');
        if(index == -1)
        {
            index = 0;
        }
        TextAttached.text = originalTexte.Substring(0, index);
        StartCoroutine(ShowLetterByLetter());
    }

    IEnumerator ShowLetterByLetter()
    {
        for(int i=index; i <= originalTexte.Length; i+=2)
        {
            TextAttached.text = originalTexte.Substring(0, i);
            if(i+2 > originalTexte.Length)
                i= originalTexte.Length-2;
            yield return new WaitForSeconds(delai);
        }
    }
}
