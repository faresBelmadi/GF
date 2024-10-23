using System;
using System.Collections;
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
            VisualUpdateOld();
            _currentRoom = value;
            VisualUpdateNew();
            MapAction();
        }
    }
    private Room _currentRoom;

    //public GameObject MenuCamera;
    public GameObject CurrentRoomCamera;
    //GameObject[] rootScene;
    private Scene _scene;

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
                //StartCoroutine("LoadSceneAsync", "BattleScene Normal");
                StartCoroutine("LoadSceneAsync", "GameScene Normal");
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
                Destroy(GameManager.Instance.gameObject);
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
            case TypeRoom.Event:
                StartCoroutine("LoadSceneAsync", "GameScene AleaScene");
                _currentRoom.Type = TypeRoom.Visited;
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

    IEnumerator LoadSceneAsync(string name)
    {
        var toLoad = name.Split(' ');
        //yield return SceneManager.LoadSceneAsync(toLoad[0], LoadSceneMode.Additive);
        //s = SceneManager.GetSceneByName(toLoad[0]);

       // rootScene = _scene.GetRootGameObjects();

        switch (name)
        {
            case "BattleScene Normal":
                StartBattle("normal");
                break;
            case "GameScene Normal":
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
            case "GameScene AleaScene":
                StartAlea();
                break;
            case "Autel":
                StartAutel();
                break;
            //case "MenuStat":
            //    StartMenuStat();
            //    break;
            default:
                break;
        }
        yield return null;
    }
    
    void StartBattle(string enemieType)
    {
        //CurrentRoomCamera = rootScene.First(c => c.name == "GameCamera");
        //GameManager.Instance.BattleMan = rootScene.First(c => c.name == "BattleManager").GetComponent<BattleManager>();

        //if (enemieType.Equals("normal"))
        //    GameManager.Instance.LoadCombatNormal();
        //else if (enemieType.Equals("elite"))
        //{
        //    GameManager.Instance.LoadCombatElite();
        //}
        //else if (enemieType.Equals("boss"))
        //{
        //    GameManager.Instance.LoadCombatBoss();
        //}
        //AudioManager.Instance.PlayMusic(MusicType.CombatMusic);

        ToggleMap(false); //We hide the map
        StartCoroutine(WaitBeforeAction(GameManager.Instance.LoadCombat));
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
        GameManager.Instance.LoadEvent();
        //CurrentRoomCamera.SetActive(true);
        //MenuCamera.SetActive(false);
    }

    public IEnumerator EndAlea()
    {
        //CurrentRoomCamera.SetActive(false);
        GameManager.Instance.AleaMan = null;
        //MenuCamera.SetActive(true);
        GameManager.Instance.UnloadEvent();
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
        yield return SceneManager.UnloadSceneAsync(_scene);
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