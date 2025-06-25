using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsSettings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var resolution = Screen.currentResolution;
        string mode = Screen.fullScreen ? "Fullscreen" : "windowed";
        Debug.Log("Current resolution : " + resolution.width + "x" + resolution.height + " in " + mode + " mode.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
