using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AppVersion : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _text;
    // Start is called before the first frame update
    void Start()
    {
        _text.text = $"Build {Application.version}";
    }
}
