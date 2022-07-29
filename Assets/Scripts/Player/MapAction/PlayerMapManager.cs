﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PlayerMapManager : MonoBehaviour
{
    public Room CurrentRoom
    {
        get
        {
            return _currentRoom;
        }
        set
        {
            VisualUpdateOld();
            _currentRoom = value;
            VisualUpdateNew();
            MapAction();
        }
    }
    private Room _currentRoom;

    public GameObject MenuCamera;
    public GameObject CurrentRoomCamera;
    GameObject[] rootScene;
    Scene s;

    private void VisualUpdateNew()
    {
        foreach (var item in _currentRoom.ConnectedRooms)
        {
            item.isNavigable = true;
        }
        foreach (var item in _currentRoom.OwnedCorridors)
        {
            item.GetComponent<MaterialSelector>().isSelected = true;
        }
    }

    private void VisualUpdateOld()
    {
        if(_currentRoom != null)
        {
            _currentRoom.Type = TypeRoom.Visited;
            foreach (var item in _currentRoom.ConnectedRooms)
            {
                item.isNavigable = false;
            }
            foreach (var item in _currentRoom.OwnedCorridors)
            {
                item.GetComponent<MaterialSelector>().isSelected = false;
            }
        }
    }

    private void MapAction()
    {
        switch (_currentRoom.Type)
        {
            case TypeRoom.Spawn:
                break;
            case TypeRoom.NotSet:
                break;
            case TypeRoom.Combat:
                StartCoroutine("LoadSceneAsync","BattleScene");
                _currentRoom.Type = TypeRoom.Visited;
                break;
            case TypeRoom.Heal:
                break;
            case TypeRoom.Event:
                StartCoroutine("LoadSceneAsync", "AleaScene");
                _currentRoom.Type = TypeRoom.Visited;
                break;
            case TypeRoom.Visited:
                break;
            default:
            break;
        }
    }

    IEnumerator LoadSceneAsync(string name)
    {
        yield return SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        s = SceneManager.GetSceneByName(name);

        rootScene = s.GetRootGameObjects();

        switch(name)
        {
            case "BattleScene":
                StartBattle();
                break;
            case "AleaScene":
                StartAlea();
                break;
            case "MenuStat":
                StartMenuStat();
                break;
            default:
            break;
        }

       
        
    }

    void StartBattle()
    {
        CurrentRoomCamera = rootScene.First(c => c.name == "BattleCamera");
        GameManager.instance.BattleMan = rootScene.First(c => c.name == "BattleManager").GetComponent<BattleManager>();
        GameManager.instance.LoadCombat();
        CurrentRoomCamera.SetActive(true);
        MenuCamera.SetActive(false);
    }

    public IEnumerator EndBattle()
    {
        CurrentRoomCamera.SetActive(false);
        GameManager.instance.BattleMan = null;
        MenuCamera.SetActive(true);
        yield return SceneManager.UnloadSceneAsync(s);
    }

    void StartAlea()
    {
        CurrentRoomCamera = rootScene.First(c => c.name == "AleaCamera");
        GameManager.instance.AleaMan = rootScene.First(c => c.name == "AleaManager").GetComponent<AleaManager>();
        GameManager.instance.LoadEvent();
        CurrentRoomCamera.SetActive(true);
        MenuCamera.SetActive(false);
    }

    public IEnumerator EndAlea()
    {
        CurrentRoomCamera.SetActive(false);
        GameManager.instance.AleaMan = null;
        MenuCamera.SetActive(true);
        yield return SceneManager.UnloadSceneAsync(s);
    }

    public void LoadMenuStat()
    {
        StartCoroutine("LoadSceneAsync", "MenuStat");
    }

    void StartMenuStat()
    {
        CurrentRoomCamera = rootScene.First(c => c.name == "MenuStatCamera");
        CurrentRoomCamera.SetActive(true);
        MenuCamera.SetActive(false);
    }

    public IEnumerator EndMenuStat()
    {
        CurrentRoomCamera.SetActive(false);
        MenuCamera.SetActive(true);
        yield return SceneManager.UnloadSceneAsync(s);
    }
}
