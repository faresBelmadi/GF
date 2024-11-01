using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GeneralPopUp : MonoBehaviour
{
    private static GeneralPopUp instance = null;
    public static GeneralPopUp Instance = instance;
    private Canvas activeCanvas = null;
    [Header("PopUp Window")]
    [SerializeField] private GameObject pupUpWindowPrefab;
    [SerializeField] private AnimationCurve movmentCurve;
    //[SerializeField] private Vector2 startPos;
    //[SerializeField] private Vector2 endPos;
    [SerializeField] private float popAnimDuration;

    private bool isActivePopUpPresent = false;
    private List<PopUpInfo> popUpQueue = new List<PopUpInfo>();

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

    private Coroutine PopUpMovmentsCoroutine = null;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("Already Instancied");
            Destroy(this);
            return;
        }
        else
        {
            instance = this;
            Instance = instance;
        }
        DontDestroyOnLoad(this.gameObject);
        InvokePopUp("Pop Up","Pop Up system is awake and ready",5);
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
        isActivePopUpPresent = true;
        PopUpMovmentsCoroutine = StartCoroutine(InvokePopUpCoroutine(popUp));
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
}
