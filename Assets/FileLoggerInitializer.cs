using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileLoggerInitializer : MonoBehaviour
{

    void Awake()
    {
        FileLogger.Init();
        FileLogger.Debug("TEST log system", LogSystem.Stats);
        FileLogger.Error("Test Osef");
        Debug.Log("Test via le unity");
    }
}
