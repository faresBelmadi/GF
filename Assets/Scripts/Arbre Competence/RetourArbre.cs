using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetourArbre : MonoBehaviour
{
    public Action Act;

    public void ClickAction()
    {
        Act();
    }
}
