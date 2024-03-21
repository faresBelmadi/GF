using System.Collections.Generic;
using UnityEditor;

#if UNITY_EDITOR
//[CustomEditor(typeof(DialogueSO))]
public class DialogueInspector : Editor
{
    SerializedProperty Base;
    int sizeBase;
    bool showListBase;
    List<bool> toggled;
    List<bool> toggledList;
    List<bool> toggledConséquences;
    List<bool> showList;
    List<int> sizeList;
    List<int> sizeConséquence;

    private void OnEnable() {
        Base = serializedObject.FindProperty("Questions");
        toggled = new List<bool>();
        showList = new List<bool>();
        toggledList = new List<bool>();
        toggledConséquences = new List<bool>();
        sizeList = new List<int>();
        sizeConséquence = new List<int>();
    }
    public override void OnInspectorGUI() {
        serializedObject.Update();

        //foldout pour la list d'objet modifié
        showListBase = EditorGUILayout.BeginFoldoutHeaderGroup(showListBase,"Liste des questions");

        if(showListBase)
        {
            //changement de l'indentation
            EditorGUI.indentLevel = 1;
            sizeBase = Base.arraySize;
            sizeBase = EditorGUILayout.IntField("Taille de la liste", sizeBase);

            if(sizeBase != Base.arraySize)
            {
                while(sizeBase > Base.arraySize)
                {
                    Base.InsertArrayElementAtIndex(Base.arraySize);
                }
                while(sizeBase < Base.arraySize)
                {
                    Base.DeleteArrayElementAtIndex(Base.arraySize-1);
                }
            }

            //affichage des objet de notre custom class
            for (int i = 0; i < Base.arraySize; i++)
            {
                //ajout d'un boolean pour le foldout de l'objet
                toggled.Add(false);
                //recuperation de l'objet
                SerializedProperty MyListRef = Base.GetArrayElementAtIndex(i);
                //recuperation des propriété de l'objet
                SerializedProperty question = MyListRef.FindPropertyRelative("Question");
                SerializedProperty RéponsePossible = MyListRef.FindPropertyRelative("ReponsePossible");
                SerializedProperty Text = question.FindPropertyRelative("Text");
                SerializedProperty idSpeaker = question.FindPropertyRelative("IDSpeaker");
                //création du foldout de l'objet
                toggled[i] = EditorGUILayout.Foldout(toggled[i],"Question " + i);
                if(toggled[i])
                {
                    #region QuestionRéponse
                    //champ pour ajouter la Question
                    EditorGUILayout.PropertyField(idSpeaker);
                    EditorGUILayout.PropertyField(Text);
                    //création de l'affichage de la liste des Réponse
                    sizeList.Add(RéponsePossible.arraySize);
                    sizeList[i] = EditorGUILayout.IntField("Nb de réponse", sizeList[i]);
                    
                    if(sizeList[i] != RéponsePossible.arraySize)
                    {
                        while(sizeList[i] > RéponsePossible.arraySize)
                        {
                            RéponsePossible.InsertArrayElementAtIndex(RéponsePossible.arraySize);
                        }
                        while(sizeList[i] < RéponsePossible.arraySize)
                        {
                            RéponsePossible.DeleteArrayElementAtIndex(RéponsePossible.arraySize-1);
                        }
                    }
                    //affichage de la liste de réponse
                    for (int j = 0; j < RéponsePossible.arraySize; j++)
                    {
                        toggledList.Add(true);
                        SerializedProperty réponse = RéponsePossible.GetArrayElementAtIndex(j)   ;
                        SerializedProperty idNextQuestion = réponse.FindPropertyRelative("IDNextQuestion");
                        SerializedProperty textreponse = réponse.FindPropertyRelative("TexteRéponse");
                        SerializedProperty conséquences = réponse.FindPropertyRelative("conséquences");
                        EditorGUI.indentLevel = 2;
                        toggledList[j] = EditorGUILayout.Foldout(toggledList[j],"Réponse " + j);
                        if(toggledList[j])
                        {
                            EditorGUILayout.PropertyField(idNextQuestion);
                            EditorGUILayout.PropertyField(textreponse);
                            #region conséquences
                            //création de l'affichage de la liste des réponse
                            sizeConséquence.Add(conséquences.arraySize);
                            sizeConséquence[j] = EditorGUILayout.IntField("Nb de conséquences", sizeConséquence[j]);
                            
                            if(sizeConséquence[j] != conséquences.arraySize)
                            {
                                while(sizeConséquence[j] > conséquences.arraySize)
                                {
                                    conséquences.InsertArrayElementAtIndex(conséquences.arraySize);
                                }
                                while(sizeConséquence[j] < conséquences.arraySize)
                                {
                                    conséquences.DeleteArrayElementAtIndex(conséquences.arraySize-1);
                                }
                            }
                            //affichage de la liste de réponse
                            for (int k = 0; k < conséquences.arraySize; k++)
                            {
                                toggledConséquences.Add(true);
                                SerializedProperty conséquence = conséquences.GetArrayElementAtIndex(k);
                                EditorGUI.indentLevel = 3;
                                EditorGUILayout.ObjectField(conséquence);
                            }
                            EditorGUI.indentLevel = 2;
                            #endregion conséquences

                        }
                    }
                    EditorGUI.indentLevel = 1;
                    #endregion QuestionRéponse
                }
                
            }
        }

        EditorGUILayout.EndFoldoutHeaderGroup();
        
        
        serializedObject.ApplyModifiedProperties();
    }
}
#endif