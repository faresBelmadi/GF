// using System;
// using UnityEditor;
// using UnityEngine;

// #if UNITY_EDITOR
// public enum DialogueConnectionPointType { In, Out }
 
// public class DialogueConnectionPoint
// { 
//     public Rect rect;

//     public DialogueConnectionPointType type;

//     public DialogueNode node;

//     public GUIStyle style;

//     public Action<DialogueConnectionPoint> OnClickConnectionPointDelete;

//     public DialogueConnectionPoint(DialogueNode node, DialogueConnectionPointType type, GUIStyle style, Action<DialogueConnectionPoint> OnClickConnectionPoint)
//     {
//         this.node = node;
//         this.type = type;
//         this.style = style;
//         this.OnClickConnectionPointDelete = OnClickConnectionPoint;
//         rect = new Rect(0, 0, 10f, 20f);
//     }

//     public void Draw()
//     {
//         rect.y = node.rect.y + (node.rect.height * 0.5f) - rect.height * 0.5f;

//         switch (type)
//         {
//             case DialogueConnectionPointType.In:
//                 rect.x = node.rect.x - (rect.width/2 + 5);
//                 break;

//             case DialogueConnectionPointType.Out:
//                 rect.x = node.rect.x + (node.rect.width);
//                 break;
//         }

//         if (GUI.Button(rect, "", style))
//         {
//             if (OnClickConnectionPointDelete != null)
//             {
//                 OnClickConnectionPointDelete(this);
//             }
//         }
//     }
// }

// public class DialogueConnection
// {
//     public DialogueConnectionPoint inPoint;
//     public DialogueConnectionPoint outPoint;
//     public Action<DialogueConnection> OnClickRemoveConnection;

//     public DialogueConnection(DialogueConnectionPoint inPoint, DialogueConnectionPoint outPoint, Action<DialogueConnection> OnClickRemoveConnection)
//     {
//         this.inPoint = inPoint;
//         this.outPoint = outPoint;
//         this.OnClickRemoveConnection = OnClickRemoveConnection;
//     }

//     public void Draw()
//     {
//         Handles.DrawBezier(
//             inPoint.rect.center,
//             outPoint.rect.center,
//             inPoint.rect.center,
//             outPoint.rect.center,
//             Color.red,
//             null,
//             2f
//         );

//         if (Handles.Button((inPoint.rect.center + outPoint.rect.center) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleHandleCap))
//         {
//             if (OnClickRemoveConnection != null)
//             {
//                 OnClickRemoveConnection(this);
//             }
//         }
//     }
// }
// #endif