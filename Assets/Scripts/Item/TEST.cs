using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    public Souvenir souv;
    public Souvenir souvClone;

    public void Start()
    {
        souvClone = Instantiate(souv);
    }
}
