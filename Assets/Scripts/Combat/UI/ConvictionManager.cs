using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ConvictionManager : PointsManager<ConvictionPoint>
{
    bool _positive;
    [SerializeField]
    private TMP_Text _descriptionText;
    [SerializeField]
    private string _idTradConvictionBuff;
    [SerializeField]
    private string _idTradConvictionDebuff;
    [SerializeField]
    private GameObject _descriptionGO;

   public bool Positive { get { return _positive; } set { _positive = value; } }

    private void OnEnable()
    {
        JoueurBehavior.OnConvictionFull += StartHighlight;
        JoueurBehavior.OnConvictionEmpty += StopHighlight;
        TradManager.OnRefreshTranslation += SetDescription;
    }
    private void OnDisable()
    {
        JoueurBehavior.OnConvictionFull -= StartHighlight;
        JoueurBehavior.OnConvictionEmpty -= StopHighlight;
        TradManager.OnRefreshTranslation -= SetDescription;
    }


    protected void StartHighlight()
    {
       foreach(var point in _points)
       {
            point.Highlight();
       }
    }
    protected new void StopHighlight()
    {
        foreach (var point in _points)
        {
            point.StopHighlight();
        }
    }

    public override void AddPoints(int newCurrentPoint)
    {
        if (newCurrentPoint <= CurrentMaximumPoint)
        {
            for (int i = CurrentPoint; i < newCurrentPoint; i++)
            {
                if(_positive)
                    _points[i].Positive();
                else
                    _points[i].Negative();
            }
            CurrentPoint = newCurrentPoint;
        }
    }


    public void UpdateMaxConviction(int maxPoint)
    {
        CurrentMaximumPoint = maxPoint;
    }
    public void ShowDescripion()
    {
        SetDescription();
        _descriptionGO.SetActive(true);
    }
    public void HideDescription()
    {
        _descriptionGO.SetActive(false);
    }
    public void SetDescription()
    {
        _descriptionText.text = $"{TradManager.instance.GetTranslation((Positive) ? _idTradConvictionBuff : _idTradConvictionDebuff)} {Mathf.Abs(GameManager.Instance.BattleMan.player.ValueConviction() * 100)}%";
    }
}

