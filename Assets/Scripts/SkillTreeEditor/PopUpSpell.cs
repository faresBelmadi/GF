using UnityEditor;
using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
public class PopUpSpell : EditorWindow {


    string id;
    public static Vector2 pos;

    public static void ShowWindow() {
        var window = ScriptableObject.CreateInstance(typeof(PopUpSpell)) as PopUpSpell;
        window.titleContent = new GUIContent("PopUpSpell");
        //window.ShowModalUtility();
        window.ShowAuxWindow();
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("entrer l'id numérique du spell :");
        id = EditorGUILayout.TextField(id);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("valider"))
        {
            SkillTreeController.current.OnClickAddNode(pos,id);
            Close();
        }

        if (GUILayout.Button("Close"))
            Close();

        EditorGUILayout.EndHorizontal();
    }
}
#endif