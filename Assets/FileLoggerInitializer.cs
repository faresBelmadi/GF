using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileLoggerInitializer : MonoBehaviour
{
    int index = 0;

    void Awake()
    {
        FileLogger.Init();
        FileLogger.Info("Jeu démarré");
    }

    void Start()
    {
        FileLogger.Debug("TEST");
        Debug.Log(Application.persistentDataPath);
    }

    void Update()
    {
        if (index < 10)
        {
            FileLogger.Error(index.ToString());
            index++;
        }
    }

}
