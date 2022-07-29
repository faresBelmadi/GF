using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuStatManager : MonoBehaviour
{
    public void End()
    {
        StartCoroutine(GameManager.instance.pmm.EndMenuStat());
    }
}
