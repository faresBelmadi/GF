using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Threading;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GeneralPopUp : MonoBehaviour
{
    private static GeneralPopUp instance = null;
    public static GeneralPopUp Instance = instance;
    [Header("PopUp Window")]
    [SerializeField] private GameObject pupUpWindowPrefab;
    [SerializeField] private AnimationCurve movmentCurve;
    [SerializeField] private float popAnimDuration;

    [Header("Quick PopUp")]
    [SerializeField] private GameObject quickPupUpPrefab;
    [SerializeField] private AnimationCurve quickPopUpMovmentCurve;
    [SerializeField] private AnimationCurve quickPopUpAlphaCurve;
    [SerializeField] private float quickPopUpFinalHeight;
    [SerializeField] private float quickPopUpAnimDuration;
    [SerializeField] private float BaseQueueDelay;


    private List<PopUpInfo> popUpQueue = new List<PopUpInfo>();
    private Coroutine PopUpMovmentsCoroutine = null;
    
    private Dictionary<Transform, List<QuickPopUpInfo>> quickPopUpQueues = new Dictionary<Transform, List<QuickPopUpInfo>>();
    private Dictionary<Transform, Coroutine> quickPopUpRoutines = new Dictionary<Transform, Coroutine>();
    
    class PopUpInfo
    {
        public string title;
        public string core;
        public float activeTime;
        public PopUpInfo(string _title, string _core, float _activeTime) 
        {
            title = _title;
            core = _core;
            activeTime = _activeTime;
        }
    }

    class QuickPopUpInfo
    {
        public string text;
        public Color textColor;
        public Transform spawnTransform;
        public Sprite backgroundSprite;
        public Sprite effectSprite;
        public QuickPopUpInfo(string _text, Color textColor, Transform _spawnTransform, Sprite _effectSprite, Sprite backgroundSprite)
        {
            this.text = _text;
            this.textColor = textColor;
            this.spawnTransform = _spawnTransform;
            this.effectSprite = _effectSprite;
            this.backgroundSprite = backgroundSprite;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("Already Instancied");
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            Instance = instance;
        }
        DontDestroyOnLoad(this.gameObject);
       // InvokePopUp("Pop Up","Pop Up system is awake and ready",5);
    }
    private GameObject GetcurrentPertinantCanvas()
    {
        GameObject nonUICanvasObject = null;
        foreach (GameObject canv in GameObject.FindGameObjectsWithTag("CanvasLayer"))
        {
            if(canv.GetComponent<Canvas>() != null 
                && canv.activeInHierarchy 
                && canv.GetComponent<Canvas>().sortingLayerName == "UI")
            {
                Debug.Log("Pertinant canvas found");
                return canv;
            }
            nonUICanvasObject = canv;
        }
        Debug.Log("RETURN NULL: No Pertiannt canvas found");
        return nonUICanvasObject;
    }
    public void ResetPopUpQueue()
    {
        popUpQueue.Clear();
    }
    public void InvokePopUp(string title = "", string core = "", float activeTime = 3f, bool putFirstInQueue = false)
    {

        PopUpInfo popUp = new PopUpInfo(title, core, activeTime);
        if (PopUpMovmentsCoroutine != null)
        {
            if(putFirstInQueue) 
            {
                popUpQueue.Insert(0, popUp);
                return;
            }
            else
            {
                popUpQueue.Add(popUp);
                return;
            }
        }

        PopUpMovmentsCoroutine = StartCoroutine(InvokePopUpCoroutine(popUp));
    }

    public void InvokeQuickPopUp(string text , Color textColor, Transform spawnTransform, Sprite effectSprite = null, Sprite backgroundSprite = null)
    {
        Debug.Log("QuickPopUp Invoked");
        QuickPopUpInfo QpopUp = new QuickPopUpInfo(text, textColor, spawnTransform, effectSprite, backgroundSprite);
        
        if (!quickPopUpQueues.ContainsKey(spawnTransform))
        {
            quickPopUpQueues[spawnTransform] = new List<QuickPopUpInfo>();
            quickPopUpRoutines[spawnTransform] = null;
        }
        quickPopUpQueues[spawnTransform].Add(QpopUp);


        if (quickPopUpRoutines[spawnTransform] != null)
        {
            Debug.Log("Quick Return");
            return;
        }

        quickPopUpRoutines[spawnTransform] = StartCoroutine(InvokeQuickPopUpCoroutine(spawnTransform));
    }

    IEnumerator InvokePopUpCoroutine(PopUpInfo popUp = null)
    {
        Vector3 endpos = Vector2.zero;
        Vector3 startPos = Vector2.zero;


        while (popUpQueue.Count > 0 || popUp != null)
        {
            if(popUp == null)
            {
                popUp = popUpQueue[0];
                popUpQueue.RemoveAt(0);
            }

            GameObject choosedCanvas = GetcurrentPertinantCanvas();
            GameObject popUpWindowObject = Instantiate(pupUpWindowPrefab, choosedCanvas.transform);
            popUpWindowObject.GetComponent<PopUpManager>().SetTitleText(popUp.title);
            popUpWindowObject.GetComponent<PopUpManager>().SetCoreText(popUp.core);
            RectTransform popUpRect = popUpWindowObject.GetComponent<RectTransform>();
            
            endpos = popUpRect.anchoredPosition;
            startPos = endpos;
            //Pop IN
            startPos.x = endpos.x - popUpRect.sizeDelta.x;
            popUpRect.anchoredPosition = startPos;
            float timeLeft = 0;
            while (timeLeft < popAnimDuration)
            {
                if (!popUpRect)
                {
                    PopUpMovmentsCoroutine = null;
                    yield break;
                }
                timeLeft += Time.deltaTime;
                popUpRect.anchoredPosition = startPos + (movmentCurve.Evaluate(timeLeft/popAnimDuration) * (endpos - startPos)); //Vector3.Lerp(startPos, endpos, movmentCurve.Evaluate(timeLeft));
                yield return null;
            }
            popUpRect.anchoredPosition = endpos;

            yield return new WaitForSeconds(popUp.activeTime);

            //Pop OUT
            while (timeLeft > 0)
            {
                if (!popUpRect)
                {
                    PopUpMovmentsCoroutine = null;
                    yield break;
                }
                timeLeft -= Time.deltaTime;
                popUpRect.anchoredPosition = startPos + (movmentCurve.Evaluate(timeLeft/ popAnimDuration) * (endpos - startPos)); //Vector3.Lerp(startPos, endpos, movmentCurve.Evaluate(timeLeft));
                yield return null;
            }
            popUpRect.anchoredPosition = startPos;

            Destroy(popUpWindowObject);
            popUp = null;
        }
        PopUpMovmentsCoroutine = null;
        yield break;
    }

    IEnumerator InvokeQuickPopUpCoroutine(Transform spawnTransform)
    {
        QuickPopUpInfo popUp = quickPopUpQueues[spawnTransform][0];
        quickPopUpQueues[spawnTransform].RemoveAt(0);

        Debug.Log("Spawning PopUp Prefab");
        GameObject popUpObject = Instantiate(quickPupUpPrefab, spawnTransform);

        QuickPopUpAccesor popUpAccessor = popUpObject.GetComponent<QuickPopUpAccesor>();
        popUpAccessor.SetText(popUp.text);
        popUpAccessor.SetEffectSprite(popUp.effectSprite);
        popUpAccessor.SetBackbroundSprite(popUp.backgroundSprite);
        popUpAccessor.SetTextColor(popUp.textColor);
        RectTransform popUpRect = popUpObject.GetComponent<RectTransform>();

        Vector3 startPos = popUpRect.anchoredPosition;
        Vector3 endpos = startPos + Vector3.up * quickPopUpFinalHeight;

        bool nextTriggered = false;
        float elapsedTime = 0;
        while (elapsedTime<Mathf.Max(quickPopUpAnimDuration,BaseQueueDelay))
        {
            if(!popUpRect)yield break;
            elapsedTime += Time.deltaTime;

            popUpRect.anchoredPosition = startPos + (quickPopUpMovmentCurve.Evaluate(elapsedTime / quickPopUpAnimDuration) * (endpos - startPos));
            popUpAccessor.SetPopUpAlpha(quickPopUpAlphaCurve.Evaluate(elapsedTime / quickPopUpAnimDuration));

            if (!nextTriggered && elapsedTime >= BaseQueueDelay) 
            {
                nextTriggered = true;
                if(quickPopUpQueues[spawnTransform].Count > 0)
                {
                    StartCoroutine(InvokeQuickPopUpCoroutine(spawnTransform));
                }else quickPopUpQueues.Remove(spawnTransform);
            }

            yield return null;
        }
        Destroy(popUpObject);

        quickPopUpRoutines[spawnTransform] = null;
        yield break;
    }
}
