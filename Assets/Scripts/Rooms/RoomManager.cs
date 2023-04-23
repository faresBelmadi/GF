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

        start.Type = TypeRoom.Spawn;
        Rooms[1].ToSet = SalleCombatNormal;
        Rooms[1].gameObject.transform.localScale = new Vector3(5,5);
        Rooms[1].Type = TypeRoom.CombatNormal;

        Rooms[2].ToSet = SalleCombatElite;
        Rooms[2].gameObject.transform.localScale = new Vector3(5, 5);
        Rooms[2].Type = TypeRoom.CombatElite;

        Rooms[3].ToSet = SalleCombatBoss;
        Rooms[3].gameObject.transform.localScale = new Vector3(5, 5);
        Rooms[3].Type = TypeRoom.CombatBoss;
        
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
