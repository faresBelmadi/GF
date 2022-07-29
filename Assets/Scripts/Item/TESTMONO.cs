using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TESTMONO : MonoBehaviour
{
    public TESTSO test;
    public TESTSO testClone;

    void Start()
    {
        testClone = Instantiate(test);

        print(test.A);
        print(test.B(3));
        print(test.B(-3));
    }
}
