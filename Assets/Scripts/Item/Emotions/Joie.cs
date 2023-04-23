using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering;
using Debug = UnityEngine.Debug;

public class Joie : Emotion
{
    public Joie()
    {
        EmotionName = "Joie";
        EmotionTypeEnum = EmotionTypeEnum.Joie;
        //GameManager.instance.playerStat.ListBuffDebuff.ItemAdded += GameManager.instance.EmotionMan.BuffDebuffAddedEventTriggered;
        //GameManager.instance.playerStat.ListBuffDebuff.ItemRemoved += GameManager.instance.EmotionMan.ListBuffDebuff_ItemRemoved; // Ou alors on passe par unity event comme pour fierte
    }
}
