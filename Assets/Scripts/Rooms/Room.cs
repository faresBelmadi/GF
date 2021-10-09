using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public enum TypeRoom
{
    NotSet,
    Combat,
    Event,
    Heal,
    Spawn,
    Visited

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
        switch (type)
        {
            case TypeRoom.Spawn:
            this.GetComponent<SpriteRenderer>().color = Color.blue;
                break;
            case TypeRoom.NotSet:
            this.GetComponent<SpriteRenderer>().color = Color.grey;
                break;
            case TypeRoom.Combat:
            this.GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case TypeRoom.Heal:
            this.GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case TypeRoom.Event:
            this.GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            case TypeRoom.Visited:
            this.GetComponent<SpriteRenderer>().color = Color.black;
                break;
            default:
            break;
        }
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
            GameManager.instance.SetRoom(this);
            var scale = oldScale;
            transform.localScale = scale;
        }
    }

}
