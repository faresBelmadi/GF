using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public List<Room> Rooms;

    [SerializeField]
    private RoomData _roomData;
    
    //public Sprite SalleCombatBoss;
    //public Sprite SalleCombatElite;
    //public Sprite SalleCombatNormal;
    //public Sprite SalleLevelUp;
    //public Sprite SalleHeal;
    //public Sprite SalleAlea;
    //public Sprite SalleStart;
    //public Sprite SalleEnd;
    


    public void Init(List<Room> _rooms)
    {
        Rooms = _rooms;

        InitRoom();
    }

    private void InitRoom()
    {
        Room start = Rooms.Find(c => c.isStart == true);

        start.ToSet = _roomData.SalleStart;
        start.gameObject.transform.localScale = new Vector3(10, 10);
        start.Type = TypeRoom.Spawn;
        start.SetLabelID(_roomData.SalleStartLabel);

        //Rooms[1].ToSet = SalleLevelUp;
        //Rooms[1].gameObject.transform.localScale = new Vector3(10, 10);
        //Rooms[1].Type = TypeRoom.Autel;

        //Rooms[1].ToSet = SalleAlea;
        //Rooms[1].gameObject.transform.localScale = new Vector3(10, 10);
        //Rooms[1].Type = TypeRoom.Event;

        //Rooms[1].ToSet = _roomData.SalleCombatBoss;
        //Rooms[1].gameObject.transform.localScale = new Vector3(10, 10);
        //Rooms[1].Type = TypeRoom.CombatBoss;
        //Rooms[1].SetLabel(TradManager.instance.GetTranslation(_roomData.SalleCombatBossLabel));

        Rooms[1].ToSet = _roomData.SalleCombatNormal;
        Rooms[1].gameObject.transform.localScale = new Vector3(10, 10);
        Rooms[1].Type = TypeRoom.CombatNormal;
        Rooms[1].SetLabelID(_roomData.SalleCombatNormalLabel);

        //Rooms[2].ToSet = SalleCombatNormal;
        //Rooms[2].gameObject.transform.localScale = new Vector3(10, 10);
        //Rooms[2].Type = TypeRoom.CombatNormal;

        Rooms[2].ToSet = _roomData.SalleLevelUp;
        Rooms[2].gameObject.transform.localScale = new Vector3(10, 10);
        Rooms[2].Type = TypeRoom.Autel;
        Rooms[2].SetLabelID(_roomData.SalleLevelUpLabel);

        Rooms[3].ToSet = _roomData.SalleCombatElite;
        Rooms[3].gameObject.transform.localScale = new Vector3(10, 10);
        Rooms[3].Type = TypeRoom.CombatElite;
        Rooms[3].SetLabelID(_roomData.SalleCombatEliteLabel);

        Rooms[4].ToSet = _roomData.SalleLevelUp;
        Rooms[4].gameObject.transform.localScale = new Vector3(10, 10);
        Rooms[4].Type = TypeRoom.Autel;
        Rooms[4].SetLabelID(_roomData.SalleLevelUpLabel);

        Rooms[5].ToSet = _roomData.SalleCombatNormal;
        Rooms[5].gameObject.transform.localScale = new Vector3(10, 10);
        Rooms[5].Type = TypeRoom.CombatNormal;
        Rooms[5].SetLabelID(_roomData.SalleCombatNormalLabel);

        Rooms[6].ToSet = _roomData.SalleLevelUp;
        Rooms[6].gameObject.transform.localScale = new Vector3(10, 10);
        Rooms[6].Type = TypeRoom.Autel;
        Rooms[6].SetLabelID(_roomData.SalleLevelUpLabel);

        Rooms[7].ToSet = _roomData.SalleCombatElite;
        Rooms[7].gameObject.transform.localScale = new Vector3(10, 10);
        Rooms[7].Type = TypeRoom.CombatElite;
        Rooms[7].SetLabelID(_roomData.SalleCombatEliteLabel);

        Rooms[8].ToSet = _roomData.SalleLevelUp;
        Rooms[8].gameObject.transform.localScale = new Vector3(10, 10);
        Rooms[8].Type = TypeRoom.Autel;
        Rooms[8].SetLabelID(_roomData.SalleLevelUpLabel);

        Rooms[9].ToSet = _roomData.SalleCombatBoss;
        Rooms[9].gameObject.transform.localScale = new Vector3(10, 10);
        Rooms[9].Type = TypeRoom.CombatBoss;
        Rooms[9].SetLabelID(_roomData.SalleCombatBossLabel);

        Rooms[10].ToSet = _roomData.SalleEnd;
        Rooms[10].gameObject.transform.localScale = new Vector3(10, 10);
        Rooms[10].Type = TypeRoom.End;
        Rooms[10].SetLabelID(_roomData.SalleEndLabel);

        //Rooms[10].ToSet = SalleEnd;
        //Rooms[10].gameObject.transform.localScale = new Vector3(10, 10);
        //Rooms[10].Type = TypeRoom.End;
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
