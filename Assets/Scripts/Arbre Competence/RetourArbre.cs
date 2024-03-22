using System;
using UnityEngine;

public class RetourArbre : MonoBehaviour
{
    public Action Act;

    public void ClickAction()
    {
        Act();
    }
}
