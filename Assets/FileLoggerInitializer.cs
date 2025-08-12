using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileLoggerInitializer : MonoBehaviour
{
    int index = 0;

    void Awake()
    {
        FileLogger.Init();
    }
}
