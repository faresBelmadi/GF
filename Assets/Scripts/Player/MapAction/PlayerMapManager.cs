using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.PlayerSettings;

public class PlayerMapManager : MonoBehaviour
{
    [SerializeField] private GameObject _roomsHolder;
    [SerializeField] private float _rollingMapTime = 1f;

    [SerializeField] private Color basePathColor;
    [SerializeField] private Color visitedPathColors;

    public static event Action OnEndGame;

    public Room CurrentRoom
    {
        get { return _currentRoom; }
        set
        {
            _currentRoom = value;

            _currentRoom.SetAccessibleRooms();
            UpdateAllPathShaders();
            MapAction();
        }
    }

    private Room _currentRoom;

    public int mapUsedSeed = 0;
    public List<int> visitedMapIndexs = new List<int>() { 0};
    public List<Tuple<int, int>> roomSelectedEncounters = new List<Tuple<int, int>>();

    public List<MapNode> map = new List<MapNode>();
    public GameObject[,] pathsGameObjects;
    public class MapNode
    {
        public GameObject objectInstance;
        public TypeRoom roomType = TypeRoom.NONE;
        public List<int> connections = new List<int>();
    }

    //public GameObject MenuCamera;
    public GameObject CurrentRoomCamera;

    //GameObject[] rootScene;
    private Scene _scene;

    public static event Action OnShowMap;

    private void OnEnable()
    {
        GameManager.OnShowMap += FadeInAllRoom;
    }

    private void OnDisable()
    {
        GameManager.OnShowMap -= FadeInAllRoom;
    }

    private void FadeInAllRoom()
    {
        for (int i = 0; i < _roomsHolder.transform.childCount; i++)
        {
            _roomsHolder.transform.GetChild(i).gameObject.SetActive(true);
        }

        OnShowMap?.Invoke();
    }
    //private void GetAccessibleRooms()
    //{
    //    foreach (int connectedRoomId in map[_currentRoom.ID].connections)
    //    {
    //        Room connectedRoom = map[connectedRoomId].objectInstance.GetComponent<Room>();
    //         if (connectedRoom.roomState != RoomState.VISITED) connectedRoom.roomState = RoomState.ACCESSIBLE;
    //        connectedRoom.SetShaderByState(connectedRoom.roomState);

    //    }
    //}

    public void UpdateAllPathShaders()
    {
        for (int i = 0; i < map.Count; i++)
        {
            foreach (int connectedId in map[i].connections)
            {
                if (i > connectedId) continue;
            
                if(map[i].objectInstance.GetComponent<Room>().roomState == RoomState.UNKNOWN
                || map[connectedId].objectInstance.GetComponent<Room>().roomState == RoomState.UNKNOWN)
                {
                    pathsGameObjects[i, connectedId].GetComponent<LineRenderer>().material.SetFloat("_Intensity", -1f);
                    pathsGameObjects[i, connectedId].GetComponent<LineRenderer>().material.SetFloat("_curtainLength", -.5f);
                    pathsGameObjects[i, connectedId].GetComponent<LineRenderer>().material.SetColor("_Color", basePathColor);

                }
                else if (map[i].objectInstance.GetComponent<Room>().roomState == RoomState.VISITED
                && map[connectedId].objectInstance.GetComponent<Room>().roomState == RoomState.VISITED)
                {
                    pathsGameObjects[i, connectedId].GetComponent<LineRenderer>().material.SetFloat("_Intensity", 1f);
                    pathsGameObjects[i, connectedId].GetComponent<LineRenderer>().material.SetFloat("_curtainLength", .5f);
                    pathsGameObjects[i, connectedId].GetComponent<LineRenderer>().material.SetColor("_Color", visitedPathColors);
                }
                else
                {
                    pathsGameObjects[i, connectedId].GetComponent<LineRenderer>().material.SetFloat("_Intensity", 3f);
                    pathsGameObjects[i, connectedId].GetComponent<LineRenderer>().material.SetFloat("_curtainLength", 1f);
                    pathsGameObjects[i, connectedId].GetComponent<LineRenderer>().material.SetColor("_Color", basePathColor);

                }
            }            
        }
    }

    private void MapAction()
    {


        switch (_currentRoom.roomType)
        {
            case TypeRoom.ENCOUNTER:
                //StartCoroutine("LoadSceneAsync", "BattleScene Normal");
                //StartBattle("normal");
                StartChoosenBattel();
                //_currentRoom.roomState = RoomState.VISITED;
                break;
            case TypeRoom.CLASS_ENCOUNTER:
                //StartCoroutine("LoadSceneAsync", "BattleScene Normal");
                StartChoosenBattel();
                //StartBattle("class");
                //_currentRoom.roomState = RoomState.VISITED;
                break;
            case TypeRoom.ELITE:
                StartChoosenBattel();
                //StartBattle("elite");
                //_currentRoom.roomState = RoomState.VISITED;
                break;
            case TypeRoom.CLASS_ELITE:
                StartChoosenBattel();
                //StartBattle("class_elite");
                //_currentRoom.roomState = RoomState.VISITED;
                break;
            case TypeRoom.BOSS:
                StartChoosenBattel();
                //StartBattle("boss");
                //_currentRoom.roomState = RoomState.VISITED;
                //StartCoroutine("LoadSceneAsync", "BattleScene Boss");
                break;
            case TypeRoom.EXIT:
                SceneManager.LoadScene("MainMenu");
                Destroy(GameManager.Instance.gameObject);
                //StartCoroutine("LoadSceneAsync", "BattleScene Boss");
                break;
            //case TypeRoom.LevelUp:
            //    StartLevelUp();
            //    break;
            case TypeRoom.AUTEL:
                StartAutel();
                //_currentRoom.roomState = RoomState.VISITED;
                break;
            //StartAutel();
            //case TypeRoom.Heal:
            //    StartCoroutine("LoadSceneAsync", "Autel");
            //    break;
            case TypeRoom.RANDOM:
                StartAlea();
                //_currentRoom.roomState = RoomState.VISITED;
                break;
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

    //IEnumerator LoadSceneAsync(string name)
    //{
    //    var toLoad = name.Split(' ');
    //    //yield return SceneManager.LoadSceneAsync(toLoad[0], LoadSceneMode.Additive);
    //    //s = SceneManager.GetSceneByName(toLoad[0]);

    //   // rootScene = _scene.GetRootGameObjects();

    //    switch (name)
    //    {
    //        case "BattleScene Normal":
    //            StartBattle("normal");
    //            break;
    //        case "GameScene Normal":
    //            StartBattle("normal");
    //            break;
    //        case "BattleScene Elite":
    //            StartBattle("elite");
    //            break;
    //        case "BattleScene Boss":
    //            StartBattle("boss");
    //            break;
    //        case "LevelUp":

    //            break;
    //        case "GameScene AleaScene":
    //            StartAlea();
    //            break;
    //        case "Autel":
    //            StartAutel();
    //            break;
    //        //case "MenuStat":
    //        //    StartMenuStat();
    //        //    break;
    //        default:
    //            break;
    //    }
    //    yield return null;
    //}
    public void StartChoosenBattel()
    {
        ToggleMap(false);
        StartCoroutine(WaitStartBattle());
    }
    void StartBattle(string enemieType)
    {
        //CurrentRoomCamera = rootScene.First(c => c.name == "GameCamera");
        //GameManager.Instance.BattleMan = rootScene.First(c => c.name == "BattleManager").GetComponent<BattleManager>();
        ToggleMap(false);

        //StartCoroutine(WaitBeforeAction(GameManager.Instance.LoadCombat));
        //return;

        if (enemieType.Equals("normal"))
        {
            StartCoroutine(WaitBeforeAction(GameManager.Instance.LoadCombatNormal));
            //GameManager.Instance.LoadCombatNormal();
        }
        else if (enemieType.Equals("class"))
        {
            StartCoroutine(WaitBeforeAction(GameManager.Instance.LoadCombatClass));
            //GameManager.Instance.LoadCombatElite();
        }
        else if (enemieType.Equals("elite"))
        {
            StartCoroutine(WaitBeforeAction(GameManager.Instance.LoadCombatElite));
            //GameManager.Instance.LoadCombatElite();
        }
        else if (enemieType.Equals("class_elite"))
        {
            StartCoroutine(WaitBeforeAction(GameManager.Instance.LoadCombatClassElite));
            //GameManager.Instance.LoadCombatElite();
        }
        else if (enemieType.Equals("boss"))
        {
            StartCoroutine(WaitBeforeAction(GameManager.Instance.LoadCombatBoss));
            //GameManager.Instance.LoadCombatBoss();
        }
        else if (enemieType.Equals("Tuto"))
        {
            StartCoroutine(WaitBeforeAction(GameManager.Instance.LoadTuto));
        }
        //AudioManager.Instance.PlayMusic(MusicType.CombatMusic);

        //ToggleMap(false); //We hide the map
        //StartCoroutine(WaitBeforeAction(GameManager.Instance.LoadCombat));
        //CurrentRoomCamera.SetActive(true);
        //MenuCamera.SetActive(false);
    }

    public IEnumerator EndBattle(bool IsLoot)
    {
        //CurrentRoomCamera.SetActive(false);
        //GameManager.Instance.BattleMan = null;
        //MenuCamera.SetActive(true);
        if (CurrentRoom.roomType == TypeRoom.BOSS)
        {
            OnEndGame?.Invoke();
        }

        if (IsLoot)
        {
            //Afficher le menutStat
            // + PopUp new Souvenir
            GameManager.Instance.Loot();
            ShowMenuStat();
        }

        AudioManager.instance.PlayMusic(MusicType.MainMenuMusic);
        GameManager.Instance.UnloadCombat();
        //yield return SceneManager.UnloadSceneAsync(_scene);
        yield return null;


    }

    void StartAlea()
    {
        //CurrentRoomCamera = rootScene.First(c => c.name == "GameCamera");
        //GameManager.Instance.AleaMan = rootScene.First(c => c.name == "AleaManager").GetComponent<AleaManager>();
        //GameManager.Instance.LoadEvent();
        ToggleMap(false); //We hide the map
        StartCoroutine(WaitBeforeAction(GameManager.Instance.LoadEvent));
        //CurrentRoomCamera.SetActive(true);
        //MenuCamera.SetActive(false);
    }

    public IEnumerator EndAlea()
    {
        //CurrentRoomCamera.SetActive(false);
        GameManager.Instance.AleaMan = null;
        //MenuCamera.SetActive(true);
        GameManager.Instance.UnloadEvent();
        GameManager.Instance.ShowMap();
        yield return null;
        //yield return SceneManager.UnloadSceneAsync(_scene);
    }

    void StartLevelUp()
    {
        //SceneManager.LoadScene("Autel");
        GameManager.Instance.UiMondeMan.EnableSkillTree();
    }

    void StartAutel()
    {
        //CurrentRoomCamera = rootScene.First(c => c.name == "AutelCamera");
        ////GameManager.Instance.OldAutelMan = rootScene.First(c => c.name == "OldAutelManager").GetComponent<OldAutelManager>();
        ////GameManager.Instance.LoadAutel();
        //CurrentRoomCamera.SetActive(true);
        //MenuCamera.SetActive(false);

        GameManager.Instance.LoadAutel();
        ToggleMap(false); //We hide the map


        AudioManager.instance.PlayMusic(MusicType.LevelUpMusic);
    }

    public IEnumerator EndAutel(bool Loot)
    {
        //CurrentRoomCamera.SetActive(false);
        //GameManager.Instance.OldAutelMan = null;
        //MenuCamera.SetActive(true);
        if (Loot == true)
        {

            ShowMenuStat();
        }

        AudioManager.instance.PlayMusic(MusicType.MainMenuMusic);
        GameManager.Instance.ShowMap();
        yield return null;
        //yield return SceneManager.UnloadSceneAsync(_scene);
    }

    public void ShowMenuStat()
    {
        GameManager.Instance.UiMondeMan.EnableStat();
        //StartCoroutine("LoadSceneAsync", "MenuStat");
    }

    void StartMenuStat()
    {
        //CurrentRoomCamera = rootScene.First(c => c.name == "MenuStatCamera");
        //GameManager.Instance.StatMan = rootScene.First(c => c.name == "MenuStatManager").GetComponent<MenuStatManager>();
        //GameManager.Instance.ShowMenuStat();
        //CurrentRoomCamera.SetActive(true);
        //MenuCamera.SetActive(false);
    }

    public void EndMenuStat()
    {
        //CurrentRoomCamera.SetActive(false);
        GameManager.Instance.StatMan = null;
        //MenuCamera.SetActive(true);
        //yield return SceneManager.UnloadSceneAsync(s);

        if (TutoManager.Instance == null)
        {
            GameManager.Instance.UiMondeMan.EnableMonde();
            GameManager.Instance.ShowMap();
        }
        else
        {
            GameManager.Instance.UiMondeMan.DisableStat();
        }
    }

    public void ToggleMap(bool isShowing)
    {
        if (isShowing)
        {
            GameManager.Instance.ShowMap();
        }
        else
        {
            GameManager.Instance.HideMap();
        }
    }

    public IEnumerator WaitBeforeAction(Action actionToDo)
    {
        yield return new WaitForSeconds(_rollingMapTime);
        actionToDo();
    }
    public IEnumerator WaitStartBattle()
    {
        yield return new WaitForSeconds(_rollingMapTime);
        Debug.Log($"Start Combat type: {_currentRoom.roomType}, Id: {_currentRoom.selectedEncounterId}");
        GameManager.Instance.LoadChoosenCombat(_currentRoom.roomType,_currentRoom.selectedEncounterId);
    }
}