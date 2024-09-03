using System.Collections.Generic;
using UnityEngine;


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
    
    Vector3 oldScale;
    private void Start() 
    {
        oldScale = transform.localScale;
    }

    private void SetColor()
    {
        this.GetComponent<SpriteRenderer>().sprite = ToSet;
    }

    private void OnMouseEnter() {
        if(isNavigable || type == TypeRoom.Visited)
        {
            var scale = new Vector3(oldScale.x * 2,oldScale.y * 2,oldScale.z);
            transform.localScale = scale;
        }
    }

    private void OnMouseExit() {
        if(isNavigable || type == TypeRoom.Visited)
        {
            var scale = oldScale;
            transform.localScale = scale;
        }
    }

    private void OnMouseDown() {
        if(isNavigable || type == TypeRoom.Visited)
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
            GameManager.instance.SetRoom(this);
            var scale = oldScale;
            transform.localScale = scale;
        }
    }

}
