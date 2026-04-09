using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [field: SerializeField] public List<FloorData> AllFloors { get; private set; }
    [field:SerializeField] public Generator MapGenerator { get; private set; }
    public FloorData CurrentFloor => AllFloors[_currentFloorIndex];
    private int _currentFloorIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void NewFloor()
    {
        _currentFloorIndex++;
        MapGenerator.GenerateNewMap();
    }
}
