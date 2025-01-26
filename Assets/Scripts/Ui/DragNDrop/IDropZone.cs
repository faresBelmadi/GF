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
    void EquipSouvenir(SouvenirUI souvenir);
    void UnequipSouvenir(SouvenirUI souvenir);
    void EnterDropZone();
    void ExitDropZone();
}
