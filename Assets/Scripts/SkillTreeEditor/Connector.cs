using System;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public enum ConnectionPointType { In, Out }
 
public class ConnectionPoint
{
     public Rect rect;

    public ConnectionPointType type;

    public Node node;

    public GUIStyle style;

    public Action<ConnectionPoint> OnClickConnectionPoint;

    public ConnectionPoint(Node node, ConnectionPointType type, GUIStyle style, Action<ConnectionPoint> OnClickConnectionPoint)
    {
        this.node = node;
        this.type = type;
        this.style = style;
        this.OnClickConnectionPoint = OnClickConnectionPoint;
        rect = new Rect(0, 0, 10f, 20f);
    }

    public void Draw()
    {
        rect.x = node.rect.x + (node.rect.width * 0.5f) - rect.width * 0.5f;

        switch (type)
        {
            case ConnectionPointType.In:
                rect.y = node.rect.y + (rect.height +70f);
                break;

            case ConnectionPointType.Out:
                rect.y = node.rect.y - (node.rect.height - 90f);
                break;
        }

        if (GUI.Button(rect, "", style))
        {
            if (OnClickConnectionPoint != null)
            {
                OnClickConnectionPoint(this);
            }
        }
    }
}

public class Connection
{
    public ConnectionPoint inPoint;
    public ConnectionPoint outPoint;
    public Action<Connection> OnClickRemoveConnection;

    public Connection(ConnectionPoint inPoint, ConnectionPoint outPoint, Action<Connection> OnClickRemoveConnection)
    {
        this.inPoint = inPoint;
        this.outPoint = outPoint;
        this.OnClickRemoveConnection = OnClickRemoveConnection;

        //outPoint.node.SpellNode.idChildren.Add(inPoint.node.SpellNode.ID);
    }

    public void Draw()
    {
        Handles.DrawBezier(
            inPoint.rect.center,
            outPoint.rect.center,
            inPoint.rect.center,
            outPoint.rect.center,
            Color.red,
            null,
            2f
        );

        if (Handles.Button((inPoint.rect.center + outPoint.rect.center) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleHandleCap))
        {
            if (OnClickRemoveConnection != null)
            {
                OnClickRemoveConnection(this);
            }
        }
    }
}
#endif