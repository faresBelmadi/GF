using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        Rooms[1].ToSet = SalleLevelUp;
        Rooms[1].gameObject.transform.localScale = new Vector3(10, 10);
        Rooms[1].Type = TypeRoom.Autel;

        //Rooms[1].ToSet = SalleCombatNormal;
        //Rooms[1].gameObject.transform.localScale = new Vector3(10, 10);
        //Rooms[1].Type = TypeRoom.CombatNormal;

        //Rooms[2].ToSet = SalleCombatNormal;
        //Rooms[2].gameObject.transform.localScale = new Vector3(10, 10);
        //Rooms[2].Type = TypeRoom.CombatNormal;

        Rooms[2].ToSet = SalleLevelUp;
        Rooms[2].gameObject.transform.localScale = new Vector3(10, 10);
        Rooms[2].Type = TypeRoom.Autel;

        Rooms[3].ToSet = SalleCombatElite;
        Rooms[3].gameObject.transform.localScale = new Vector3(10, 10);
        Rooms[3].Type = TypeRoom.CombatElite;

        Rooms[4].ToSet = SalleCombatBoss;
        Rooms[4].gameObject.transform.localScale = new Vector3(10, 10);
        Rooms[4].Type = TypeRoom.CombatBoss;
        
        GameManager.instance.SetRoom(start);
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
