using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{

    public int savedClass;
    // Start is called before the first frame update
    void Start()
    {
        CheckSave();
    }

    void CheckSave()
    {
#if UNITY_EDITOR
        string path = "Assets/SavedData/GameData/Game.json";
#else
        string path = Application.persistentDataPath + "/SavedData/GameData/Game.json";
#endif
        string dataAsJson;
        if (File.Exists(path))
        {
            PlayerPrefs.SetInt("hasSave", 1);
            dataAsJson = File.ReadAllText(path);

            // Pass the json to JsonUtility, and tell it to create a SkillTree object from it
            var loadedData = JsonUtility.FromJson<GameData>(dataAsJson);
            if (!loadedData.CurrentRun.Ended)
            {
                this.gameObject.GetComponent<Button>().interactable = true;
                savedClass = loadedData.CurrentRun.ClassID;
            }
            else
            {
                this.gameObject.GetComponent<Button>().interactable = false;
            }

        }
        else

            PlayerPrefs.SetInt("hasSave", 0);
    }
}
