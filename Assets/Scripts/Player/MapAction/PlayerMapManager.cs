using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMapManager : MonoBehaviour
{
    [SerializeField]
    private float _rollingMapTime = 1f;

    public Room CurrentRoom
    {
        get
        {
            return _currentRoom;
        }
        set
        {
            //VisualUpdateOld();
            _currentRoom = value;

            GetAccessibleRooms();
            //VisualUpdateNew();
            MapAction();
        }
    }
    private Room _currentRoom;
    public List<MapNode> map = new List<MapNode>();

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
    private void GetAccessibleRooms()
    {
        foreach (int connectedRoomId in map[_currentRoom.ID].connections)
        {
            Room connectedRoom = map[connectedRoomId].objectInstance.GetComponent<Room>();
             if (connectedRoom.roomState != RoomState.VISITED) connectedRoom.roomState = RoomState.ACCESSIBLE;
            connectedRoom.SetColorByState(connectedRoom.roomState);
        }
    }
    private void VisualUpdateNew()
    {
        //_currentRoom.GetComponent<SpriteRenderer>().color = Color.white;
        //_currentRoom.ChangeColor(Color.white);
        _currentRoom.SetColorByState(_currentRoom.roomState);
        //foreach (var item in _currentRoom.ConnectedRooms)
        //{
        //    item.isNavigable = true;
        //    //item.GetComponent<SpriteRenderer>().color = Color.white;
        //    item.ChangeColor(Color.white);
        //}
        //foreach (var item in _currentRoom.OwnedCorridors)
        //{
        //    SetLineColor(item, Color.white);
        //}
    }


    private void VisualUpdateOld()
    {
        if (_currentRoom != null)
        {
            _currentRoom.roomState = RoomState.VISITED;
            _currentRoom.SetColorByState(_currentRoom.roomState);
            //_currentRoom.gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
            //_currentRoom.ChangeColor(Color.gray);
            //foreach (var item in _currentRoom.ConnectedRooms)
            //{
            //    item.isNavigable = false;
            //}
            //foreach (var item in _currentRoom.OwnedCorridors)
            //{
            //    SetLineColor(item, Color.gray);
            //}
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
        switch (_currentRoom.roomType)
        {
            case TypeRoom.ENCOUNTER:
                //StartCoroutine("LoadSceneAsync", "BattleScene Normal");
                StartBattle("normal");
                //_currentRoom.roomState = RoomState.VISITED;
                break;
            case TypeRoom.CLASS_ENCOUNTER:
                //StartCoroutine("LoadSceneAsync", "BattleScene Normal");
                StartBattle("class");
                //_currentRoom.roomState = RoomState.VISITED;
                break;
            case TypeRoom.ELITE:
                StartBattle("elite");
                //_currentRoom.roomState = RoomState.VISITED;
                break;
            case TypeRoom.CLASS_ELITE:
                StartBattle("class_elite");
                //_currentRoom.roomState = RoomState.VISITED;
                break;
            case TypeRoom.BOSS:
                StartBattle("boss");
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
        UiMondeManager uiMondeManager = GetComponent<UiMondeManager>();
        uiMondeManager.EnableSkillTree();
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
        UiMondeManager uiMondeManager = GetComponent<UiMondeManager>();
        uiMondeManager.EnableStat();
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

        UiMondeManager uiMondeManager = GetComponent<UiMondeManager>();
        uiMondeManager.EnableMonde();
        GameManager.Instance.ShowMap();
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
}