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
        if (!string.IsNullOrEmpty(toAdd.idTradName) && !string.IsNullOrEmpty(toAdd.idTradDescription))
        {
            if (TradManager.instance.CapaDictionary.TryGetValue(toAdd.idTradName,
                    out List<string> capaNameAllLangueList) &&
                TradManager.instance.CapaDictionary.TryGetValue(toAdd.idTradDescription,
                    out List<string> capaDescAllLangueList)
                && TradManager.instance.IdLanguage != -1000)
            {
                buffDebuffName = capaNameAllLangueList[TradManager.instance.IdLanguage];
                buffDebuffDescription = capaDescAllLangueList[TradManager.instance.IdLanguage];
            }
            else
            {
                if (!TradManager.instance.CapaDictionary.TryGetValue(toAdd.idTradName,
                        out List<string> osef))
                    Debug.Log("idTradName not in dictionary");
                if (!TradManager.instance.CapaDictionary.TryGetValue(toAdd.idTradDescription,
                        out List<string> osef2))
                    Debug.Log("idTradDescription not in dictionary");
                if (TradManager.instance.IdLanguage == -1000)
                    Debug.Log("IdLanguage not in dictionary");
                buffDebuffName = toAdd.name;
                buffDebuffDescription = toAdd.Description;
            }
        }
        else
        {
            if (string.IsNullOrEmpty(toAdd.idTradName))
                Debug.Log("IdTradName est null/empty pour " + toAdd.name);
            if (string.IsNullOrEmpty(toAdd.idTradDescription))
                Debug.Log("idTradDescription est null/empty pour " + toAdd.name);
            buffDebuffName = toAdd.name;
            buffDebuffDescription = toAdd.Description;
        }

        if (toAdd.IsDebuff)
        {
            GameObject buffObject = null;
            foreach (GameObject presentBuffObject in ListBuffDebuffGO)
            {
                if(presentBuffObject.GetComponent<BuffDebuffComponant>().buffName == buffDebuffName)
                {
                    buffObject = presentBuffObject;
                    break;
                }
            }
            if (buffObject)
            {
                int buffCnt = characterStat.ListBuffDebuff.Count(x => x.Nom == buffDebuffName);
                buffObject.GetComponent<BuffDebuffComponant>().buffCntLabel.text = buffCnt.ToString();
            }
            else
            {
                buffObject = Instantiate(DebuffPrefab, DebuffContainer.transform);
                BuffDebuffComponant buffComp = buffObject.GetComponent<BuffDebuffComponant>();
                buffComp.buffName = buffDebuffName;
                buffComp.buffNameLabel.text = buffDebuffName;
                buffComp.buffCntLabel.text = "1";
                buffComp.buffDescriptionLabel.text = buffDebuffDescription;
                ListBuffDebuffGO.Add(buffObject);
            }
            //OLD-----------------------------------------------------------------------------------------------------
            /*
            var t = ListBuffDebuffGO.FirstOrDefault(c =>
                c.GetComponentInChildren<TextMeshProUGUI>().text == buffDebuffName);
            if (t == null)
            {
                t = Instantiate(DebuffPrefab, DebuffContainer.transform);
                t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNom").text =
                    buffDebuffName;
                t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNb").text = "1";
                t.GetComponent<DescriptionHoverTriggerBuffDebuff>().Description.text = buffDebuffDescription;
                ListBuffDebuffGO.Add(t);
            }
            else
            {
                var NbBuffDebuff = characterStat.ListBuffDebuff.Count(x => x.Nom == buffDebuffName);
                t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNb").text =
                    NbBuffDebuff.ToString();
            }
            */

        }
        else
        {
            GameObject buffObject = null;
            foreach (GameObject presentBuffObject in ListBuffDebuffGO)
            {
                if (presentBuffObject.GetComponent<BuffDebuffComponant>().buffName == buffDebuffName)
                {
                    buffObject = presentBuffObject;
                    break;
                }
            }
            if (buffObject)
            {
                int buffCnt = characterStat.ListBuffDebuff.Count(x => x.Nom == buffDebuffName);
                buffObject.GetComponent<BuffDebuffComponant>().buffCntLabel.text = buffCnt.ToString();
            }
            else
            {
                buffObject = Instantiate(BuffPrefab, BuffContainer.transform);
                BuffDebuffComponant buffComp = buffObject.GetComponent<BuffDebuffComponant>();
                buffComp.buffName = buffDebuffName;
                buffComp.buffNameLabel.text = buffDebuffName;
                buffComp.buffCntLabel.text = "1";
                buffComp.buffDescriptionLabel.text = buffDebuffDescription;
                ListBuffDebuffGO.Add(buffObject);
            }
            //OLD-----------------------------------------------------------------------------------------------------
            /*
            var t = ListBuffDebuffGO.FirstOrDefault(c =>
                c.GetComponentInChildren<TextMeshProUGUI>().text == buffDebuffName);
            if (t == null)
            {
                t = Instantiate(BuffPrefab, BuffContainer.transform);
                t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNom").text =
                    buffDebuffName;
                t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNb").text = "1";
                t.GetComponent<DescriptionHoverTriggerBuffDebuff>().Description.text = buffDebuffDescription;
                ListBuffDebuffGO.Add(t);
            }
            else
            {
                var test = characterStat.ListBuffDebuff.Count(x => x.Nom == buffDebuffName);
                t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNb").text =
                    test.ToString();
            }
            */
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
                            effet.modifstate.Radiance =
                                Mathf.FloorToInt((effet.Pourcentage / 100f) * toChange.Radiance);
                            toChange.removeStat(effet.modifstate);
                        }
                    }
                }

                string buffDebuffName;
                if (item.idTradName != null)
                {
                    if (TradManager.instance.CapaDictionary.TryGetValue(item.idTradName,
                            out List<string> capaNameAllLanguageList))
                    {
                        buffDebuffName = capaNameAllLanguageList[TradManager.instance.IdLanguage];
                    }
                    else
                    {
                        buffDebuffName = item.name;
                    }
                }
                else
                {
                    buffDebuffName = item.name;
                }

                var t = ListBuffDebuffGO.FirstOrDefault(c =>
                    c.GetComponentInChildren<TextMeshProUGUI>().text == buffDebuffName);
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