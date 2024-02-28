using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatBehavior : MonoBehaviour
{

    public List<GameObject> ListBuffDebuffGO = new List<GameObject>();
    public GameObject BuffPrefab;
    public Transform BuffContainer;
    public GameObject DebuffPrefab;
    public Transform DebuffContainer;

    public Action EndTurnBM;

    public int LastDamageTaken;
    public bool gainedTension;

    public void AddBuffDebuff(BuffDebuff toAdd, CharacterStat characterStat)
    {
        if (toAdd.IsDebuff)
        {
            var t = ListBuffDebuffGO.FirstOrDefault(c => c.GetComponentInChildren<TextMeshProUGUI>().text == toAdd.Nom);
            if (t == null)
            {
                t = Instantiate(DebuffPrefab, DebuffContainer.transform);
                t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNom").text = toAdd.Nom;
                t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNb").text = "1";
                t.GetComponent<DescriptionHoverTriggerBuffDebuff>().Description.text = toAdd.Description;
                ListBuffDebuffGO.Add(t);
            }
            else
            {
                var test = characterStat.ListBuffDebuff.Count(x => x.Nom == toAdd.Nom);
                t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNb").text = test.ToString();
            }

        }
        else
        {
            var t = ListBuffDebuffGO.FirstOrDefault(c => c.GetComponentInChildren<TextMeshProUGUI>().text == toAdd.Nom);
            if (t == null)
            {
                t = Instantiate(BuffPrefab, BuffContainer.transform);
                t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNom").text = toAdd.Nom;
                t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNb").text = "1";
                t.GetComponent<DescriptionHoverTriggerBuffDebuff>().Description.text = toAdd.Description;
                ListBuffDebuffGO.Add(t);
            }
            else
            {
                var test = characterStat.ListBuffDebuff.Count(x => x.Nom == toAdd.Nom);
                t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNb").text = test.ToString();
            }
        }
    }

    public void DecompteDebuff(List<BuffDebuff> BuffDebuff, Decompte Timer, CharacterStat toChange)
    {
        foreach (var item in BuffDebuff)
        {
            if (item.Decompte == Timer)
                item.Temps--;
        }

    }

    public List<BuffDebuff> UpdateBuffDebuffGameObject(List<BuffDebuff> BuffDebuff, CharacterStat toChange)
    {
        foreach (var item in BuffDebuff)
        {
            if (item.Temps < 0)
            {
                if (item.timerApplication == TimerApplication.Persistant)
                {
                    foreach (var effet in item.Effet)
                    {
                        if (effet.TypeEffet != TypeEffet.RadianceMax)
                            toChange.removeStat(effet.modifstate);
                        else
                        {
                            effet.modifstate.Radiance = Mathf.FloorToInt((effet.Pourcentage / 100f) * toChange.Radiance);
                            toChange.removeStat(effet.modifstate);
                        }
                    }
                }
                var t = ListBuffDebuffGO.FirstOrDefault(c => c.GetComponentInChildren<TextMeshProUGUI>().text == item.Nom);
                if (t != null)
                {
                    var s = t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNb").text;
                    int nb = int.Parse(s);
                    nb -= 1;
                    s = nb + "";
                    if (nb <= 0)
                    {
                        ListBuffDebuffGO.Remove(t);
                        GameObject.Destroy(t);
                    }
                    else
                        t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNb").text = s;
                }
            }
        }

        BuffDebuff.RemoveAll(c => c.Temps < 0);
        return BuffDebuff;
    }
}   
