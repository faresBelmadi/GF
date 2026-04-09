using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [field: SerializeField] public List<FloorData> AllFloors { get; private set; }
    [field:SerializeField] public Generator MapGenerator { get; private set; }
    private int _currentFloorIndex = -1;

    public FloorData CurrentFloor => AllFloors[_currentFloorIndex];
    public bool IsLastFloor => _currentFloorIndex >= AllFloors.Count - 1;

    // Start is called before the first frame update
    void Start()
    {
        NewFloor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewFloor()
    {
        _currentFloorIndex++;
        Debug.Log("Generating new floor (" + _currentFloorIndex + ")");
        MapGenerator.InitGenerator();
        MapGenerator.GenerateNewMap(CurrentFloor.RoomPool);
    }
}
