using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(Spell))]
public class SpellInspector : Editor
{
    List<SerializedProperty> Base;
    SerializedProperty Effet;
    SerializedProperty DebuffsBuffs;
    SerializedProperty TypeAcharnement;

    int sizeEffet;
    int sizeBuffs;
    List<bool> toggled;
    bool showListEffet;
    bool showListBuffs;

    private void OnEnable() {
        Base = new List<SerializedProperty>();
        toggled = new List<bool>();
        Base.Add(serializedObject.FindProperty("Nom"));
        Base.Add(serializedObject.FindProperty("Sprite"));
        Base.Add(serializedObject.FindProperty("DescriptionObject"));
        Base.Add(serializedObject.FindProperty("IDSpell"));
        Base.Add(serializedObject.FindProperty("IDChildren"));
        Base.Add(serializedObject.FindProperty("SpellStatue"));
        Base.Add(serializedObject.FindProperty("IsAvailable"));
        Base.Add(serializedObject.FindProperty("CostUnlock"));
        Base.Add(serializedObject.FindProperty("Costs"));
        Effet = serializedObject.FindProperty("ActionEffet");
        DebuffsBuffs = serializedObject.FindProperty("ActionBuffDebuff");
    }
    public override void OnInspectorGUI() {
        serializedObject.Update();

        //affichage des propriété non modifié
        foreach (var item in Base)
        {
            EditorGUILayout.PropertyField(item);
        }

        //foldout pour la list d'objet modifié
        showListEffet = EditorGUILayout.BeginFoldoutHeaderGroup(showListEffet,"Liste d'effet");

        if(showListEffet)
        {
            //changement de l'indentation
            EditorGUI.indentLevel = 1;
            //création de la variable pour le changement de taille de la liste
            sizeEffet = Effet.arraySize;
            sizeEffet = EditorGUILayout.IntField("Taille de la liste", sizeEffet);

            if(sizeEffet != Effet.arraySize)
            {
                while(sizeEffet > Effet.arraySize)
                {
                    Effet.InsertArrayElementAtIndex(Effet.arraySize);
                }
                while(sizeEffet < Effet.arraySize)
                {
                    Effet.DeleteArrayElementAtIndex(Effet.arraySize-1);
                }
            }

            //affichage des objet de notre custom class
            for (int i = 0; i < Effet.arraySize; i++)
            {

                //recuperation de l'objet
                SerializedProperty MyListRef = Effet.GetArrayElementAtIndex(i);

                EditorGUILayout.ObjectField(MyListRef);
                
            }
        }
        
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUI.indentLevel = 0;
        
        //foldout pour la list d'objet modifié
        showListBuffs = EditorGUILayout.BeginFoldoutHeaderGroup(showListBuffs,"Liste de Buffs/Debuffs");

        if(showListBuffs)
        {
            //changement de l'indentation
            EditorGUI.indentLevel = 1;
            //création de la variable pour le changement de taille de la liste
            sizeBuffs = DebuffsBuffs.arraySize;
            sizeBuffs = EditorGUILayout.IntField("Taille de la liste", sizeBuffs);

            if(sizeBuffs != DebuffsBuffs.arraySize)
            {
                while(sizeBuffs > DebuffsBuffs.arraySize)
                {
                    DebuffsBuffs.InsertArrayElementAtIndex(DebuffsBuffs.arraySize);
                }
                while(sizeBuffs < DebuffsBuffs.arraySize)
                {
                    DebuffsBuffs.DeleteArrayElementAtIndex(DebuffsBuffs.arraySize-1);
                }
            }

            //affichage des objet de notre custom class
            for (int i = 0; i < DebuffsBuffs.arraySize; i++)
            {
 
                //recuperation de l'objet
                SerializedProperty MyListRef = DebuffsBuffs.GetArrayElementAtIndex(i);

                    
                EditorGUILayout.ObjectField(MyListRef);
                
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        
        
        serializedObject.ApplyModifiedProperties();
    }
}
#endif