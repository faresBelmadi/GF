using UnityEditor;
using System.Collections.Generic;

#if UNITY_EDITOR
[CustomEditor(typeof(SkillTreePrinter))]
public class SkillTreePrinterInspector : Editor {
   List<SerializedProperty> UIProperty;
   List<SerializedProperty> TextProperty;

    List<SerializedProperty> SpawnableProperty;

   bool ShowUI,ShowText,ShowNodes;

   private void OnEnable() {
       UIProperty = new List<SerializedProperty>();
       TextProperty = new List<SerializedProperty>();
       SpawnableProperty = new List<SerializedProperty>();
       UIProperty.Add(serializedObject.FindProperty("SkillTreeContainer"));
       UIProperty.Add(serializedObject.FindProperty("InspectorContainer"));
       UIProperty.Add(serializedObject.FindProperty("ScrollView"));
       TextProperty.Add(serializedObject.FindProperty("EssenceHeader"));
       TextProperty.Add(serializedObject.FindProperty("ClassTitle"));
       TextProperty.Add(serializedObject.FindProperty("SpellTitle"));
       TextProperty.Add(serializedObject.FindProperty("SpellDescription"));
       TextProperty.Add(serializedObject.FindProperty("SpellCost"));
       TextProperty.Add(serializedObject.FindProperty("SpellCombatCost"));
       SpawnableProperty.Add(serializedObject.FindProperty("SpawnedNodes"));
       SpawnableProperty.Add(serializedObject.FindProperty("SpawnedLines"));
       SpawnableProperty.Add(serializedObject.FindProperty("NodePrefab"));
       SpawnableProperty.Add(serializedObject.FindProperty("LinePrefab"));
       SpawnableProperty.Add(serializedObject.FindProperty("ListJson"));
   }

   public override void OnInspectorGUI() {
        serializedObject.Update();
 
        ShowUI = EditorGUILayout.BeginFoldoutHeaderGroup(ShowUI , "UI Container");
        if(ShowUI) 
        {
            foreach (var item in UIProperty)
            {
                EditorGUILayout.PropertyField(item);
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();              
        ShowText = EditorGUILayout.BeginFoldoutHeaderGroup(ShowText , "Text mesh");
        if(ShowText) 
        {
            foreach (var item in TextProperty)
            {
                EditorGUILayout.PropertyField(item);
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();        
        ShowNodes = EditorGUILayout.BeginFoldoutHeaderGroup(ShowNodes , "Nodes");
        if(ShowNodes) 
        {
            foreach (var item in SpawnableProperty)
            {
                EditorGUILayout.PropertyField(item);
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        
 
        serializedObject.ApplyModifiedProperties();
   }
}
#endif