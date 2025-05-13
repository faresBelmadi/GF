using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEnnemi : MonoBehaviour
{

    [SerializeField] private ProgressBarManager HPBarManager;
    [SerializeField] private ProgressBarManager TensionBarManager;
    [SerializeField] public EnflateSystem inflateUISystem;
    [SerializeField] public Image[] imageCadreFGs;

    public TextMeshProUGUI HpText;
    public TextMeshProUGUI NameText;
    public GameObject Ciblage;
    public Image Intention;

    public bool TargetingMode = false;

    public Action RaiseEvent;
    public Action OnPreviewDamage;
    public Action OnStopPreviewDamage;

    public Transform degatSoinParent, buffParents, debuffParents;

    public GameObject soinPrefab;
    public GameObject degatPrefab;
    public List<GameObject> DebuffSpawned = new List<GameObject>();

    public void UpdateHp(int newHp, int newMaxHp)
    {
        if (!HPBarManager) return;
        HPBarManager.UpdatePBar(newHp, newMaxHp);
        HPBarManager.ToggleBloomPulses(false);
        HpText.text = newHp.ToString();// + "/" + newMaxHp;
    }

    public void UpdateTension(int newTension, int nbPalier)
    {
        if (!TensionBarManager) return;
        TensionBarManager.UpdatePBar(newTension, nbPalier);
    }

    public void UpdateNom(string nom)
    {
        NameText.text = nom;
    }
    public void ShowTargeting() => Ciblage.SetActive(true);
    public void HideTargeting()
    {
        Ciblage.SetActive(false);
    }
    private void OnMouseEnter()
    {
        //ici pour arreter le ciblage
        if (gameObject.GetComponent<EnnemyBehavior>().IsIntangible)
        {
            return;
        }
        if (TargetingMode)
        {
            ShowTargeting();
            OnPreviewDamage?.Invoke();
        }
        //if (debuffParents.childCount > 0 || buffParents.childCount > 0)
        //    GetComponentInChildren<DescriptionHoverTrigger>().SendMessage("ShowDescription");
    }

    private void OnMouseExit()
    {
        if (gameObject.GetComponent<EnnemyBehavior>().IsIntangible)
        {
            return;
        }
        if (TargetingMode)
        {
            HideTargeting();
            OnStopPreviewDamage?.Invoke();
        }

        //if (debuffParents.childCount > 0 || buffParents.childCount > 0)
        //    GetComponentInChildren<DescriptionHoverTrigger>().SendMessage("HideDescription");
    }

    public void SpawnDegatSoin(int value)
    {
        //GameObject t;
        if (value < 0)
            GeneralPopUp.Instance.InvokeQuickPopUp(value.ToString(),Color.red,degatSoinParent);
            //t = Instantiate(degatPrefab, degatSoinParent);
        else
            GeneralPopUp.Instance.InvokeQuickPopUp(value.ToString(),Color.green,degatSoinParent);
            //t = Instantiate(soinPrefab, degatSoinParent);

        //t.GetComponent<TextAnimDegats>().Value = value;

    }

    public void ChangeIntention(Sprite intent)
    {
        Intention.sprite = intent;
    }

    public void PreviewDmg(int value, int maxRadiance)
    {
        HPBarManager.PreviewBar(value, maxRadiance);
    }
    public void StopPreview()
    {
        HPBarManager.StopPreview();
    }

    private void OnMouseDown()
    {
        if (gameObject.GetComponent<EnnemyBehavior>().IsIntangible)
        {
            return;
        }
        if (TargetingMode)
        {
            RaiseEvent();
            Ciblage.SetActive(false);
        }
    }

}
