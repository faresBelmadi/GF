using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum TypeRoom
{
    NotSet,
    CombatNormal,
    CombatElite,
    CombatBoss,
    LevelUp,
    Autel,
    Event,
    Heal,
    Spawn,
    Visited,
    End
}

[System.Serializable]
public class Room : MonoBehaviour
{
    [SerializeField]
    private TypeRoom type;
    public TypeRoom Type 
    {
        get
        {
            return type;
        }
        set
        {
            type = value;
            SetColor();

        }
    }
    public Sprite ToSet;
    public List<GameObject> OwnedCorridors = new List<GameObject>();

    public int ID;
    public List<Room> ConnectedRooms = new List<Room>();
    public bool isStart;
    public bool isNavigable;
    [SerializeField]
    private GameObject _roomObject;
    [SerializeField]
    private TMP_Text _roomText;
    
    private Vector3 oldScale;
    private void Start() 
    {
        oldScale = _roomObject.transform.localScale;
    }

    private void SetColor()
    {
        _roomObject.GetComponent<SpriteRenderer>().sprite = ToSet;
        //this.GetComponent<Image>().sprite = ToSet;
    }
    public void SetLabel(string text)
    {
        _roomText.text = text;
    }
    public void ChangeColor(Color color)
    {
        _roomObject.GetComponent<SpriteRenderer>().color = color;
    }
    private void OnMouseEnter() {
        if (!GameManager.Instance.IsPaused && (isNavigable || type == TypeRoom.Visited))
        {
            var scale = new Vector3(oldScale.x * 2,oldScale.y * 2,oldScale.z);
            _roomObject.transform.localScale = scale;
            
        }
    }

    private void OnMouseExit() {
        if (!GameManager.Instance.IsPaused && (isNavigable || type == TypeRoom.Visited))
        {
            var scale = oldScale;
            
            _roomObject.transform.localScale = scale;
        }
    }

    private void OnMouseDown() {
        if(!GameManager.Instance.IsPaused && ( isNavigable || type == TypeRoom.Visited))
        {
            if (type == TypeRoom.CombatBoss || type == TypeRoom.CombatElite || type == TypeRoom.CombatNormal)
            {
                AudioManager.instance.SFX.PlaySFXClip(SFXType.MapBattleSFX);
            }
            else if (type == TypeRoom.Autel)
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
