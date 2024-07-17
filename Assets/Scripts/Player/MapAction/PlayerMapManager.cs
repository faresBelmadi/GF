﻿using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        _currentRoom.GetComponent<SpriteRenderer>().color = Color.white;
        foreach (var item in _currentRoom.ConnectedRooms)
        {
            item.isNavigable = true;
            item.GetComponent<SpriteRenderer>().color = Color.white;
        }
        foreach (var item in _currentRoom.OwnedCorridors)
        {
            SetLineColor(item, Color.white);
        }
    }


    private void VisualUpdateOld()
    {
        if (_currentRoom != null)
        {
            _currentRoom.Type = TypeRoom.Visited;
            _currentRoom.gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
            foreach (var item in _currentRoom.ConnectedRooms)
            {
                item.isNavigable = false;
            }
            foreach (var item in _currentRoom.OwnedCorridors)
            {
                SetLineColor(item, Color.gray);
            }
        }
    }

    private static void SetLineColor(GameObject item, Color color)
    {
        var gradient = item.GetComponent<LineRenderer>().colorGradient;
        var colorKeys = gradient.colorKeys;
        for (var j = 0; j < colorKeys.Length; j++)
        {
            colorKeys[j].color = color;
        }

        gradient.colorKeys = colorKeys;
        item.GetComponent<LineRenderer>().colorGradient = gradient;
    }


    private void MapAction()
    {
        switch (_currentRoom.Type)
        {
            case TypeRoom.CombatNormal:
                StartCoroutine("LoadSceneAsync", "BattleScene Normal");
                _currentRoom.Type = TypeRoom.Visited;
                break;
            case TypeRoom.CombatElite:
                StartCoroutine("LoadSceneAsync", "BattleScene Elite");
                _currentRoom.Type = TypeRoom.Visited;
                break;
            case TypeRoom.CombatBoss:
                StartCoroutine("LoadSceneAsync", "BattleScene Boss");
                _currentRoom.Type = TypeRoom.Visited;
                //StartCoroutine("LoadSceneAsync", "BattleScene Boss");
                break;
            case TypeRoom.End:
                SceneManager.LoadScene("MainMenu");
                Destroy(GameManager.instance.gameObject);
                //StartCoroutine("LoadSceneAsync", "BattleScene Boss");
                break;
            case TypeRoom.LevelUp:
                StartLevelUp();
                break;
            case TypeRoom.Autel:
                StartCoroutine("LoadSceneAsync", "Autel");
                _currentRoom.Type = TypeRoom.Visited;
                break;
                //StartAutel();
            //case TypeRoom.Heal:
            //    StartCoroutine("LoadSceneAsync", "Autel");
            //    break;
            //case TypeRoom.Event:
            //    StartCoroutine("LoadSceneAsync", "AleaScene");
            //    _currentRoom.Type = TypeRoom.Visited;
            //    break;
            //case TypeRoom.Visited:
            //    break;
            //case TypeRoom.Spawn:
            //    break;
            //case TypeRoom.NotSet:
            //    break;
            default:
                break;
        }
    }

    IEnumerator LoadSceneAsync(string name)
    {
        var toLoad = name.Split(' ');
        yield return SceneManager.LoadSceneAsync(toLoad[0], LoadSceneMode.Additive);
        s = SceneManager.GetSceneByName(toLoad[0]);

        rootScene = s.GetRootGameObjects();

        switch (name)
        {
            case "BattleScene Normal":
                StartBattle("normal");
                break;
            case "BattleScene Elite":
                StartBattle("elite");
                break;
            case "BattleScene Boss":
                StartBattle("boss");
                break;
            case "LevelUp":

                break;
            //case "AleaScene":
            //    StartAlea();
            //    break;
            case "Autel":
                StartAutel();
                break;
            //case "MenuStat":
            //    StartMenuStat();
            //    break;
            default:
                break;
        }
    }
    
    void StartBattle(string enemieType)
    {
        CurrentRoomCamera = rootScene.First(c => c.name == "BattleCamera");
        GameManager.instance.BattleMan = rootScene.First(c => c.name == "BattleManager").GetComponent<BattleManager>();
        //if (enemieType.Equals("normal"))
        //    GameManager.instance.LoadCombatNormal();
        //else if (enemieType.Equals("elite"))
        //{
        //    GameManager.instance.LoadCombatElite();
        //}
        //else if (enemieType.Equals("boss"))
        //{
        //    GameManager.instance.LoadCombatBoss();
        //}
        //AudioManager.instance.PlayMusic(MusicType.CombatMusic);
        GameManager.instance.LoadCombat();
        CurrentRoomCamera.SetActive(true);
        MenuCamera.SetActive(false);
    }

    public IEnumerator EndBattle(bool IsLoot)
    {
        CurrentRoomCamera.SetActive(false);
        GameManager.instance.BattleMan = null;
        MenuCamera.SetActive(true);
        if (IsLoot)
        {
            //Afficher le menutStat
            // + PopUp new Souvenir
            ShowMenuStat();
        }
        AudioManager.instance.PlayMusic(MusicType.MainMenuMusic);
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

    void StartLevelUp()
    {
        //SceneManager.LoadScene("Autel");
        UiMondeManager uiMondeManager = GetComponent<UiMondeManager>();
        uiMondeManager.EnableSkillTree();
    }

    void StartAutel()
    {
        CurrentRoomCamera = rootScene.First(c => c.name == "AutelCamera");
        //GameManager.instance.OldAutelMan = rootScene.First(c => c.name == "OldAutelManager").GetComponent<OldAutelManager>();
        //GameManager.instance.LoadAutel();
        CurrentRoomCamera.SetActive(true);
        MenuCamera.SetActive(false);

        AudioManager.instance.PlayMusic(MusicType.LevelUpMusic);
    }

    public IEnumerator EndAutel(bool Loot)
    {
        CurrentRoomCamera.SetActive(false);
        //GameManager.instance.OldAutelMan = null;
        MenuCamera.SetActive(true);
        if (Loot == true)
        {
            ShowMenuStat();
        }
        
        AudioManager.instance.PlayMusic(MusicType.MainMenuMusic);
        yield return SceneManager.UnloadSceneAsync(s);
    }

    public void ShowMenuStat()
    {
        UiMondeManager uiMondeManager = GetComponent<UiMondeManager>();
        uiMondeManager.EnableStat();
        //StartCoroutine("LoadSceneAsync", "MenuStat");
    }

    void StartMenuStat()
    {
        //CurrentRoomCamera = rootScene.First(c => c.name == "MenuStatCamera");
        //GameManager.instance.StatMan = rootScene.First(c => c.name == "MenuStatManager").GetComponent<MenuStatManager>();
        //GameManager.instance.ShowMenuStat();
        //CurrentRoomCamera.SetActive(true);
        //MenuCamera.SetActive(false);
    }

    public void EndMenuStat()
    {
        //CurrentRoomCamera.SetActive(false);
        GameManager.instance.StatMan = null;
        //MenuCamera.SetActive(true);
        //yield return SceneManager.UnloadSceneAsync(s);

        UiMondeManager uiMondeManager = GetComponent<UiMondeManager>();
        uiMondeManager.EnableMonde();
    }
}