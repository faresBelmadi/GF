using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuffDebuffComponant : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public Image buffSprite;
    [SerializeField] public GameObject buffCntHolder;
    [SerializeField] public TextMeshProUGUI buffCntLabel;
    //[SerializeField] public TextMeshProUGUI buffTimeLabel;
    [SerializeField] public GameObject popUpPanel;
    [SerializeField] public TextMeshProUGUI buffNameLabel;
    [SerializeField] public TextMeshProUGUI buffDescriptionLabel;
    [SerializeField] private float ConvictionBonus;
    public string buffName;

    public List<BuffDebuff> BuffDebuffs { get; private set; } = new List<BuffDebuff>();

    
    private void OnEnable()
    {
        CombatBehavior<CharacterStat>.OnUpdateUI += UpdateUI;
        TradManager.OnRefreshTranslation += SetNameAndDescription;
    }
    private void OnDisable()
    {
        CombatBehavior<CharacterStat>.OnUpdateUI -= UpdateUI;
        TradManager.OnRefreshTranslation -= SetNameAndDescription;
    }

    public void InitBuffDebuff (BuffDebuff buffDebuff)
    {
        AddStack(buffDebuff);

        buffName = TradManager.instance.GetTranslation(buffDebuff.idTradName, buffDebuff.Nom);
        buffNameLabel.text = TradManager.instance.GetTranslation(buffDebuff.idTradName, buffDebuff.Nom); 
        List<float> variableValues = new List<float>();


        foreach (Effet e in buffDebuff.Effet)
        {
            if (e.ValeurBrut != 0)
                variableValues.Add(Mathf.Abs(e.ValeurBrut));
            if (e.Pourcentage != 0)
                variableValues.Add(Mathf.Abs((float)e.Pourcentage));
        }
        buffDescriptionLabel.text = TradManager.instance.GetTranslation(buffDebuff.idTradDescription, buffDebuff.Description);
       
    }
    private void SetNameAndDescription()
    {
        if (BuffDebuffs.Count > 0)
        {
            buffName = TradManager.instance.GetTranslation(BuffDebuffs[0].idTradName, BuffDebuffs[0].Nom);
            buffNameLabel.text = TradManager.instance.GetTranslation(BuffDebuffs[0].idTradName, BuffDebuffs[0].Nom);
            buffDescriptionLabel.text = TradManager.instance.GetTranslation(BuffDebuffs[0].idTradDescription, BuffDebuffs[0].Description);
        }
    }
    public void AddStack(BuffDebuff buffDebuff)
    {
        for (int i = BuffDebuffs.Count - 1; i >= 0; i--)
        {
            if (BuffDebuffs[i] == null)
            {
                BuffDebuffs.RemoveAt(i);
            }
        }
        BuffDebuffs.Add(buffDebuff);
    }
    public void RemoveNullStack()
    {
        for (int i = BuffDebuffs.Count - 1; i >= 0; i--)
        {
            if (BuffDebuffs[i].TimeLeft <= 0)
                BuffDebuffs.RemoveAt(i);
        }
    
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("enter");
        popUpPanel.SetActive(true);
        UpdateUI();
        //TODO temporary fix, on place la tooltip a une position d�finit, plutot que la scroll indefinitivement sur la droite, ce qui amene a une boucle infini si on place les buff autre part
        GameObject posGO = GameObject.FindGameObjectsWithTag("TooltipPosition")[0];
        if (posGO != null)
        {
            popUpPanel.transform.position = posGO.transform.position;
        }
        //Erreur Critique, boucle infinie, a corriger
        /*
        if (!IsFullyVisibleFrom(popUpPanel.GetComponent<RectTransform>()))
        {
            while (!IsFullyVisibleFrom(popUpPanel.GetComponent<RectTransform>()))
                popUpPanel.transform.Translate(-1, 0, 0);
        }*/
    }
    private bool IsFullyVisibleFrom(RectTransform rectTransform)
    {
        return CountCornersVisibleFrom(rectTransform, FindAnyObjectByType<Camera>()) == 4;
    }
    private static int CountCornersVisibleFrom(RectTransform rectTransform, Camera camera)
    {
        Rect screenBounds = new Rect(0f, 0f, Screen.width, Screen.height); // Screen space bounds (assumes camera renders across the entire screen)
        Vector3[] objectCorners = new Vector3[4];
        rectTransform.GetWorldCorners(objectCorners);

        int visibleCorners = 0;
        Vector3 tempScreenSpaceCorner; // Cached
        for (var i = 0; i < objectCorners.Length; i++) // For each corner in rectTransform
        {
            tempScreenSpaceCorner = camera.WorldToScreenPoint(objectCorners[i]); // Transform world space position of corner to screen space
            if (screenBounds.Contains(tempScreenSpaceCorner)) // If the corner is inside the screen
            {
                visibleCorners++;
            }
        }
        return visibleCorners;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("exit");
        popUpPanel.SetActive(false);
    }
    public void UpdateUI()
    {
        int timeLeft = -1;
        foreach (var buff in BuffDebuffs)
        {
            if (timeLeft < buff.TimeLeft)
                timeLeft = buff.TimeLeft;
        }
        if (BuffDebuffs.Count > 0)
        {
            var currentBuffDebuffs = BuffDebuffs.First();
            List<float> variableValues = new List<float>();


            foreach (Effet e in currentBuffDebuffs.Effet)
            {
                if (e.ValeurBrut != 0)
                    variableValues.Add(Mathf.Abs(e.ValeurBrut));
                if (e.Pourcentage != 0)
                    variableValues.Add(Mathf.Abs((float)e.Pourcentage));
            }
            buffDescriptionLabel.text = TradManager.instance.GetTranslation(currentBuffDebuffs.idTradDescription, currentBuffDebuffs.Description)
            + (timeLeft != -1 ? "\n(Time left : " + timeLeft + ")" : "");
        }
    }

}
