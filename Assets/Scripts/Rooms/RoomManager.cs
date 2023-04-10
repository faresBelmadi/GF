using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public List<Room> Rooms;
    
    public Sprite SalleCombat;
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
        var maxCo = FindMaxConnection();

        foreach (var item in start.ConnectedRooms)
        {
            item.ToSet = SalleCombat;
            item.Type = TypeRoom.Combat;
            foreach (var item2 in item.ConnectedRooms)
            {
                if(item2.Type == TypeRoom.NotSet)
                {
                    item2.ToSet = SalleCombat;
                    item2.Type = TypeRoom.Combat;
                }
            }
        }
        bool healSet = false;
        foreach (var item in Rooms)
        {
            if(item.ConnectedRooms.Count >= maxCo && item != start && !healSet && item.ConnectedRooms.Where(c => c.Type == TypeRoom.Heal).Count() == 0)
            {
                item.ToSet = SalleHeal;
                item.Type = TypeRoom.Heal;
                healSet = true;
            }
            else if(item.ConnectedRooms.Count >= maxCo && item != start && item.ConnectedRooms.Where(c => c.Type == TypeRoom.Event).Count() == 0 && item.Type != TypeRoom.Heal)
            {
                item.ToSet = SalleAlea;
                item.Type = TypeRoom.Event;
            }
            else if(item.Type == TypeRoom.NotSet)
            {
                healSet = false;
                bool Combat = true;
                for (int i = 0; i < item.ConnectedRooms.Count; i++)
                {
                    if(item.ConnectedRooms[i].Type != TypeRoom.Combat)
                    {
                        Combat = true;
                    }
                    else
                    {
                        Combat = false;
                        i = item.ConnectedRooms.Count;
                    }
                }
                if(Combat)
                {
                    item.ToSet = SalleCombat;
                    item.Type = TypeRoom.Combat;

                }
                else
                {
                    item.ToSet = SalleCombat;
                    item.Type = TypeRoom.Combat;
                }
            }
        }
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
