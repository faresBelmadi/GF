using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class EnemyPassiveDescription : MonoBehaviour
{
    [SerializeField]
    private GameObject _tooltip;
    
    public void InitTooltip(string idLabel, string defaultDesc)
    {
        _tooltip.SetActive(true);
        _tooltip.GetComponent<TextComponent>().InitTextComponent(idLabel, defaultDesc);
    }
}
