using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonComponent : MonoBehaviour
{
    public void Clic()
    {
        AudioManager.instance.SFX.PlaySFXClip(SFXType.ClicButtonSFX);
    }
}
