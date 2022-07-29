using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New dialogue", menuName = "crée test", order = 11)]
public class TESTSO : ScriptableObject
{
    int a = 10;
    int[] b = new int[5] { 0, 17, 34, 42, 67 };

    public int A
    {
        get { return a; }
    }

    // return value in b array, or -1 if x is out-of-range
    public int B(int x)
    {
        if (x >= 0 && x <= 5)
            return b[x];
        else
            return -1;
    }

    public void Awake()
    {
        Debug.Log("Awake");
    }

    public void OnEnable()
    {
        Debug.Log("OnEnable");
    }

    public void OnDisable()
    {
        Debug.Log("OnDisable");
    }

    public void OnDestroy()
    {
        Debug.Log("OnDestroy");
    }
}
