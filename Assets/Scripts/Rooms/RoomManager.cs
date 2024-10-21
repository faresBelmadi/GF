using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public List<Room> Rooms;
    
    public Sprite SalleCombatBoss;
    public Sprite SalleCombatElite;
    public Sprite SalleCombatNormal;
    public Sprite SalleLevelUp;
    public Sprite SalleHeal;
    public Sprite SalleAlea;
    public Sprite SalleStart;
    public Sprite SalleEnd;



    public void Init(List<Room> _rooms)
    {
        Rooms = _rooms;

        InitRoom();
    }

    private void InitRoom()
    {
        Room start = Rooms.Find(c => c.isStart == true);

        start.ToSet = SalleStart;
        start.gameObject.transform.localScale = new Vector3(10, 10);
        start.Type = TypeRoom.Spawn;

        //Rooms[1].ToSet = SalleAlea;
        //Rooms[1].gameObject.transform.localScale = new Vector3(10, 10);
        //Rooms[1].Type = TypeRoom.Event;

        Rooms[1].ToSet = SalleCombatNormal;
        Rooms[1].gameObject.transform.localScale = new Vector3(10, 10);
        Rooms[1].Type = TypeRoom.CombatNormal;

        Rooms[2].ToSet = SalleCombatNormal;
        Rooms[2].gameObject.transform.localScale = new Vector3(10, 10);
        Rooms[2].Type = TypeRoom.CombatNormal;

        //Rooms[2].ToSet = SalleLevelUp;
        //Rooms[2].gameObject.transform.localScale = new Vector3(10, 10);
        //Rooms[2].Type = TypeRoom.Autel;

        Rooms[3].ToSet = SalleCombatElite;
        Rooms[3].gameObject.transform.localScale = new Vector3(10, 10);
        Rooms[3].Type = TypeRoom.CombatElite;

        Rooms[4].ToSet = SalleLevelUp;
        Rooms[4].gameObject.transform.localScale = new Vector3(10, 10);
        Rooms[4].Type = TypeRoom.Autel;

        Rooms[5].ToSet = SalleCombatNormal;
        Rooms[5].gameObject.transform.localScale = new Vector3(10, 10);
        Rooms[5].Type = TypeRoom.CombatNormal;

        Rooms[6].ToSet = SalleLevelUp;
        Rooms[6].gameObject.transform.localScale = new Vector3(10, 10);
        Rooms[6].Type = TypeRoom.Autel;

        Rooms[7].ToSet = SalleCombatElite;
        Rooms[7].gameObject.transform.localScale = new Vector3(10, 10);
        Rooms[7].Type = TypeRoom.CombatElite;

        Rooms[8].ToSet = SalleLevelUp;
        Rooms[8].gameObject.transform.localScale = new Vector3(10, 10);
        Rooms[8].Type = TypeRoom.Autel;

        //Rooms[9].ToSet = SalleCombatBoss;
        //Rooms[9].gameObject.transform.localScale = new Vector3(10, 10);
        //Rooms[9].Type = TypeRoom.CombatBoss;

        Rooms[9].ToSet = SalleEnd;
        Rooms[9].gameObject.transform.localScale = new Vector3(10, 10);
        Rooms[9].Type = TypeRoom.End; 
        GameManager.Instance.SetRoom(start);
    }
    
    public int FindMaxConnection()
    {
        int max = 0;

        foreach (var item in Rooms)
        {
            if(item.ConnectedRooms.Count > max && item.isStart == false)
                max = item.ConnectedRooms.Count;
        }
        return max;
    }


}
