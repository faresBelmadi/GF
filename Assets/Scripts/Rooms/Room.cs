using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;


public enum TypeRoom
{
    NONE,
    START,
    LOOT,
    AUTEL,
    RANDOM,
    CLASS_RANDOM,
    ENCOUNTER,
    CLASS_ENCOUNTER,
    ELITE,
    CLASS_ELITE,
    BOSS,
    EXIT,
}

public enum RoomState
{
    UNKNOWN,
    ACCESSIBLE,
    VISITED
}

[System.Serializable]
public class Room : MonoBehaviour
{
    [SerializeField] private RoomData roomData;
    public TypeRoom roomType;
    public RoomState roomState;
    
    public Sprite spriteToSet;
    //public List<GameObject> OwnedCorridors = new List<GameObject>();

    public int ID;
    //public List<Room> ConnectedRooms = new List<Room>();
    //public bool isStart;
    //public bool isNavigable;
    [SerializeField]
    private GameObject roomIconObject;
    [SerializeField]
    private TMP_Text _roomText;

    [SerializeField] private Material baseRoomMaterial;

    public int selectedEncounterId;

    private string _labelID;
    private Vector3 oldScale;
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private float _fadeDuration = 1f;
    private void OnEnable()
    {
        TradManager.OnRefreshTranslation += RefreshLabel;
        PlayerMapManager.OnShowMap += FadeIn;
        GameManager.OnHideMap += FadeOut;
    }
    private void OnDisable()
    {
        TradManager.OnRefreshTranslation -= RefreshLabel;
        PlayerMapManager.OnShowMap -= FadeIn;
        GameManager.OnHideMap -= FadeOut;
    }
    private void Start() 
    {
        oldScale = roomIconObject.transform.localScale;
        _spriteRenderer = roomIconObject.GetComponent<SpriteRenderer>();
    }
  

    public void SetRoom(int roomId, TypeRoom type, float defaultSpriteSize, RoomState state = RoomState.UNKNOWN)
    {
        this.ID = roomId;
        this.roomType = type;
        this.roomState = state;
        gameObject.transform.localScale = new Vector3(defaultSpriteSize, defaultSpriteSize);
        roomIconObject.GetComponent<SpriteRenderer>().sprite = GetSpriteByRoomType(type);
        roomIconObject.GetComponent<SpriteRenderer>().material = new Material(baseRoomMaterial);
        _roomText.text = GetLabelByRoomType(type);
        SetShaderByState(roomState);
        //SetColorByState(roomState);
        //this.GetComponent<Image>().sprite = ToSet;
    }
    //public void SetEncounter(int EncounterId)
    //{
    //    selectedEncounterId = EncounterId;
    //}

    public void SetAccessibleRooms()
    {
        List<PlayerMapManager.MapNode> map = GameManager.Instance.pmm.map;
        foreach (int connectedRoomId in map[ID].connections)
        {
            Room connectedRoom = map[connectedRoomId].objectInstance.GetComponent<Room>();
            if (connectedRoom.roomState != RoomState.VISITED) connectedRoom.roomState = RoomState.ACCESSIBLE;
            connectedRoom.SetShaderByState(connectedRoom.roomState);

        }
    }


    private Sprite GetSpriteByRoomType(TypeRoom roomType)
    {
        switch (roomType)
        {
            case TypeRoom.NONE:
                return roomData.spriteUnkown;

            case TypeRoom.START:
                return roomData.spriteStart;

            case TypeRoom.LOOT:
                return roomData.spriteLoot;
            
            case TypeRoom.AUTEL:
                return roomData.spriteAutel;

            case TypeRoom.RANDOM:
                return roomData.spriteRandom;

            case TypeRoom.CLASS_RANDOM:
                return roomData.spriteClassRandom;

            case TypeRoom.ENCOUNTER:
                return roomData.spriteEncounter;

            case TypeRoom.CLASS_ENCOUNTER:
                return roomData.spriteClassEncounter;

            case TypeRoom.ELITE:
                return roomData.spriteElite;
                            
            case TypeRoom.CLASS_ELITE:
                return roomData.spriteClassElite;

            case TypeRoom.BOSS:
                return roomData.spriteBoss;
            
            case TypeRoom.EXIT:
                return roomData.spriteExit;

            default: return roomData.spriteUnkown;
        }
    }

    private string GetLabelByRoomType(TypeRoom type)
    {
        string rawLabel = "";
        switch(type)
        {
            case TypeRoom.NONE:
                rawLabel = roomData.labelUnkown;
                break;
            case TypeRoom.START:
                rawLabel = roomData.labelStart;
                break;

            case TypeRoom.LOOT:
                rawLabel = roomData.labelLoot;
                break;

            case TypeRoom.AUTEL:
                rawLabel = roomData.labelAutel;
                break;

            case TypeRoom.RANDOM:
                rawLabel = roomData.labelRandom;
                break;

            case TypeRoom.CLASS_RANDOM:
                rawLabel = roomData.labelClassRandom;
                break;

            case TypeRoom.ENCOUNTER:
                rawLabel = roomData.labelEncounter;
                break;

            case TypeRoom.CLASS_ENCOUNTER:
                rawLabel = roomData.labelClassEncounter;
                break;

            case TypeRoom.ELITE:
                rawLabel = roomData.labelElite;
                break;

            case TypeRoom.CLASS_ELITE:
                rawLabel = roomData.labelClassElite;
                break;

            case TypeRoom.BOSS:
                rawLabel = roomData.labelBoss;
                break;

            case TypeRoom.EXIT:
                rawLabel = roomData.labelExit;
                break;

            default: rawLabel = roomData.labelUnkown;
                break;
        }
        return TradManager.instance.GetTranslation(rawLabel);
    }
    public void SetColorByState(RoomState roomstate)
    {
        //_roomObject.GetComponent<SpriteRenderer>().color = roomstate == RoomState.UNKNOWN ? Color.black: Color.white;
        Color spriteColor;
        switch (roomstate)
        {
            case RoomState.UNKNOWN:
                spriteColor = Color.black;
                break;
            
            case RoomState.ACCESSIBLE:
                spriteColor = Color.white;
                break;

            case RoomState.VISITED:
                spriteColor = Color.cyan;
                break;

            default : spriteColor = Color.white;
                break;
        }

        roomIconObject.GetComponent<SpriteRenderer>().color = spriteColor;
    }

    public void SetShaderByState(RoomState roomState)
    {
        //Clear all keywords
        roomIconObject.GetComponent<SpriteRenderer>().material.DisableKeyword("_ROOMSTATUS_UNKNOWN");
        roomIconObject.GetComponent<SpriteRenderer>().material.DisableKeyword("_ROOMSTATUS_ACCESSIBLE");
        roomIconObject.GetComponent<SpriteRenderer>().material.DisableKeyword("_ROOMSTATUS_VISITED");

        //string keyWord = "NOT SET";
        switch (roomState)
        {
            case RoomState.UNKNOWN:
                //keyWord = "_ROOMSTATUS_UNKNOWN";
                roomIconObject.GetComponent<SpriteRenderer>().material.EnableKeyword("_ROOMSTATUS_UNKNOWN");
                break;

            case RoomState.ACCESSIBLE:
                //keyWord = "_ROOMSTATUS_ACCESSIBLE";
                roomIconObject.GetComponent<SpriteRenderer>().material.EnableKeyword("_ROOMSTATUS_ACCESSIBLE");
                break;

            case RoomState.VISITED:
                //keyWord = "_ROOMSTATUS_VISITED";
                roomIconObject.GetComponent<SpriteRenderer>().material.EnableKeyword("_ROOMSTATUS_VISITED");
                break;

            default:
                //keyWord = "_ROOMSTATUS_UNKNOWN";
                roomIconObject.GetComponent<SpriteRenderer>().material.EnableKeyword("_ROOMSTATUS_UNKNOWN");
                break;
        }
        //Debug.Log($"Set Shader Status to: {keyWord}");
        //roomIconObject.GetComponent<SpriteRenderer>().material.EnableKeyword(keyWord);
        //roomIconObject.GetComponent<SpriteRenderer>().material.DisableKeyword("_ROOMSTATUS_UNKNOWN");
        //Debug.Log($"Enabled keyword0 find = {roomIconObject.GetComponent<SpriteRenderer>().material.enabledKeywords[1]}");
    }
    
    private void RefreshLabel()
    {
        _roomText.text = GetLabelByRoomType(roomType);
    }
    public void ChangeColor(Color color)
    {
        roomIconObject.GetComponent<SpriteRenderer>().color = color;
    }
    private void OnMouseEnter() {
        //if(isNavigable || roomState == RoomState.ACCESSIBLE)
        if (!GameManager.Instance.IsPaused && (roomState == RoomState.ACCESSIBLE))
        {
            var scale = new Vector3(oldScale.x * 2,oldScale.y * 2,oldScale.z);
            roomIconObject.transform.localScale = scale;
        }
    }

    private void OnMouseExit() {
        //if(isNavigable || roomState == RoomState.ACCESSIBLE)
        if (!GameManager.Instance.IsPaused && (roomState == RoomState.ACCESSIBLE))
        {
            var scale = oldScale;
            
            roomIconObject.transform.localScale = scale;
        }
    }

    private void OnMouseDown() {
        //if(isNavigable || roomState == RoomState.ACCESSIBLE)
        if (!GameManager.Instance.IsPaused && (roomState == RoomState.ACCESSIBLE)){
            if (roomType == TypeRoom.BOSS || roomType == TypeRoom.ELITE || roomType == TypeRoom.ENCOUNTER)
            {
                AudioManager.instance.SFX.PlaySFXClip(SFXType.MapBattleSFX);
            }
            else if (roomType == TypeRoom.AUTEL)
            {
                AudioManager.instance.SFX.PlaySFXClip(SFXType.MapAutelSFX);
            }
            else
            {
                AudioManager.instance.SFX.PlaySFXClip(SFXType.MapSFX);
            }
            GameManager.Instance.SetRoom(this);
            roomState = RoomState.VISITED;
            SetShaderByState(roomState);
            GameManager.Instance.pmm.UpdateAllPathShaders();

            var scale = oldScale;
            roomIconObject.transform.localScale = scale;
            
        }
    }
    private void FadeIn()
    {
        StartCoroutine(Fade(false));
    }
    private void FadeOut()
    {
        StartCoroutine(Fade(true));
    }
    private IEnumerator Fade(bool isFadeOut)
    {
        if (!isFadeOut) yield return new WaitForSeconds(1.25f);
        Color c = _spriteRenderer.color;
        Color textColor = _roomText.color;
        float timer = 0f;
        float startingAlpha = isFadeOut ? 1f : 0f;
        float targetAlpha = isFadeOut ? 0f : 1f;
        while (timer < _fadeDuration)
        {
            float alpha = Mathf.Lerp(startingAlpha, targetAlpha, timer);
            c.a = alpha;
            textColor.a = alpha;

            //_spriteRenderer.color = c;
            _spriteRenderer.material.SetColor("_Color", c);
            _roomText.color = textColor;
            timer += Time.deltaTime;
            yield return null;
        }
        c.a = targetAlpha;
        textColor.a = targetAlpha;
        //_spriteRenderer.color = c;
        _spriteRenderer.material.SetColor("_Color", c);
        _roomText.color = textColor;

        gameObject.SetActive(!isFadeOut);
    }
  

}
