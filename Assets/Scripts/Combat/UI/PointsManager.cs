using System.Collections.Generic;
using UnityEngine;

public class PointsManager<T> : MonoBehaviour where T : Point
{
	[SerializeField]
	protected List<T> _points;
	public int CurrentMaximumPoint { get; protected set; }
	public int CurrentPoint { get; protected set; }


	protected virtual void StartHighlight(int numberToHighlight)
	{
		int n = 0;
		for (int i = CurrentPoint - 1; n < numberToHighlight; i--)
		{
			_points[i].Highlight();
			n++;
		}
	}
    protected void StopHighlight()
	{
		for (int i = 0; i < CurrentMaximumPoint; i++)
		{
			_points[i].StopHighlight();
		}
	}

    public void UpdatePoint(int point)
    {
        if (point < CurrentPoint)
        {
            RemovePoints(point);
        }
        else if (point > CurrentPoint)
        {
            AddPoints(point);
        }
    }

    public void RemovePoints(int newCurrentPoint)
    {
        if (newCurrentPoint >= 0 && newCurrentPoint < CurrentMaximumPoint)
        {
            for (int i = CurrentPoint - 1; i >= newCurrentPoint; i--)
            {
                _points[i].Empty();
            }
            CurrentPoint = newCurrentPoint;
        }
    }
    public virtual void AddPoints(int newCurrentPoint)
    {
        if (newCurrentPoint <= CurrentMaximumPoint)
        {
            for (int i = CurrentPoint; i < newCurrentPoint; i++)
            {
                _points[i].Full();
            }
            CurrentPoint = newCurrentPoint;
        }
    }
}