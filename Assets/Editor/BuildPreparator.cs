using GF.GgsToCsv;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GF.Tools.Build
{
    public class BuildPreparator
    {
        private const string _mainScenePath = "Assets/Scenes/GameScene.unity";

        private const string _gameManagerGameObjectName = "GameManager";
        private const string _encounterSetPath = "Assets/Asset_in_game/Rencontre/DemoEncounterSetData.asset";
        private const string _souvenirListDataPath = "Assets/Asset_in_game/Souvenir/DropList Souvenir/WarriorSouvenirListData.asset";

        private const string _guerrierClassPath = "Assets/Asset_in_game/Personnage_joueur/Guerrier/Stats_Guerrier/Guerrier_Gains.asset";
        private const string _guerrierStatPath = "Assets/Asset_in_game/Personnage_joueur/Guerrier/Stats_Guerrier/Stat_Guerrier.asset";

        private const string _roomsGameObjectName = "Rooms";

        public void Prepare()
        {
            ImportLocaFiles();

            EnsureMainSceneLoaded();
            EnsureGameManagerReferences();
            EnsureRoomsReferences();

            AssetDatabase.SaveAssets();
        }

        private void ImportLocaFiles()
        {
            var importer = new GgsToCsvImporter();
            importer.ImportAll();
        }

        private void EnsureMainSceneLoaded()
        {
            bool needLoad = false;
            if (SceneManager.sceneCount == 0)
            {
                Debug.Log($"No scene loaded and we need {Path.GetFileName(_mainScenePath)}.");
                needLoad = true;
            }
            else if (SceneManager.sceneCount == 1)
            {
                var scene = SceneManager.GetSceneAt(0);
                if (scene.path != _mainScenePath)
                {
                    Debug.Log($"Loaded scene is {Path.GetFileName(scene.path)} and we need {Path.GetFileName(_mainScenePath)}.");
                    needLoad = true;
                }
            }
            else
            {
                Debug.Log($"Too many scenes loaded and we need only {Path.GetFileName(_mainScenePath)}.");
                needLoad = true;
            }

            if (needLoad)
            {
                EditorSceneManager.OpenScene(_mainScenePath);
                Debug.Log($"Scene {Path.GetFileName(_mainScenePath)} have been loaded.");
            }
        }

        private void EnsureGameManagerReferences()
        {
            var gameMgrGO = GameObject.Find(_gameManagerGameObjectName);
            if (gameMgrGO == null)
            {
                Debug.LogError($"No GameObject with name {_gameManagerGameObjectName} found in scene.");
                return;
            }

            var gameMgr = gameMgrGO.GetComponent<GameManager>();
            if (gameMgr == null)
            {
                Debug.LogError($"No component {nameof(GameManager)} found on gameobject {_gameManagerGameObjectName}.", gameMgrGO);
                return;
            }

            EnsureReferenceAtPath<EncounterSetData>(gameMgr, "<EncounterSet>k__BackingField", _encounterSetPath, prettyPropertyName: "EncounterSet", logContextGO:gameMgrGO);
            EnsureReferenceAtPath<SouvenirListData>(gameMgr, "_souvenirListData", _souvenirListDataPath, prettyPropertyName: "SouvenirListData", logContextGO: gameMgrGO);

            var pathAndClasses = gameMgr.AllClasses.ToDictionary(c => AssetDatabase.GetAssetPath(c), c => c);
            if (pathAndClasses.TryGetValue(_guerrierClassPath, out var guerrierClass))
            {
                EnsureReferenceAtPath<JoueurStat>(guerrierClass, nameof(guerrierClass.PlayerStat), _guerrierStatPath, prettyReferencerName:"Guerrier's ClassPlayer", logContextGO:guerrierClass);
            }
            else
            {
                Debug.LogError($"No player class in {nameof(GameManager)} references path {_guerrierClassPath}.", guerrierClass);
                return;
            }
        }

        private void EnsureRoomsReferences()
        {
            var roomsGO = GameObject.Find(_roomsGameObjectName);
            if (roomsGO == null)
            {
                Debug.LogError($"No GameObject with name {_roomsGameObjectName} found in scene.");
                return;
            }

            var generator = roomsGO.GetComponent<Generator>();
            if (generator == null)
            {
                Debug.LogError($"No component {nameof(Generator)} found on gameobject {_roomsGameObjectName}.", roomsGO);
                return;
            }

            EnsureValue(generator, "seed", 0, prettyHolderName:"Rooms' Generator", logContextGO:roomsGO);
            EnsureValue(generator, "doCycle", false, prettyHolderName:"Rooms' Generator", logContextGO:roomsGO);
        }

        private void EnsureReferenceAtPath<T>(Object referencer, string propertyName, string expectedPath,
                                              string prettyReferencerName = null, string prettyPropertyName = null, Object logContextGO = null)
            where T : Object
        {
            if (prettyReferencerName == null)
                prettyReferencerName = referencer.name;
            if (prettyPropertyName == null)
                prettyPropertyName = propertyName;

            SerializedObject so = new SerializedObject(referencer);
            SerializedProperty property = so.FindProperty(propertyName);

            if (property == null)
            {
                Debug.LogError($"There is no property {propertyName} in {prettyReferencerName}.", logContextGO);
                // uncomment this if you need help finding your property
                //LogAllProperties(so);
                return;
            }

            if (property.propertyType != SerializedPropertyType.ObjectReference)
            {
                Debug.LogError($"Property {prettyPropertyName} in {prettyReferencerName} is not of an ObjectReference, reference cannot be ensured this way!");
                return;
            }

            T expectedAsset = AssetDatabase.LoadAssetAtPath<T>(expectedPath);
            if (expectedAsset == null)
            {
                Debug.LogError($"There is not asset at path {expectedPath}.");
                return;
            }

            if (property.objectReferenceValue == expectedAsset)
            {
                return; // all good
            }

            var oldRefPath = AssetDatabase.GetAssetPath(property.objectReferenceValue);
            Debug.Log($"{prettyReferencerName}'s {prettyPropertyName} referenced {(string.IsNullOrEmpty(oldRefPath) ? "element in scene" : $"{oldRefPath}")}.", logContextGO);
            property.objectReferenceValue = expectedAsset;
            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(referencer);
            Debug.Log($"{prettyReferencerName}'s {prettyPropertyName} now references {expectedPath}.", logContextGO);
        }

        private void EnsureValue(Object holder, string propertyName, object expectedValue,
                                 string prettyHolderName = null, string prettyPropertyName = null, Object logContextGO = null)
        {
            if (prettyHolderName == null)
                prettyHolderName = holder.name;
            if (prettyPropertyName == null)
                prettyPropertyName = propertyName;

            SerializedObject so = new SerializedObject(holder);
            SerializedProperty property = so.FindProperty(propertyName);

            if (property == null)
            {
                Debug.LogError($"There is not property {propertyName} in {prettyHolderName}.", logContextGO);
                // uncomment this if you need help finding your property
                //LogAllProperties(so);
                return;
            }

            switch (expectedValue)
            {
                case bool expectedBool:
                    if (property.boolValue == expectedBool)
                        return;
                    Debug.Log($"{prettyHolderName}'s {prettyPropertyName} was {property.boolValue}.", logContextGO);
                    property.boolValue = expectedBool;
                    break;

                case int expectedInt:
                    if (property.intValue == expectedInt)
                        return;
                    Debug.Log($"{prettyHolderName}'s {prettyPropertyName} was {property.intValue}.", logContextGO);
                    property.intValue = expectedInt;
                    break;

                default:
                    Debug.LogError($"Expected value is of type {expectedValue.GetType().Name} and it's not implemented.");
                    break;
            }

            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(holder);
            Debug.Log($"{prettyHolderName}'s {prettyPropertyName} now is {expectedValue}.", logContextGO);
        }

        /// <summary>
        /// Utility method to log all properties in a given SerializedObject.
        /// Generated backing fields, for example, can be trickily named :)
        /// </summary>
        /// <param name="so"></param>
        private void LogAllProperties(SerializedObject so)
        {
            SerializedProperty propertyIterator = so.GetIterator();
            List<string> props = new List<string>();
            bool expanded = propertyIterator.Next(true);
            while (expanded)
            {
                props.Add(propertyIterator.propertyPath);
                expanded = propertyIterator.Next(propertyIterator.hasVisibleChildren);
            }
            Debug.Log(string.Join("\n", props));
        }
    }
}