using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public abstract class CombatBehavior<T> : MonoBehaviour where T : CharacterStat
{
    [SerializeField]
    protected T _stat;
    public virtual T Stat 
    {
        get => _stat;
        set { _stat = value; }
    }
    public List<GameObject> ListBuffDebuffGO = new List<GameObject>();
    public GameObject BuffPrefab;
    public Transform BuffContainer;
    public Transform DebuffContainer;
    [SerializeField]
    private Color _debuffTextColor = Color.red;
    [SerializeField]
    private Color _buffTextColor = Color.green;
    [SerializeField]
    protected List<TextComponent> _passiveTooltips;

    public Action EndTurnBM;
    public static Action OnUpdateUI;


    public int LastDamageTaken;
    public bool gainedTension;

    private Vector3 _startingPos;
    [field: SerializeField] public bool IsIntangible { get; protected set; } = false;

    [SerializeField] protected int nbBuffDebuffApplied;

    protected CommonStats commonStats;
    #region Events
    public event Action OnGainTensionLevel;
    public event Action OnTakeDamage;

    public List<AbstractPassive> PassiveList { get; protected set; }
    #endregion

    public virtual string Name { get => name; }

    private void Start()
    {
        _startingPos = transform.parent.position;
    }
    public void ClearBuffBar()
    {
        foreach (var buff in ListBuffDebuffGO)
        {
            Destroy(buff);
        }
        ListBuffDebuffGO.Clear();
    }

    protected float ValueConviction()
    {
        float value = 0f;
        
        switch(Stat.Conviction)
        {
            case 10:
                value = (Stat.Conviction + commonStats.MaxConvictionBonusValue) * commonStats.ConvictionValue / 100f;

                break;
            case -10:
                value = (Stat.Conviction - commonStats.MaxConvictionBonusValue) * commonStats.ConvictionValue / 100f;

                break;
            default:
                value = Stat.Conviction * commonStats.ConvictionValue / 100f;
                break;
        }
        return value;
    }
    protected BuffDebuff ApplyConviction(BuffDebuff toModify, float valueToApply)
    {
        BuffDebuff buff = toModify;

        if(nbBuffDebuffApplied >= commonStats.ConvictionNbBuffTrigger)
        {
            int positif = buff.IsDebuff ? -1 : 1;
            if (buff.IsDebuff && valueToApply < 0)
            {
                foreach (var effet in buff.Effet)
                {
                    int percentPositif = effet.Pourcentage > 0 ? 1 : -1;
                    int valuePositif = effet.ValeurBrut > 0 ? 1 : -1;
                    effet.Pourcentage += Mathf.FloorToInt(Mathf.Abs(effet.Pourcentage * valueToApply) *  percentPositif);

                    effet.ValeurBrut += Mathf.FloorToInt(Mathf.Abs(effet.ValeurBrut * valueToApply) *  valuePositif);
                }
            }
            else if (!buff.IsDebuff && valueToApply > 0)
            {
                foreach (var effet in buff.Effet)
                {
                    int percentPositif = effet.Pourcentage > 0 ? 1 : -1;
                    int valuePositif = effet.ValeurBrut > 0 ? 1 : -1;
                    effet.Pourcentage += Mathf.FloorToInt(Mathf.Abs(effet.Pourcentage * valueToApply) * percentPositif);

                    effet.ValeurBrut += Mathf.FloorToInt(Mathf.Abs(effet.ValeurBrut * valueToApply)  * valuePositif);
                }
            }
            nbBuffDebuffApplied %= commonStats.ConvictionNbBuffTrigger;
        }

        return buff;
    }

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
            int buffCnt = characterStat.ListBuffDebuff.Count(x => TradManager.instance.GetTranslation(x.idTradName, x.Nom) == buffDebuffName);
            buffObject.GetComponent<BuffDebuffComponant>().AddStack(toAdd);
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
            
            buffComp.buffSprite.sprite = toAdd.Icon;

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
            List<float> variableValues = new List<float>();


            foreach (Effet e in buff.Effet)
            {
                if (e.ValeurBrut != 0)
                    variableValues.Add(Mathf.Abs(e.ValeurBrut));
                if (e.Pourcentage != 0)
                    variableValues.Add(Mathf.Abs((float)e.Pourcentage));
            }
            
            buffDebuffName = TradManager.instance.GetTranslation(buff.idTradName, buff.Nom);
            buffDebuffDescription = TradManager.instance.GetTranslation(buff.idTradDescription, buff.Description,variableValues);

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
    
    public void DecompteDebuff(List<BuffDebuff> BuffDebuff, Decompte decompte, CharacterStat toChange)
    {
        //Debug.Log($"Decompte Buffs: {Timer.ToString()}");
        foreach (var item in BuffDebuff)
        {
            if (item.ConditionnalBuff == ConditionalBuff.NONE && item.Decompte == decompte && decompte != Decompte.none) 
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

    private void RemoveAllBuffDebuff()
    {
        foreach (var buff in Stat.ListBuffDebuff)
        {

            foreach (var effet in buff.Effet)
            {
                if (effet.TypeEffet != TypeEffet.RadianceMax)
                    Stat.removeStat(effet.modifstateOutput);
                else
                {
                    effet.modifstateOutput.Radiance =
                        Mathf.FloorToInt((effet.Pourcentage / 100f) * Stat.Radiance);
                    Stat.removeStat(effet.modifstateOutput);
                }
            }
        }
        for (int i = ListBuffDebuffGO.Count-1; i > -1 ; i--)
        {
            Destroy(ListBuffDebuffGO[i].gameObject);
        }
        ListBuffDebuffGO.Clear();
        Stat.ListBuffDebuff.Clear();

    }


    //TODO: a voir mieux
    public void RemoveBuffByIdTradName(string idTradName)
    {
        var ListBuff = Stat.ListBuffDebuff.FindAll(x => x.idTradName.Equals(idTradName));
        foreach(var buff in ListBuff)
        {
            
            foreach (var effet in buff.Effet)
            {
                if (effet.TypeEffet != TypeEffet.RadianceMax)
                    Stat.removeStat(effet.modifstateOutput);
                else
                {
                    effet.modifstateOutput.Radiance =
                        Mathf.FloorToInt((effet.Pourcentage / 100f) * Stat.Radiance);
                    Stat.removeStat(effet.modifstateOutput);
                }
            }
            string buffDebuffName;
            if (buff.idTradName != null)
            {
                buffDebuffName = TradManager.instance.GetTranslation(buff.idTradName, buff.name);
            }
            else
            {
                buffDebuffName = buff.name;
            }

            GameObject buffObject = null;
            foreach (GameObject presentBuffObject in ListBuffDebuffGO)
            {
                if (presentBuffObject.GetComponent<BuffDebuffComponant>().buffName == GetBuffNameAndDescription(buff)[0])
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
                buffComponant.RemoveNullStack();
                if (buffCnt > 0)
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
            Stat.ListBuffDebuff.Remove(buff);

        }
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
                            toChange.removeStat(effet.modifstateOutput);
                        else
                        {
                            effet.modifstateOutput.Radiance =
                                Mathf.FloorToInt((effet.Pourcentage / 100f) * toChange.Radiance);
                            toChange.removeStat(effet.modifstateOutput);
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
                    buffComponant.RemoveNullStack();
                    if (buffCnt > 0)
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
    public void ToggleVisibility(bool isVisible)
    {
        transform.parent.position = isVisible ? _startingPos : new Vector3(_startingPos.x, -10000f, _startingPos.z);
    }

    public void EnervementTension()
    {
        var t = (int)((Stat.Tension / (commonStats.NbPalier * Stat.ValeurPalier)) * commonStats.NbPalier);
        if (t >= commonStats.NbPalier)
            t = commonStats.NbPalier;
        else
            t++;

        Stat.Tension = t * Stat.ValeurPalier;
    }

    public void ApaisementTension()
    {

        var t = (int)((Stat.Tension / (commonStats.NbPalier * Stat.ValeurPalier)) * commonStats.NbPalier);
        if (t <= 0)
            t = 0;
        else
            t--;

        Stat.Tension = t * Stat.ValeurPalier;
    }

    public void ReceiveTension(Source sourceDamage)
    {
        int oldPalier = (int)(_stat.Tension / _stat.ValeurPalier);
        switch (sourceDamage)
        {
            case Source.Attaque:
                Stat.Tension += commonStats.GainTensionAttaque;
                gainedTension = true;
                break;
            case Source.Dot:
                Stat.Tension += commonStats.GainTensionDot;
                gainedTension = true;
                break;
            case Source.Buff:
                Stat.Tension += commonStats.GainTensionDebuff;
                gainedTension = true;
                break;
            case Source.Soin:
                Stat.Tension += commonStats.GainTensionSoin;
                gainedTension = true;
                break;
        }
        int newPalier = (int)(_stat.Tension / _stat.ValeurPalier);
        if (oldPalier < newPalier)
            OnGainTensionLevel?.Invoke();                               // On gagne un palier de tension
        if (Stat.Tension >= Stat.ValeurPalier * commonStats.NbPalier)
        {
            Stat.Tension = Stat.ValeurPalier * commonStats.NbPalier;
        }
        if (Stat.Tension < 0)
            Stat.Tension = 0;
    }

    public virtual bool CanHaveAnotherTurn()
    {
        return Stat.Tension >= Stat.ValeurPalier * commonStats.NbPalier;
    }
    public virtual void ResetStat()
    {
        Stat.MultiplDegat = 1;
        Stat.MultiplDef = 1;
        Stat.MultiplSoin = 1;
        Stat.MultipleBuffDebuff = 1;
        Stat.RadianceMax = Stat.RadianceMaxOriginal;
        Stat.Vitesse = Stat.VitesseOriginal;
        Stat.Resilience = Stat.ResilienceOriginal;
        Stat.ForceAme = Stat.ForceAmeOriginal;
        Stat.Conviction = Stat.ConvictionOriginal;
    }
    public void MakeIntangible()
    {
        IsIntangible = true;
    }
    public void MakeTangible()
    {
        IsIntangible = false;
    }

}