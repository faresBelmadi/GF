using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiMondeManager : MonoBehaviour
{
    public GameObject UiMondeHolder;
    public GameObject UiSkillTreeHolder;
    public GameObject UiStatHolder;

    public void EnableSkillTree()
    {
        UiMondeHolder.SetActive(false);
        UiSkillTreeHolder.SetActive(true);
    }

    public void EnableStat()
    {
        UiMondeHolder.SetActive(false);
        UiStatHolder.SetActive(true);
    }

    public void EnableMonde()
    {
        UiMondeHolder.SetActive(true);
        UiSkillTreeHolder.SetActive(false);
        UiStatHolder.SetActive(false);
    }
}
