using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

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
        string buffDebuffName;
        string buffDebuffDescription;
        if (!String.IsNullOrEmpty(toAdd.idTradName) && !String.IsNullOrEmpty(toAdd.idTradDescription))
        {
            Debug.Log("OUI");
            buffDebuffName = TradManager.instance.CapaDictionary[toAdd.idTradName][TradManager.instance.IdLanguage];
            buffDebuffDescription = TradManager.instance.CapaDictionary[toAdd.idTradDescription][TradManager.instance.IdLanguage];
        }
        else
        {
            Debug.Log("NON");
            buffDebuffName = toAdd.name;
            buffDebuffDescription = toAdd.Description;
        }
        if (toAdd.IsDebuff)
        {
            var t = ListBuffDebuffGO.FirstOrDefault(c => c.GetComponentInChildren<TextMeshProUGUI>().text == buffDebuffName);
            if (t == null)
            {
                t = Instantiate(DebuffPrefab, DebuffContainer.transform);
                t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNom").text = buffDebuffName;
                t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNb").text = "1";
                t.GetComponent<DescriptionHoverTriggerBuffDebuff>().Description.text = buffDebuffDescription;
                ListBuffDebuffGO.Add(t);
            }
            else
            {
                var NbBuffDebuff = characterStat.ListBuffDebuff.Count(x => x.Nom == buffDebuffName);
                t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNb").text = NbBuffDebuff.ToString();
            }

        }
        else
        {
            var t = ListBuffDebuffGO.FirstOrDefault(c => c.GetComponentInChildren<TextMeshProUGUI>().text == buffDebuffName);
            if (t == null)
            {
                t = Instantiate(BuffPrefab, BuffContainer.transform);
                t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNom").text = buffDebuffName;
                t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNb").text = "1";
                t.GetComponent<DescriptionHoverTriggerBuffDebuff>().Description.text = buffDebuffDescription;
                ListBuffDebuffGO.Add(t);
            }
            else
            {
                var test = characterStat.ListBuffDebuff.Count(x => x.Nom == buffDebuffName);
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
                string buffDebuffName;
                if (item.idTradName != null && item.idTradDescription != null)
                {
                    buffDebuffName = TradManager.instance.CapaDictionary[item.idTradName][TradManager.instance.IdLanguage];
                }
                else
                {
                    buffDebuffName = item.name;
                }
                var t = ListBuffDebuffGO.FirstOrDefault(c => c.GetComponentInChildren<TextMeshProUGUI>().text == buffDebuffName);
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
