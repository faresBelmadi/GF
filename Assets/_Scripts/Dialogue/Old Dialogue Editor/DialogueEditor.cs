// using UnityEngine;
// using UnityEditor;
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using System.Text;

// #if UNITY_EDITOR
// public class DialogueEditor : EditorWindow 
// {
//     public static DialogueEditor current;
//     public DialogueListEditor sidebarList;
//     float sidebarWidth = 240f; // Size of the sidebar
//     public List<DialogueNode> nodeOnEditor;
//     List<Dialogue> dialogues;

//     List<string> Folders;
//     List<string> toPrint;

//     GUIStyle nodeInPoint, nodeOutPoint;
//     Vector2 drag;
//     DialogueConnectionPoint selectedInPoint;
//     DialogueConnectionPoint selectedOutPoint;
//     List<DialogueConnection> connections;

    
//     private Vector2 offset;



//     [MenuItem("LivingMemory/Dialogue Editor")]
//     private static void OpenWindow()
//     {
//         current = GetWindow<DialogueEditor>();
//         current.titleContent = new GUIContent("Dialogue Editor");
//         if(current.sidebarList == null) current.sidebarList = new DialogueListEditor();
//         if(current.nodeOnEditor == null) current.nodeOnEditor = new List<DialogueNode>();

//     }
//     private void OnEnable() {

//         nodeInPoint = new GUIStyle();
//         nodeInPoint.normal.background = Resources.Load("green") as Texture2D;
//         nodeInPoint.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
//         nodeInPoint.border = new RectOffset(4, 4, 12, 12);

//         nodeOutPoint = new GUIStyle();
//         nodeOutPoint.normal.background = Resources.Load("red") as Texture2D;
//         nodeOutPoint.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
//         nodeOutPoint.border = new RectOffset(4, 4, 12, 12);
//     }

//     private void OnGUI() {

//         DrawGrid(20, 0.2f, Color.gray);
//         DrawGrid(100, 0.4f, Color.gray);


//         sidebarList.DrawList(new Rect(position.width - sidebarWidth, 0, sidebarWidth, position.height), 10f, Color.gray);
//         foreach (var item in nodeOnEditor)
//         {
//             item.Draw();
//         }
//         DrawConnections();
//         DrawConnectionLine(Event.current);
//         ProcessNodeEvents(Event.current);
//         ProcessEvents(Event.current);
//     }

//     private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
//     {
//         int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
//         int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

//         Handles.BeginGUI();
//         Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

//         offset += drag * 0.5f;
//         Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

//         for (int i = 0; i < widthDivs; i++)
//         {
//             Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
//         }

//         for (int j = 0; j < heightDivs; j++)
//         {
//             Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
//         }

//         Handles.color = Color.red;
//         Handles.EndGUI();
//     }

//     public void AddNode(Dialogue Node)
//     {
//         nodeOnEditor.Add(new DialogueNode(new Rect(current.position.width/2 - sidebarWidth,current.position.height/2,200,100),Node,new GUIStyle[]{nodeInPoint,nodeOutPoint},OnClickInPoint,OnClickOutPoint));
//     }
//     public void RemoveNode(DialogueNode node)
//     {
//         nodeOnEditor.Remove(node);
//     }    
    
//     private void OnDrag(Vector2 delta)
//     {
//         drag = delta;

//         if (nodeOnEditor != null)
//         {
//             for (int i = 0; i < nodeOnEditor.Count; i++)
//             {
//                 nodeOnEditor[i].Drag(delta);
//             }
//         }

//         GUI.changed = true;
//     }

//     internal List<Dialogue> GetDialogues(DialogueOption index)
//     {
//         return Resources.FindObjectsOfTypeAll<Dialogue>().Where(c => c.Option == index).ToList();
//     }

//     private void ProcessNodeEvents(Event e)
//     {
//         if (nodeOnEditor != null)
//         {
//             for (int i = nodeOnEditor.Count - 1; i >= 0; i--)
//             {
//                 bool guiChanged = nodeOnEditor[i].ProcessEvents(e);

//                 if (guiChanged)
//                 {
//                     GUI.changed = true;
//                 }
//             }
//         }
//     }

//     private void OnClickInPoint(DialogueConnectionPoint inPoint)
//     {
//         selectedInPoint = inPoint;

//         if (selectedOutPoint != null)
//         {
//             if (selectedOutPoint.node != selectedInPoint.node)
//             {
//                 CreateConnection();
//                 ClearConnectionSelection();
//             }
//             else
//             {
//                 ClearConnectionSelection();
//             }
//         }
//     }

//     private void OnClickOutPoint(DialogueConnectionPoint outPoint)
//     {
//         selectedOutPoint = outPoint;
//         if (selectedInPoint != null)
//         {
            
//             if (selectedOutPoint.node != selectedInPoint.node)
//             {
//                 CreateConnection();
//                 ClearConnectionSelection();
//             }
//             else
//             {
//                 ClearConnectionSelection();
//             }
//         }
//     }    

//     private void DrawConnectionLine(Event e)
//     {
//         if (selectedInPoint != null && selectedOutPoint == null)
//         {
//             Handles.DrawBezier(
//                 selectedInPoint.rect.center,
//                 e.mousePosition,
//                 selectedInPoint.rect.center,
//                 e.mousePosition,
//                 Color.red,
//                 null,
//                 2f
//             );

//             GUI.changed = true;
//         }

//         if (selectedOutPoint != null && selectedInPoint == null)
//         {
//             Handles.DrawBezier(
//                 selectedOutPoint.rect.center,
//                 e.mousePosition,
//                 selectedOutPoint.rect.center,
//                 e.mousePosition,
//                 Color.red,
//                 null,
//                 2f
//             );

//             GUI.changed = true;
//         }
//     }
//     private void CreateConnection()
//     {
//         if (connections == null)
//         {
//             connections = new List<DialogueConnection>();
//         }
//         connections.Add(new DialogueConnection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));
//     }

//     private void DrawConnections()
//     {
//         if (connections != null)
//         {
//             for (int i = 0; i < connections.Count; i++)
//             {
//                 connections[i].Draw();
//             }
//         }
//     }
//     private void OnClickRemoveConnection(DialogueConnection connection)
//     {
//         connections.Remove(connection);
//     }

//     private void ClearConnectionSelection()
//     {
//         selectedInPoint = null;
//         selectedOutPoint = null;
//     }

//     private void ProcessEvents(Event e)
//     {
//         drag = Vector2.zero;

//         switch (e.type)
//         {
//             case EventType.MouseDown:
//                 if (e.button == 0)
//                 {
//                     //ClearConnectionSelection();
//                 }

//                 if (e.button == 1)
//                 {
//                     //ProcessContextMenu(e.mousePosition);
//                 }
//                 break;

//             case EventType.MouseDrag:
//                 if (e.button == 0)
//                 {
//                     OnDrag(e.delta);
//                 }
//                 break;
//         }
//     }
// }
// #endif