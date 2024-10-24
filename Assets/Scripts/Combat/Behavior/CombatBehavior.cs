﻿using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatBehavior : MonoBehaviour
{
    [SerializeField] Sprite[] buffsSprites;  
    public List<GameObject> ListBuffDebuffGO = new List<GameObject>();
    public GameObject BuffPrefab;
    public Transform BuffContainer;
    public Transform DebuffContainer;
    [SerializeField]
    private Color _debuffTextColor = Color.red;
    [SerializeField]
    private Color _buffTextColor = Color.green;

    public Action EndTurnBM;
    public static Action OnUpdateUI;

    public int LastDamageTaken;
    public bool gainedTension;



    public void AddBuffDebuff(BuffDebuff toAdd, CharacterStat characterStat)
    {
        AudioManager.instance.SFX.PlaySFXClip(SFXType.BuffTriggerSFX);
        string[] buffDebuffInfos = GetBuffNameAndDescription(toAdd);
        string buffDebuffName = buffDebuffInfos[0];
        string buffDebuffDescription = buffDebuffInfos[1];

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
            buffObject.GetComponent<BuffDebuffComponant>().buffCntHolder.GetComponent<EnflateSystem>().TriggerInflation();
            //buffObject.GetComponent<BuffDebuffComponant>().buffTimeLabel.text = toAdd.Temps.ToString();
        }
        else
        {
            buffObject = Instantiate(BuffPrefab, toAdd.IsDebuff? DebuffContainer.transform : BuffContainer.transform);
            ControlBuffBarsSize();
            BuffDebuffComponant buffComp = buffObject.GetComponent<BuffDebuffComponant>();
            //buffComp.buffSprite.sprite = CorrespondingSprite
            //TEMP
            buffComp.buffSprite.sprite = buffsSprites[toAdd.IsDebuff ? 1 : 0];

            buffComp.buffName = buffDebuffName;
            buffComp.buffNameLabel.text = buffDebuffName;
            buffComp.buffCntLabel.text = "1";
            //buffComp.buffTimeLabel.text = toAdd.Temps.ToString();
            buffComp.buffDescriptionLabel.text = buffDebuffDescription;
            buffComp.buffDescriptionLabel.color = toAdd.IsDebuff ? _debuffTextColor : _buffTextColor;
            buffComp.buffCntHolder.GetComponent<EnflateSystem>().TriggerInflation();
            buffComp.InitBuffDebuff(toAdd);
            ListBuffDebuffGO.Add(buffObject);
        }
        //buffObject.GetComponent<EnflateSystem>().TriggerInflation();
    }
    private void ControlBuffBarsSize()
    {
        if (ListBuffDebuffGO.Count <= 0) return;

        float limit = 400f;
        float buffHeight = ListBuffDebuffGO[0].GetComponent<RectTransform>().rect.height;
        int buffCnt = BuffContainer.childCount - 1;
        int deBuffCnt = DebuffContainer.childCount - 1;
       // Debug.Log($"Limit:{limit}\nBuffHeight:{buffHeight}\nBuffCnt: {buffCnt}");
        if ((buffCnt * buffHeight) > limit)
        {
            BuffContainer.GetComponent<VerticalLayoutGroup>().spacing = -limit*(1-limit/(buffCnt*buffHeight))/buffCnt;
        }
        else
        {
            BuffContainer.GetComponent<VerticalLayoutGroup>().spacing = 3;
        }
        if ((deBuffCnt * buffHeight) > limit)
        {
            DebuffContainer.GetComponent<VerticalLayoutGroup>().spacing = -limit * (1 - limit / (deBuffCnt * buffHeight)) / deBuffCnt;
        }
        else
        {
            DebuffContainer.GetComponent<VerticalLayoutGroup>().spacing = 3;
        }
    }
    private string[] GetBuffNameAndDescription(BuffDebuff buff)
    {
        string buffDebuffName;
        string buffDebuffDescription;
        if (!string.IsNullOrEmpty(buff.idTradName) && !string.IsNullOrEmpty(buff.idTradDescription))
        {
            //if (TradManager.Instance.CapaDictionary.TryGetValue(buff.idTradName,
            //        out List<string> capaNameAllLangueList) &&
            //    TradManager.Instance.CapaDictionary.TryGetValue(buff.idTradDescription,
            //        out List<string> capaDescAllLangueList)
            //    && TradManager.Instance.IdLanguage != -1000)
            //{
            //    buffDebuffName = capaNameAllLangueList[TradManager.Instance.IdLanguage];
            //    buffDebuffDescription = capaDescAllLangueList[TradManager.Instance.IdLanguage];
            //}
            //else
            //{
            //    if (!TradManager.Instance.CapaDictionary.TryGetValue(buff.idTradName,
            //            out List<string> osef))
            //        Debug.Log("idTradName not in dictionary");
            //    if (!TradManager.Instance.CapaDictionary.TryGetValue(buff.idTradDescription,
            //            out List<string> osef2))
            //        Debug.Log("idTradDescription not in dictionary");
            //    if (TradManager.Instance.IdLanguage == -1000)
            //        Debug.Log("IdLanguage not in dictionary");
            //    buffDebuffName = buff.name;
            //    buffDebuffDescription = buff.Description;
            //}
            buffDebuffName = TradManager.instance.GetTranslation(buff.idTradName);
            buffDebuffDescription = TradManager.instance.GetTranslation(buff.idTradDescription);

        }
        else
        {
            if (string.IsNullOrEmpty(buff.idTradName))
                Debug.Log("IdTradName est null/empty pour " + buff.name);
            if (string.IsNullOrEmpty(buff.idTradDescription))
                Debug.Log("idTradDescription est null/empty pour " + buff.name);
            buffDebuffName = buff.name;
            buffDebuffDescription = buff.Description;
        }
        return new string[2] { buffDebuffName, buffDebuffDescription };
    }
    
    public void DecompteDebuff(List<BuffDebuff> BuffDebuff, Decompte Timer, CharacterStat toChange)
    {
        //Debug.Log($"Decompte Buffs: {Timer.ToString()}");
        foreach (var item in BuffDebuff)
        {
            if (item.Decompte == Timer) 
            {
                //Debug.Log($"Decompte {item.Nom} from {gameObject.name}");

                item.Temps--;
                /*
                GameObject buffObject = null;
                foreach (GameObject presentBuffObject in ListBuffDebuffGO)
                {
                    if (presentBuffObject.GetComponent<BuffDebuffComponant>().buffName == GetBuffNameAndDescription(item)[0])
                    {
                        buffObject = presentBuffObject;
                        break;
                    }
                }
                if (buffObject)
                {
                    buffObject.GetComponent<BuffDebuffComponant>().buffTimeLabel.text = item.Temps.ToString();
                    buffObject.GetComponent<BuffDebuffComponant>().buffTimeHolder.GetComponent<EnflateSystem>().TriggerInflation();
                }
                else
                {
                    Debug.Log("Buff Not Found");
                }
                */
            }


        }

    }

    protected void OnUpdate()
    {
        OnUpdateUI?.Invoke();
    }
    
    public List<BuffDebuff> UpdateBuffDebuffGameObject(List<BuffDebuff> ListBuffDebuff, CharacterStat toChange)
    {
        foreach (var item in ListBuffDebuff)
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
                    buffDebuffName = TradManager.instance.GetTranslation(item.idTradName, item.name);
                }
                else
                {
                    buffDebuffName = item.name;
                }

                GameObject buffObject = null;
                foreach (GameObject presentBuffObject in ListBuffDebuffGO)
                {
                    if (presentBuffObject.GetComponent<BuffDebuffComponant>().buffName == GetBuffNameAndDescription(item)[0])
                    {
                        buffObject = presentBuffObject;
                        break;
                    }
                }
                if (buffObject)
                {
                    BuffDebuffComponant buffComponant = buffObject.GetComponent<BuffDebuffComponant>();
                    //VERY DIRTY
                    int buffCnt = int.Parse(buffComponant.buffCntLabel.text);
                    buffCnt--;
                    if(buffCnt > 0)
                    {
                        buffComponant.buffCntLabel.text = buffCnt.ToString();
                        buffComponant.buffCntHolder.GetComponent<EnflateSystem>().TriggerInflation();
                    }
                    else
                    {
                        AudioManager.instance.SFX.PlaySFXClip(SFXType.BuffDisapearSFX);
                        ListBuffDebuffGO.Remove(buffObject);
                        Destroy(buffObject);
                    }
                }
                else
                {
                    Debug.Log("ERROR: Buff Not Found");
                }
                /*
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
                */
            }
        }

        ListBuffDebuff.RemoveAll(c => c.Temps < 0);
        return ListBuffDebuff;
    }
}