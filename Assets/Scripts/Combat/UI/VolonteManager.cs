using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolonteManager : PointsManager<VolontePoint> 
{
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


    public void InitVolonte(int numberOfMaximumPoint, int numberOfStartingPoint)
    {
        CurrentPoint = numberOfStartingPoint;
        UpdateMaxVolonte(numberOfMaximumPoint);
    }
    public void UpdateMaxVolonte (int maxPoint)
    {
        CurrentMaximumPoint = maxPoint;
        int currentPoint = 0;
        for (int i = 0; i < _points.Count; i++)
        {
            currentPoint++;
            if (currentPoint <= CurrentMaximumPoint)
            {
                _points[i].gameObject.SetActive(true);
                _points[i].Empty();
            }
            else
            {
                _points[i].gameObject.SetActive(false);
            }
        }
        if (CurrentPoint > CurrentMaximumPoint)
            CurrentPoint = CurrentMaximumPoint;

        for (int i = 0; i < CurrentPoint; i++)
        {
            _points[i].Full();
        }
    }
}
