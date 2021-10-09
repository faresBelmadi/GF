// using System.Collections;
// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;

// #if UNITY_EDITOR
// public class DialogueListEditor
// {
//     Vector2 scrollDistance = Vector2.zero;
//     public float sizeScroll;
//     public Rect rectScroll;
//     public Rect rectScrollFull;
//     List<string> values;
//     List<Dialogue> SideBarList;

//     DialogueOption selected;
//     bool openList;
//     public void DrawList(Rect rect, float padding, Color color) {
// 			float innerWidth = rect.width - (padding * 2f);
// 			float innerHeight = rect.height - (padding * 2f);

//             changeSelectedValue(selected);
//             var temp =  40 + (60*SideBarList.Count)*2;
//             if(temp <= innerHeight)
//                 sizeScroll = innerHeight-30;
//             else
//                 sizeScroll = temp;


//             rectScrollFull = new Rect(padding, padding+15, innerWidth, innerHeight-30);

// 			GUI.BeginGroup(rect); // Container

// 			DrawBox(new Rect(0, 0, rect.width, rect.height), color);

//             var dropdownRect = new Rect(padding,5,innerWidth-10, 20);

//             selected = (DialogueOption)EditorGUI.EnumPopup(dropdownRect,selected);
//             changeSelectedValue(selected);
            
//             scrollDistance = GUI.BeginScrollView(rectScrollFull,scrollDistance,new Rect(0, 30, 0, sizeScroll));
//             for (int i = 0; i < SideBarList.Count; i++)
//             {
//                 if(GUI.Button(new Rect(new Vector2(0,40+60*i),new Vector2(innerWidth-10,50)),new GUIContent(){ tooltip = SideBarList[i].Text,text = SideBarList[i].name }))
//                 {
//                     DialogueEditor.current.AddNode(SideBarList[i]);
//                 }
//             }
//             GUI.EndScrollView();
// 			GUI.EndGroup(); // Container

//     }

//     void changeSelectedValue(DialogueOption index)
//     {
//         SideBarList = DialogueEditor.current.GetDialogues(index);
//     }

//     void DrawBox (Rect position, Color color) {
// 			Color oldColor = GUI.color;

// 			GUI.color = color;
// 			GUI.Box(position, "");

// 			GUI.color = oldColor;
// 		}
// }
// #endif