using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvictionManager : PointsManager<ConvictionPoint>
{
    bool _positive;

    public bool Positive { get { return _positive; } set { _positive = value; } }

    private void OnEnable()
    {
        JoueurBehavior.OnConvictionFull += StartHighlight;
        JoueurBehavior.OnConvictionEmpty += StopHighlight;
    }
    private void OnDisable()
    {
        JoueurBehavior.OnConvictionFull -= StartHighlight;
        JoueurBehavior.OnConvictionEmpty -= StopHighlight;
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
}

