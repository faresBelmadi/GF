using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolonteManager : MonoBehaviour
{
    [Tooltip("Trié du premier point au 7ème")]
    [SerializeField]
    private List<VolontePoint> _volontePoints;



    public int CurrentMaximumPoint { get; private set; }
    public int CurrentPoint { get; private set; }

    private void OnEnable()
    {
        HighlightCost.OnHighlintingVolonte += StartHighlight;
        HighlightCost.OnStopHighlintingVolonte += StopHighlight;
    }
    private void OnDisable()
    {
        HighlightCost.OnHighlintingVolonte -= StartHighlight;
        HighlightCost.OnStopHighlintingVolonte -= StopHighlight;
    }

    private void StartHighlight(int numberToHighlight)
    {
        int n = 0;
        for (int i = CurrentPoint -1; n<numberToHighlight;i--)
        {
            _volontePoints[i].Highlight();
            n++;
        }
    }
    private void StopHighlight()
    {
        for (int i = 0; i < CurrentMaximumPoint; i++)
        {
            _volontePoints[i].StopHighlight();
        }
    }

    public void UpdateVolonte(int volontePoint)
    {
        if (volontePoint < CurrentPoint)
        {
            ConsumeVolonte(volontePoint);
        }
        else if (volontePoint > CurrentPoint)
        {
            RefillVolonte(volontePoint);
        }
    }

    public void ConsumeVolonte (int newCurrentPoint)
    {
        if (newCurrentPoint >= 0 && newCurrentPoint < CurrentMaximumPoint)
        {
            for (int i = CurrentPoint -1; i >= newCurrentPoint;i--)
            {
                _volontePoints[i].Empty();
            }
            CurrentPoint = newCurrentPoint;
        }
    }
    public void RefillVolonte(int newCurrentPoint)
    {
        if (newCurrentPoint <= CurrentMaximumPoint)
        {
            for (int i = CurrentPoint; i < newCurrentPoint; i++)
            {
                _volontePoints[i].Full();
            }
            CurrentPoint = newCurrentPoint;
        }
    }

    public void InitVolonte(int numberOfMaximumPoint, int numberOfStartingPoint)
    {
        CurrentPoint = numberOfStartingPoint;
        UpdateMaxVolonte(numberOfMaximumPoint);
    }
    public void UpdateMaxVolonte (int maxPoint)
    {
        CurrentMaximumPoint = maxPoint;
        int currentPoint = 0;
        for (int i = 0; i < _volontePoints.Count; i++)
        {
            currentPoint++;
            if (currentPoint <= CurrentMaximumPoint)
            {
                _volontePoints[i].gameObject.SetActive(true);
                _volontePoints[i].Empty();
            }
            else
            {
                _volontePoints[i].gameObject.SetActive(false);
            }
        }
        if (CurrentPoint > CurrentMaximumPoint)
            CurrentPoint = CurrentMaximumPoint;

        for (int i = 0; i < CurrentPoint; i++)
        {
            _volontePoints[i].Full();
        }
    }
}
