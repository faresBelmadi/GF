using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class DescriptionHoverVisibility
{
    public void CheckVisibilityAndAdjustPosition(RectTransform rectTransform, Camera camera)
    {
        Rect screenBounds = new Rect(0f, 0f, Screen.width, Screen.height);
        Vector3[] objectCorners = new Vector3[4];
        rectTransform.GetWorldCorners(objectCorners);

        float leftExceed = 0f, rightExceed = 0f, topExceed = 0f, bottomExceed = 0f;

        for (int i = 0; i < objectCorners.Length; i+=2)
        {
            Vector3 screenSpaceCorner = camera.WorldToScreenPoint(objectCorners[i]);

            Vector2 newPoz = Vector2.zero;

            if (screenSpaceCorner.x < screenBounds.xMin)
            {
                leftExceed = Mathf.Min(leftExceed, screenSpaceCorner.x - screenBounds.xMin);
                newPoz.x -= leftExceed;
            }
            else if (screenSpaceCorner.x > screenBounds.xMax)
            {
                rightExceed = Mathf.Max(rightExceed, screenSpaceCorner.x - screenBounds.xMax);
                newPoz.x -= rightExceed;
            }

            if (screenSpaceCorner.y < screenBounds.yMin)
            {
                bottomExceed = Mathf.Min(bottomExceed, screenSpaceCorner.y - screenBounds.yMin);
                newPoz.y -= bottomExceed;
            }
            else if (screenSpaceCorner.y > screenBounds.yMax)
            {
                topExceed = Mathf.Max(topExceed, screenSpaceCorner.y - screenBounds.yMax);
                newPoz.y -= topExceed;
            }

            newPoz += rectTransform.anchoredPosition;
            rectTransform.anchoredPosition = newPoz;
        }
    }
}
