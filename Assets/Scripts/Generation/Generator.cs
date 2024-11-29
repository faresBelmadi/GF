//using JetBrains.Annotations;
//using System;
//using System.CodeDom.Compiler;
using JetBrains.Annotations;
using Synapse.Runtime.Debug;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;



//using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Generator : MonoBehaviour
{
    //[SerializeField] private RoomData roomData;
    
    [Header("Spawned Object")]
    public Dictionary<Vector2,GameObject> spawnedRoomsObj;

    [Header("Spawnable")]
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private GameObject pathPrefab;


    [Header("Rooms Display Settings")]
    [SerializeField] private Vector2 mapAreaSize;
    [SerializeField] private Vector2 mapAreaOffset;
    //[SerializeField] private Material baseRoomMaterial;
    [SerializeField] private Material basePathMaterial;
    [SerializeField] private float roomDefaultSpriteSize = 8f;

    [Header("Map Settings"),Tooltip("Don't Add Start, Boss or Loot rooms, they are Added automaticaly.")]
    [SerializeField] private List<TypeRoom> roomPool = new List<TypeRoom>();
    [SerializeField] private int defaultRowSize;
    [SerializeField] private int defaultColSize;
    [SerializeField, Tooltip("0 for random")] private int seed;
    [Header("DEBUG")]
    [SerializeField] private bool doCycle = false;
    [SerializeField] private float cycleTime = 1f;

    [Header("Forbiden connections")]
    [SerializeField] private List<TypeRoom> illegalConnection_START= new List<TypeRoom>();
    [SerializeField] private List<TypeRoom> illegalConnection_BOSS = new List<TypeRoom>();
    [SerializeField] private List<TypeRoom> illegalConnection_AUTEL = new List<TypeRoom>();
    [SerializeField] private List<TypeRoom> illegalConnection_DEFAULTS = new List<TypeRoom>();
    [SerializeField] private List<TypeRoom> illegalConnection_ELITES = new List<TypeRoom>();

    private List<GameObject> Lines;
    //private GameObject[,] pathsGameObjects;
    private List<Container> ResultBsp;

    private List<TypeRoom> aviableRoomPool = new List<TypeRoom>();
    private List<PlayerMapManager.MapNode> mapNodes;

    private float TEMPtimer;
    
    void Start()
    {
        spawnedRoomsObj = new Dictionary<Vector2, GameObject>();
        Lines = new List<GameObject>();
        //aviableRoomPool = new List<TypeRoom>(roomPool);

        mapNodes = GameManager.Instance.pmm.map;

        GenerateNewMap();

    }

    private void Update()
    {
        if(doCycle)
        {
            if(TEMPtimer < cycleTime)
            {
                TEMPtimer += Time.deltaTime;
            }
            else
            {
                TEMPtimer = 0f;
                //aviableRoomPool = new List<TypeRoom>(roomPool);
                GenerateNewMap();
            }
        }
    }

    public void GenerateNewMap()
    {
        int usedSeed = seed == 0 ? Random.Range(int.MinValue, int.MaxValue) : seed;

        int elitCnt = aviableRoomPool.Where(rType => rType == TypeRoom.ELITE || rType == TypeRoom.CLASS_ELITE).Count();
        int defaultRoomCnt = 2/*Start & Boss*/ + aviableRoomPool.Count();//Loots rooms not counted

        int colSize = Mathf.Max( defaultColSize,Mathf.FloorToInt((defaultRoomCnt - 6) / defaultRowSize) +1);
        int gridSize = defaultRowSize * colSize;

        int roomCnt = gridSize + 6;
        //int roomCnt = defaultRoomCnt;

        int maxTry = 5;
        int tryCnt = 0;
        while (tryCnt < maxTry)
        {
            aviableRoomPool = new List<TypeRoom>(roomPool);
            Random.InitState(usedSeed);
            Debug.Log($"Seed to use: {usedSeed}");
            
            ClearGen();

            Debug.Log("Generate map");
            GenerateMap(roomCnt);// OR LOAD MAPNODES FROM SAVE FILES
            Debug.Log($"Used Seed: {usedSeed}");

            if (IsMapValid(roomCnt)) break;

            usedSeed ++;
        }
        
        GameManager.Instance.pmm.mapUsedSeed = usedSeed;


        Debug.Log("spawn first 3 rooms");
        SpawnRoom(0, roomCnt, RoomState.VISITED);
        SpawnRoom(1, roomCnt, RoomState.ACCESSIBLE);
        SpawnRoom(2, roomCnt, RoomState.ACCESSIBLE);

        Debug.Log("spawn all other non Empty rooms");
        for (int i = 3; i < mapNodes.Count(); i++)
        {
            if (mapNodes[i].roomType != TypeRoom.NONE) SpawnRoom(i, roomCnt);
        }
        Debug.Log("spawn all paths");
        GameManager.Instance.pmm.pathsGameObjects = new GameObject[roomCnt,roomCnt];
        SpawnAllPaths(roomCnt);
        // <= LOAD already visited rooms

    }
    private void GenerateMap(int roomCnt)
    {
        mapNodes.Clear();


        for (int i = 0; i < roomCnt; i++)
        {
            mapNodes.Add(new PlayerMapManager.MapNode());
        }

        //Setup Start & Boss Rooms
        mapNodes[0].roomType = TypeRoom.START;
        mapNodes[0].connections.Add(1);
        mapNodes[0].connections.Add(2);
        
        mapNodes[roomCnt - 1].roomType = TypeRoom.BOSS;
        mapNodes[roomCnt - 1].connections.Add(roomCnt - 2);
        mapNodes[roomCnt - 1].connections.Add(roomCnt - 3);
        
        //TESTs
        //Setup dirrect exit connected rooms (1,2, -3,-2)
        List<TypeRoom> validFirstRooms = new List<TypeRoom>() 
        {
            TypeRoom.ENCOUNTER, 
            TypeRoom.CLASS_ENCOUNTER, 
            TypeRoom.RANDOM, 
            TypeRoom.CLASS_RANDOM 
        };
    
        for (int i = 0; i < roomCnt; i++)
        {
            //add Every Possible connections
            foreach (int connectionId in GetPossibleConnections(i, roomCnt))
            {
                mapNodes[i].connections.Add(connectionId);
            }
        }
        //Fill Grid with Null rooms
        for (int i = 3; i < roomCnt - 3; i++)
        {
            mapNodes[i].roomType = TypeRoom.NONE;
        }

        List<TypeRoom> firstOptions = aviableRoomPool.Where(rType => validFirstRooms.Contains(rType)).ToList();

        mapNodes[1].roomType = firstOptions[Random.Range(0, firstOptions.Count() - 1)];
        mapNodes[1].connections.Add(0);
        HandleLootRooms(mapNodes[1].roomType,1);
        aviableRoomPool.Remove(mapNodes[1].roomType);

        firstOptions.RemoveAll(rType => rType == mapNodes[1].roomType);

        mapNodes[2].roomType = firstOptions[Random.Range(0, firstOptions.Count() - 1)];
        mapNodes[2].connections.Add(0);
        HandleLootRooms(mapNodes[2].roomType, 2);

        aviableRoomPool.Remove(mapNodes[2].roomType);

        mapNodes[roomCnt - 2].roomType = aviableRoomPool[Random.Range(0, aviableRoomPool.Count() - 1)];
        mapNodes[roomCnt - 2].connections.Add(roomCnt - 1);
        HandleLootRooms(mapNodes[roomCnt - 2].roomType, roomCnt - 2);
        aviableRoomPool.Remove(mapNodes[roomCnt - 2].roomType);

        mapNodes[roomCnt - 3].roomType = aviableRoomPool[Random.Range(0, aviableRoomPool.Count() - 1)];
        mapNodes[roomCnt - 3].connections.Add(roomCnt - 1);
        HandleLootRooms(mapNodes[roomCnt - 3].roomType, roomCnt - 3);

        aviableRoomPool.Remove(mapNodes[roomCnt - 3].roomType);

        //Choose Types And Connections
        
        //FILL UP AVIABLES POOL WITH NONE
        if (aviableRoomPool.Count() < roomCnt - 6)
        {
            for (int i = aviableRoomPool.Count(); i < (roomCnt - 6) ; i++)
            {
                aviableRoomPool.Add(TypeRoom.NONE);
            }
        }
        
        int maxLoop = 3;
        for (int loopId = 0; loopId < maxLoop; loopId++)
        {

            int startIndex = Random.Range(0, roomCnt - 6);
            for (int i = 0; i < roomCnt - 6; i++)
            {
                int roomIndex = ((i + startIndex) % (roomCnt - 6)) + 3;

                if (mapNodes[roomIndex].roomType != TypeRoom.NONE)
                {
                    //Debug.Log($"Room {roomIndex} AlreadySet");
                    continue;
                }

                //Debug.Log($"step {i}, choosing for id:{roomIndex}");
                List<TypeRoom> nonEmptyconnectedRooms = new List<TypeRoom>();
                foreach (int connectionId in mapNodes[roomIndex].connections)
                {
                    if (mapNodes[connectionId].roomType != TypeRoom.NONE)
                    {
                        nonEmptyconnectedRooms.Add(mapNodes[connectionId].roomType);
                    }
                }
            
                List<TypeRoom> validRooms = GetPossibleRoomTypes(nonEmptyconnectedRooms, aviableRoomPool);
            
                if (validRooms.Count() <= 0)
                {
                    Debug.Log($"/!\\\nno aviable room choice for room at pos {roomIndex}");
                    Debug.Log("choice Remaining:");
                    foreach (TypeRoom room in aviableRoomPool)
                    {
                        Debug.Log(room);
                    }
                    Debug.Log("Connections:");
                    foreach (TypeRoom room in nonEmptyconnectedRooms)
                    {
                        Debug.Log(room);
                    }
                    continue;
                }

                int choosedRoomId = Random.Range(0, validRooms.Count());
                HandleLootRooms(validRooms[choosedRoomId],roomIndex);
                //Debug.Log($"room choosed:{validRooms[choosedRoomId].ToString()}");
                mapNodes[roomIndex].roomType = validRooms[choosedRoomId];
                aviableRoomPool.Remove(validRooms[choosedRoomId]);

            }
            if (aviableRoomPool.Count() > 0)
            {
                //Debug.Log("room Left to Pick:");
                //foreach (TypeRoom type in aviableRoomPool)
                //{
                //    Debug.Log(type.ToString());
                //}
            }
            else
            {
                Debug.Log($"Map Node Choice finished at iterration: {loopId}");
                break;
            }
        }


        //Interconnect between Unchoosed Nodes
        for (int i = 0; i < roomCnt; i++)
        {
            List<int> interco = new List<int>();
            if (mapNodes[i].roomType == TypeRoom.NONE)
            {
                //Debug.Log($"Interconnection for {i}:");
                interco = GetAllInterconnections(i, new List<int>());

            }
            foreach (int con in interco)
            {
                Debug.Log(con);
            }

            foreach (int source in interco)
            {
                foreach (int dest in interco)
                {
                    if (source == dest) continue;
                    //Debug.Log($"Try connection: {source}:{dest}");
                    if (GetPossibleRoomTypes(
                            new List<TypeRoom>() { mapNodes[source].roomType },
                            new List<TypeRoom>() { mapNodes[dest].roomType }
                            ).Count() > 0
                            && !((source==1 || source == 2) && (dest == 1 || dest == 2))
                            && !((source==roomCnt-2|| source == roomCnt-3) && (dest == roomCnt - 2 || dest == roomCnt - 3))
                            )
                    {
                        if (!mapNodes[source].connections.Contains(dest))
                        {
                            mapNodes[source].connections.Add(dest);
                        }
                        if (!mapNodes[dest].connections.Contains(source))
                        {
                            mapNodes[dest].connections.Add(source);
                        }
                    }
                    else
                    {
                        //Debug.Log($"Trying invalid connection: {source}:{dest}");
                    }
                }
            }
        }

        //Clear all Unchoused Connections
        //Debug.Log("Clear all Unchoused Connections");
        for (int i = 0; i < roomCnt; i++)
        {
            List<int> interco = new List<int>();
            if (mapNodes[i].roomType == TypeRoom.NONE)
            {
                //foreach (int connectedRoomId in mapNodes[i].connections)
                //{
                //    mapNodes[connectedRoomId].connections.Remove(i);
                //}

                mapNodes[i].connections.Clear();
            }
        }

        //remove unused connections
        //Debug.Log("remove unused connections");
        for (int i = 0; i < roomCnt; i++)
        {
            List<int> connectionToRemove = new List<int>();
            foreach (int connection in mapNodes[i].connections)
            {
                if (mapNodes[connection].roomType == TypeRoom.NONE) connectionToRemove.Add(connection);
                if (mapNodes[connection].roomType == TypeRoom.LOOT)
                {
                    if (mapNodes[connection].connections[0] != i) connectionToRemove.Add(connection);
                }
            }
            foreach (int connection in connectionToRemove)
            {
                mapNodes[i].connections.Remove(connection);
            }
        }
        //Debug.Log("Removing duplicates in connections");
        for (int i = 0;i < roomCnt; i++)
        {
            mapNodes[i].connections = mapNodes[i].connections.Distinct().ToList();
        }
        LengthBasedDecimatePath(roomCnt);


        Debug.Log($"End Generate Map with seed: {seed}");
    }

    private bool IsMapValid(int nodeCnt)
    {
        int elitCnt = roomPool.Where(rType => rType == TypeRoom.ELITE || rType == TypeRoom.CLASS_ELITE).Count();
        int finalRoomCnt = 2/*Start & Boss*/ + roomPool.Count() + elitCnt;//Loots 

        List<int> foundNodes = new List<int>() { 0 };
        List<int> currentlyConnected = new List<int>() { 0 };

        while (currentlyConnected.Count > 0)
        {
            //List<int> nextConnections = new List<int>();
            if (currentlyConnected[0] != nodeCnt-1) //Don't get boss connections
            {
                foreach (int connectedId in mapNodes[currentlyConnected[0]].connections)
                {
                    if (!foundNodes.Contains(connectedId))
                    {
                        foundNodes.Add(connectedId);
                        //nextConnections.Add(connectedId);
                        currentlyConnected.Add(connectedId);
                    }
                }
            }
            
            currentlyConnected.RemoveAt(0);
        }
        Debug.Log($"{foundNodes.Count} room find over {finalRoomCnt} expected");

        return foundNodes.Count == finalRoomCnt;
    }

    private void LengthBasedDecimatePath(int roomCnt)
    {
        for (int i = 0; i < roomCnt; i++)
        {
            int maxConnection = Random.Range(3, 4);
            if (mapNodes[i].connections.Count() <= maxConnection) continue;


            int cntToRemove = mapNodes[i].connections.Count() - maxConnection;
            
            Dictionary<int,float> connectionLength = new Dictionary<int,float>();
            Vector2 roomPos = GetPositionByIndex(i,roomCnt);
            //Debug.Log($"Connected ids length of room {i}:");
            foreach (int connectionId in mapNodes[i].connections)
            {
                if(connectionId == 0
                    || connectionId == roomCnt-1
                    ) 
                {
                    cntToRemove--;
                    continue;
                }

                if (mapNodes[connectionId].roomType == TypeRoom.LOOT
                    && mapNodes[connectionId].connections.Contains(i))
                {
                    //cntToRemove--;
                    continue;
                }

                connectionLength.Add(connectionId, (GetPositionByIndex(connectionId, roomCnt) - roomPos).magnitude);
            }
            
            connectionLength = connectionLength.OrderByDescending(kvPair => kvPair.Value).ToDictionary(kvPair => kvPair.Key, kvPair => kvPair.Value);

            //Debug.Log($"removing {cntToRemove} connections of room {i}:");
            for (int j = 0; j < cntToRemove; j++)
            {
                //Debug.Log($"removing connection: {i}-{connectionLength.ElementAt(j).Key}");
                mapNodes[i].connections.Remove(connectionLength.ElementAt(j).Key);
                mapNodes[connectionLength.ElementAt(j).Key].connections.Remove(i);
            }
        }
    }
    private void DetectPathCrossing(int roomCnt)
    {
        for (int i = 0; i< roomCnt; i++)
        {

        }
    }
    private bool doPathCross(Vector2 firstPathStart, Vector2 firstPathEnd, Vector2 secondPathStart, Vector2 secondPathEnd)
    {
        bool doPathsCross = false;




        return doPathsCross;
    }
    private void DumbDeciamtePath(int roomCnt)
    {
        for (int i = 0; i < roomCnt; i++)
        {
            int maxConnection = Random.Range(2, 3);
            int cntToRemove = mapNodes[i].connections.Count() - maxConnection;
            for (int j = 0; j < cntToRemove; j++)
            {
                int startingOffset = Random.Range(0, mapNodes[i].connections.Count());
                for (int k = 0; k < mapNodes[i].connections.Count(); k++)
                {
                    int idToCheck = (k + startingOffset) % mapNodes[i].connections.Count();
                    int connectedRoomId = mapNodes[i].connections[idToCheck];
                    if (mapNodes[connectedRoomId].connections.Count() > 2 && connectedRoomId != 0 && connectedRoomId != roomCnt - 1)
                    {
                        mapNodes[i].connections.RemoveAt(idToCheck);
                        mapNodes[connectedRoomId].connections.Remove(i);
                        break;
                    }
                }
            }
        }
    }

    private void HandleLootRooms(TypeRoom roomType, int elitIndex)
    {
        if (roomType == TypeRoom.ELITE || roomType == TypeRoom.CLASS_ELITE)
        {
            int idOffset = Random.Range(0, mapNodes[elitIndex].connections.Count());
            for (int j = 0; j < mapNodes[elitIndex].connections.Count(); j++)
            {
                int usedId = mapNodes[elitIndex].connections[(j + idOffset) % mapNodes[elitIndex].connections.Count()];
                if (mapNodes[usedId].roomType == TypeRoom.NONE)
                {
                    mapNodes[usedId].roomType = TypeRoom.LOOT;
                    mapNodes[usedId].connections = new List<int>() { elitIndex };
                    break;
                }
                else
                {
                    Debug.Log("No Room for loot");
                }
            }

        }
    }

    private List<int> GetAllInterconnections(int rootId, List<int> idToIgnore)
    {
        idToIgnore.Add(rootId);
        List<int> interconnection = new List<int>();
        for(int i = 0; i < mapNodes[rootId].connections.Count(); i++)
        {
            int idToCheck = mapNodes[rootId].connections[i];
            if (idToIgnore.Contains(idToCheck)) continue;
            if (mapNodes[idToCheck].roomType != TypeRoom.NONE)
            {
                if (mapNodes[idToCheck].roomType != TypeRoom.LOOT)
                {
                    interconnection.Add(idToCheck);
                    idToIgnore.Add(idToCheck);
                }
            }
            else
            {
                interconnection.AddRange(GetAllInterconnections(idToCheck, idToIgnore));
            }
        }
        return interconnection;
    }

    private List<TypeRoom> GetPossibleRoomTypes(List<TypeRoom> connectedRooms, List<TypeRoom> RemainingPool)
    {
        List<TypeRoom> validRooms = new List<TypeRoom>(RemainingPool);
        foreach (TypeRoom connectedRoom in connectedRooms)
        {
            if (connectedRoom == TypeRoom.AUTEL)
                validRooms.RemoveAll(rType => illegalConnection_AUTEL.Contains(rType));
            if (connectedRoom == TypeRoom.ENCOUNTER
                || connectedRoom == TypeRoom.CLASS_ENCOUNTER
                || connectedRoom == TypeRoom.RANDOM
                || connectedRoom == TypeRoom.CLASS_RANDOM
                )
                validRooms.RemoveAll(rType => illegalConnection_DEFAULTS.Contains(rType));
            if (connectedRoom == TypeRoom.ELITE
                || connectedRoom == TypeRoom.CLASS_ELITE
                )
                validRooms.RemoveAll(rType => illegalConnection_ELITES.Contains(rType));

            //if (connectedRoom == TypeRoom.LOOT) validRooms.Clear();
        }
        return validRooms;
    }

    private List<int> GetPossibleConnections(int nodeIndex, int roomCnt)
    {
        List<int> possibleConnections = new List<int>();
        //int roomCnt = 2/*Start & Boss*/ /*+ elitCnt*/ /*Loots*/+ roomPool.Count();
        if (nodeIndex < 3 || nodeIndex > roomCnt - 4)
        {
            if (nodeIndex == 1)
            {
                for (int i = 0; i < Mathf.CeilToInt(defaultRowSize / 2f); i++)
                {
                    possibleConnections.Add(i + 3);
                }
            }
            if (nodeIndex == 2)
            {
                for (int i = defaultRowSize / 2; i < defaultRowSize; i++)
                {
                    possibleConnections.Add(i + 3);
                }
            }
            
            if (nodeIndex == roomCnt - 3)
            {
                for (int i = roomCnt - 3 - defaultRowSize; i < (roomCnt - 3) - Mathf.FloorToInt(defaultRowSize / 2f); i++)
                {
                    possibleConnections.Add(i);
                    mapNodes[i].connections.Add(roomCnt - 3);
                }
            }
            if (nodeIndex == roomCnt - 2)
            {
                for (int i = (roomCnt - 3) - Mathf.CeilToInt(defaultRowSize / 2f); i < roomCnt - 3; i++)
                {
                    possibleConnections.Add(i);
                    mapNodes[i].connections.Add(roomCnt - 2);
                }
            }
            
        }
        else
        {
            //LeftNode
            if (((nodeIndex-3) % defaultRowSize) - 1 >= 0)
            {
                possibleConnections.Add(nodeIndex - 1);
            }

            //RightNode
            if (nodeIndex < roomCnt-4 && ((nodeIndex - 3) % defaultRowSize) + 1 < defaultRowSize)
            {
                possibleConnections.Add(nodeIndex + 1);
            }

            //TopNode
            if (nodeIndex + defaultRowSize < roomCnt - 3)
            {
                possibleConnections.Add(nodeIndex + defaultRowSize);
            }
            else
            {
                if ((nodeIndex - 3) % defaultRowSize < defaultRowSize / 2f)
                    possibleConnections.Add(roomCnt - 3);
                if ((nodeIndex - 3) % defaultRowSize >= defaultRowSize / 2)
                    possibleConnections.Add(roomCnt - 2);
            }

            //BottomNode
            if ((nodeIndex - 3) - defaultRowSize >= 0)
            {
                possibleConnections.Add(nodeIndex - defaultRowSize);
            }
            else
            {
                if ((nodeIndex - 3) % defaultRowSize >= defaultRowSize / 2)
                {
                    mapNodes[2].connections.Add(nodeIndex);
                    possibleConnections.Add(2);
                }
                if ((nodeIndex - 3) % defaultRowSize < defaultRowSize / 2f)
                {
                    mapNodes[1].connections.Add(nodeIndex);
                    possibleConnections.Add(1);
                }
            }
        }

        return possibleConnections;
    }
    private Vector2 GetPositionByIndex(int index, int roomCnt)
    {
        int defaultRoomCnt = 2/*Start & Boss*/ + aviableRoomPool.Count();
        int rowCnt = Mathf.Max(defaultColSize, Mathf.FloorToInt((defaultRoomCnt - 6) / defaultRowSize) + 1);//Mathf.FloorToInt((float)roomPool.Count() / (float)defaultRowSize);//last row will overflow


        float verticalSpacing = mapAreaSize.y/((rowCnt+2)+1);
        float horisontalSpacing = mapAreaSize.x/(defaultRowSize+1);

        Vector2 mapCenter = new Vector2(mapAreaSize.x/2f+mapAreaOffset.x, mapAreaSize.y / 2f + mapAreaOffset.y);

        if(index <3 ||  index > roomCnt - 4)
        {
            if(index == 0)
            {
                return new Vector2(mapCenter.x, mapAreaOffset.y);
            }
            else if(index == 1)
            {
                return new Vector2(mapCenter.x - horisontalSpacing, mapAreaOffset.y + verticalSpacing);
            }
            else if (index == 2)
            {
                return new Vector2(mapCenter.x + horisontalSpacing, mapAreaOffset.y + verticalSpacing);
            }
            else if (index == roomCnt - 1)
            {
                return new Vector2(mapCenter.x, mapAreaSize.y + mapAreaOffset.y);
            }
            else if (index == roomCnt - 2)
            {
                return new Vector2(mapCenter.x + horisontalSpacing, mapAreaSize.y + mapAreaOffset.y - verticalSpacing);
            }
            else if (index == roomCnt - 3)
            {
                return new Vector2(mapCenter.x - horisontalSpacing, mapAreaSize.y + mapAreaOffset.y - verticalSpacing);
            }
            else { return Vector2.zero; }
        }
        else
        {
            float posX = ((index-3) % defaultRowSize) * horisontalSpacing;
            float posY = (Mathf.FloorToInt((index-3) / defaultRowSize)+2)*verticalSpacing;
            float centeringOffset = (mapAreaSize.x / 2f) - (((defaultRowSize - 1) / 2f) * horisontalSpacing);

            return new Vector2(posX + mapAreaOffset.x + centeringOffset, posY + mapAreaOffset.y);
        }

    }

    void SpawnRoom(int mapIndex, int roomCnt, RoomState defaultState = RoomState.UNKNOWN)
    {
        TypeRoom type = mapNodes[mapIndex].roomType;
        Vector2 pos = GetPositionByIndex(mapIndex, roomCnt);

        Vector3 position = new Vector3(pos.x, pos.y, 75);
        GameObject roomObject = Instantiate(roomPrefab, position, Quaternion.identity, transform);
        roomObject.name = type.ToString();
        mapNodes[mapIndex].objectInstance = roomObject;

        //roomObject.GetComponent<SpriteRenderer>().material = new Material(baseRoomMaterial);

        Room room = roomObject.GetComponent<Room>();
        room.SetRoom(mapIndex,type, roomDefaultSpriteSize, defaultState);
        
        int encounterId = GameManager.Instance.SelectEncounterId(type);
        GameManager.Instance.pmm.roomSelectedEncounters.Add(new System.Tuple<int, int>(mapIndex, encounterId));
        room.selectedEncounterId = encounterId;
        //room.SetEncounter(encounterId);
        Debug.Log($"Room {mapIndex}({type.ToString()}): EncounterSelected: {encounterId}");
        //room.SetEncounter(GameManager.Instance.SelectEncounterId(type));
        spawnedRoomsObj.Add(new Vector2(pos.x,pos.y),roomObject);
    }
    void SpawnAllPaths(int roomCnt)
    {
        for (int i = 0; i < mapNodes.Count(); i++)
        {
            foreach (int connection in mapNodes[i].connections)
            {
                if(connection <= i) continue;
                GameObject pathObject = Instantiate(pathPrefab, mapNodes[i].objectInstance.transform);
                Vector3 startPos = GetPositionByIndex(i, roomCnt);
                Vector3 endPos = GetPositionByIndex(connection, roomCnt);
                Vector3[] pathPos = new Vector3[] { startPos, endPos };
                pathObject.GetComponent<LineRenderer>().SetPositions(pathPos);
                pathObject.GetComponent<LineRenderer>().material = new Material(basePathMaterial);

                Lines.Add(pathObject);
                GameManager.Instance.pmm.pathsGameObjects[i,connection] = pathObject;
            }
        }
        GameManager.Instance.pmm.UpdateAllPathShaders();
    }

    void ClearGen()
    {
        foreach (var item in spawnedRoomsObj)
        {
            Destroy(item.Value);
        }
        spawnedRoomsObj.Clear();
        
        foreach (var item in Lines)
        {
            Destroy(item);
        }
        Lines.Clear();

    }
}