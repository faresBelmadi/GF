// using UnityEngine;
// using System.Collections;
// using UnityEditor;
// using System.Text;
// using System;
// using System.Linq;


// #if UNITY_EDITOR
// //We use scriptable object because in the future we will need the messages that unity calls on scriptable objects
// //such as OnDestroy() OnEnable()
// public class DialogueNode {

//     public Rect rect;
//     string title;
//     Dialogue thisNode;
//     bool isDragged;
//     bool isSelected;
//     GUIStyle style;
//     GUIStyle selectedStyle;
    
//     DialogueConnectionPoint inPoint,outPoint;

//     public DialogueNode(Rect pos, Dialogue node,GUIStyle[] pointStyle,Action<DialogueConnectionPoint> OnClickInPoint, Action<DialogueConnectionPoint> OnClickOutPoint)
//     {
//         rect = pos;
//         title = node.nom;
//         thisNode = node;
// 		inPoint = new DialogueConnectionPoint(this,DialogueConnectionPointType.In,pointStyle[0],OnClickInPoint);
//         outPoint = new DialogueConnectionPoint(this,DialogueConnectionPointType.Out,pointStyle[1],OnClickOutPoint);
//     }

//     public void Draw()
//     {
//         inPoint.Draw();
//         outPoint.Draw();
//         chooseStyle();
//         GUI.Box(rect, title,style);
//     }

//     private void chooseStyle()
//     {
//         switch(thisNode.Option)
//         {
//             case DialogueOption.QuestionEnemy:
//             style = GUI.skin.customStyles.First( c => c.name == "flow node 0");
//             selectedStyle = GUI.skin.customStyles.First( c => c.name == "flow node 0 on");
//             break;
//             case DialogueOption.PhraseJoueur:
//             style = GUI.skin.customStyles.First( c => c.name == "flow node 2");
//             selectedStyle = GUI.skin.customStyles.First( c => c.name == "flow node 2 on");
//             break;
//             case DialogueOption.ReponseJoueur:
//             style = GUI.skin.customStyles.First( c => c.name == "flow node hex 1");
//             selectedStyle = GUI.skin.customStyles.First( c => c.name == "flow node hex 1 on");
//             break;
//             case DialogueOption.Narrateur:
//             style = GUI.skin.customStyles.First( c => c.name == "");
//             selectedStyle = GUI.skin.customStyles.First( c => c.name == "");
//             break;
//             case DialogueOption.ConséquenceInGame:
//             style = GUI.skin.customStyles.First( c => c.name == "flow node hex 4");
//             selectedStyle = GUI.skin.customStyles.First( c => c.name == "flow node hex 4 on");
//             break;
//         }
//     }

//     public void Drag(Vector2 delta)
//     {
//         rect.position += delta;
//         // rectID.position += delta;
//         // rectUnlocked.position += delta;
//         // rectUnlockLabel.position += delta;
//         // rectRefresh.position += delta;
//         // rectRefreshLabel.position += delta;
//         // rectRefreshButton.position += delta;
//     }

//     public bool ProcessEvents(Event e)
//     {
//         switch (e.type)
//         {
//             case EventType.MouseDown:
//                 if (e.button == 0)
//                 {
//                     if (rect.Contains(e.mousePosition))
//                     {
//                         isDragged = true;
//                         GUI.changed = true;
//                         isSelected = true;
//                         //style = selectedNodeStyle;
//                     }
//                     else
//                     {
//                         GUI.changed = true;
//                         isSelected = false;
//                         //style = defaultNodeStyle;
//                     }
//                 }

//                 if (e.button == 1 && isSelected && rect.Contains(e.mousePosition))
//                 {
//                     ProcessContextMenu();
//                     e.Use();
//                 }
//                 break;

//             case EventType.MouseUp:
//                 isDragged = false;
//                 break;

//             case EventType.MouseDrag:
//                 if (e.button == 0 && isDragged)
//                 {
//                     Drag(e.delta);
//                     e.Use();
//                     return true;
//                 }
//                 break;
//         }

//         return false;
//     }

//     private void ProcessContextMenu()
//     {
//         GenericMenu genericMenu = new GenericMenu();
//         genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
//         genericMenu.ShowAsContext();
//     }

//     private void OnClickRemoveNode()
//     {
//         DialogueEditor.current.RemoveNode(this);
//     }

// }
// #endif