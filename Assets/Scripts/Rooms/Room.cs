using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
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
    EXIT
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
    private GameObject _roomObject;
    [SerializeField]
    private TMP_Text _roomText;
    
    private Vector3 oldScale;
    private void Start() 
    {
        oldScale = _roomObject.transform.localScale;
    }

    public void SetRoom(int roomId, TypeRoom type, RoomState state = RoomState.UNKNOWN)
    {
        this.ID = roomId;
        this.roomType = type;
        this.roomState = state;
        gameObject.transform.localScale = new Vector3(7, 7);
        _roomObject.GetComponent<SpriteRenderer>().sprite = GetSpriteByRoomType(type);
        _roomText.text = GetLabelByRoomType(type);
        SetColorByState(roomState);
        //this.GetComponent<Image>().sprite = ToSet;
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

        _roomObject.GetComponent<SpriteRenderer>().color = spriteColor;
    }
    public void SetLabel(string text)
    {
        //TradManager.instance.GetTranslation(_roomData.SalleCombatBossLabel)
        _roomText.text = text;
    }
    public void ChangeColor(Color color)
    {
        _roomObject.GetComponent<SpriteRenderer>().color = color;
    }
    private void OnMouseEnter() {
        //if(isNavigable || roomState == RoomState.ACCESSIBLE)
        if (roomState == RoomState.ACCESSIBLE)
        {
            var scale = new Vector3(oldScale.x * 2,oldScale.y * 2,oldScale.z);
            _roomObject.transform.localScale = scale;
        }
    }

    private void OnMouseExit() {
        //if(isNavigable || roomState == RoomState.ACCESSIBLE)
        if (roomState == RoomState.ACCESSIBLE)
        {
            var scale = oldScale;
            
            _roomObject.transform.localScale = scale;
        }
    }

    private void OnMouseDown() {
        //if(isNavigable || roomState == RoomState.ACCESSIBLE)
        if (roomState == RoomState.ACCESSIBLE){
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
            var scale = oldScale;
            _roomObject.transform.localScale = scale;
            
        }
    }

}
