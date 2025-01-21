using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDropZone
{
    /// <summary>
    /// Get the position to drop the element
    /// </summary>
    /// <returns>The transform where to drop</returns>
    Transform GetDropZone();
    void EquipSouvenir(int price);
    void UnequipSouvenir(int price);
}
