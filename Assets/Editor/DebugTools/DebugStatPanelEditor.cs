using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DebugStatPanel))]
public class DebugStatPanelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        
        serializedObject.Update();
        DebugStatPanel panel = (DebugStatPanel)target;

        DrawDefaultInspector();
        //This draws the default screen.  You don't need this if you want
        //to start from scratch, but I use this when I'm just adding a button or
        //some small addition and don't feel like recreating the whole inspector.

        if (GUILayout.Button("Laod All Effects"))
        {
            LoadAllEffect(panel);
            //add everthing the button would do.
            Debug.Log("All Affect Loaded");
            
        }
        if (GUILayout.Button("Clear Effect list"))
        {
            ClearAllEffect(panel);
            serializedObject.ApplyModifiedProperties();
            //add everthing the button would do.
            Debug.Log("Effect list cleared");
        }
        EditorUtility.SetDirty(panel);
    }
    private List<Effet> FindAll()
    {
        List<Effet> assets = new List<Effet>();
        string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(Effet)));
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            Effet asset = AssetDatabase.LoadAssetAtPath<Effet>(assetPath);
            if (asset != null)
            {
                assets.Add(asset);
            }
        }
        return assets;
    }
    private void LoadAllEffect(DebugStatPanel panel)
    {
        ClearAllEffect(panel);
        var list = FindAll();
        for (int i = 0; i < list.Count; i++)
        {
            panel.EffectList.Add(list[i]);
            serializedObject.FindProperty($"EffectList").arraySize++;
            serializedObject.FindProperty($"EffectList").GetArrayElementAtIndex(i).objectReferenceValue = list[i];
        }
        serializedObject.ApplyModifiedProperties();
        
    }
    private void ClearAllEffect(DebugStatPanel panel)
    {
        serializedObject.FindProperty($"EffectList").ClearArray();
        serializedObject.ApplyModifiedProperties();
        panel.EffectList.Clear();
    }
}
