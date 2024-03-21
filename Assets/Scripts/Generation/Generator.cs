using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Generator : MonoBehaviour
{
    [Header("BSP")]
    public BSPGen config;
    public List<Container> ResultBsp;

    [Header("Spawned Object")]
    public Dictionary<Vector2,GameObject> Spawned;
    Dictionary<GameObject,List<GameObject>> Corridors;
    public List<GameObject> Lines;

    [Header("Spawnable")]
    public GameObject Spawn;
    public Transform Parent;
    public GameObject Line;


    [Header("Components")]
    [SerializeField]
    private EdgeCollider2D CrossCheck;
    [SerializeField]
    private RoomManager roomManager;

    void Start()
    {
        Spawned = new Dictionary<Vector2, GameObject>();
        Corridors = new Dictionary<GameObject, List<GameObject>>();
        Lines = new List<GameObject>();
        ResultBsp = new List<Container>();
        ButtonClick();
    }
    
    public void ButtonClick()
    {
        ClearGen();
        Generate();
        SpawnRoom();
        CreateCorridor();
        SpawnCorridor();
        InitManager();
        ClearUseless();
    }

    void Generate()
    {
        ResultBsp = config.Generate();
    }

    void SpawnRoom()
    {
        foreach (var item in ResultBsp)
        {
            //int i = 0;
            //bool CanSpawn = true;
            Vector2 pos = item.Center;
            Vector3 position = new Vector3(pos.x, pos.y, 75);
            var t = Instantiate(Spawn, position, Quaternion.identity, Parent);
            t.name = "room";
            Spawned.Add(pos, t);
        }
    }

    void CreateCorridor()
    {
        foreach (var item in Spawned)
        {
            var t = GetNumberCorridor();
            List<Vector2> temp = new List<Vector2>();
            List<Vector2> Rejected = new List<Vector2>();

            for (int i = 0; temp.Count < t && i < Spawned.Count ; i++)
            {
                Vector2 tempObject = Vector2.zero;
                float tempDistance = 100000f;
                foreach (var item2 in Spawned)
                {
                    if(!temp.Contains(item2.Key) && item.Value != item2.Value && !Corridors.ContainsKey(item2.Value))
                    {

                        bool angle = CheckAngle(item2.Key,item.Key,temp);
                        bool GoThroughRoom = CheckRooms(item2.Key,item.Key);
                        bool cross = CheckList(Corridors,item2.Key,item.Key);
                        float dist = Vector2.Distance(item.Key,item2.Key);
                        if(dist < tempDistance && angle && GoThroughRoom && !cross)
                        {
                            tempDistance = dist;
                            tempObject = item2.Key;
                        }

                    }
                }
                if(tempObject != Vector2.zero)
                    temp.Add(tempObject);
            }
            List<GameObject> teList = new List<GameObject>();
            foreach (var vector in temp)
            {
                GameObject te = new GameObject();
                Spawned.TryGetValue(vector, out te);
                teList.Add(te);
            }
            Corridors.Add(item.Value,teList);
        }
    }

    private bool CheckAngle(Vector2 ToCheck, Vector2 Base, List<Vector2> OtherPoints)
    {
        bool Checked = true;
        foreach (var item in OtherPoints)
        {
            var vectorToCheck = new Vector2(ToCheck.x - Base.x,ToCheck.y - Base.y);
            var vectorAgainst = new Vector2(item.x - Base.x, item.y - Base.y);
            var Angle = Vector2.Angle(vectorToCheck,vectorAgainst);
            if(Angle < 35 || Angle > 325)
                Checked = false;
        }
        
        return Checked;
    }
    
    private bool CheckRooms(Vector2 ToCheck, Vector2 Base)
    {
        bool CanSpawn = true;

        Spawned.Where(c => c.Key == ToCheck).FirstOrDefault().Value.GetComponent<BoxCollider2D>().enabled = false;

        RaycastHit2D hit = Physics2D.Linecast(ToCheck,Base);

        foreach (var item in Spawned)
        {
            if(item.Key != Base && item.Key != ToCheck)
            {
                if(item.Key == (Vector2)hit.collider.transform.position)
                    CanSpawn = false;
            }
        }

        Spawned.Where(c => c.Key == ToCheck).FirstOrDefault().Value.GetComponent<BoxCollider2D>().enabled = true;

        return CanSpawn;
    }

    private bool CheckList(Dictionary<GameObject,List<GameObject>> OtherPoints, Vector2 ToCheck, Vector2 BasePoint )
    {
        Spawned.Where(c => c.Key == ToCheck).FirstOrDefault().Value.GetComponent<BoxCollider2D>().enabled = false;
        Spawned.Where(c => c.Key == BasePoint).FirstOrDefault().Value.GetComponent<BoxCollider2D>().enabled = false;

        foreach (var item in OtherPoints)
        {
            foreach (var item2 in item.Value)
            {
                var P0 = BasePoint;
                var P1 = ToCheck;
                var P2 = item.Key.transform.position;
                var P3 = item2.transform.position;

                CrossCheck.SetPoints(new List<Vector2>{P2,P3});

                RaycastHit2D hit = Physics2D.Linecast(P0,P1);

                if(hit.transform != null && hit.transform.name == "Edge" && hit.point != P1 && hit.point != P0)
                {
                    Spawned.Where(c => c.Key == ToCheck).FirstOrDefault().Value.GetComponent<BoxCollider2D>().enabled = true;
                    Spawned.Where(c => c.Key == BasePoint).FirstOrDefault().Value.GetComponent<BoxCollider2D>().enabled = true;
                    return true;
                }
            }
        }
        
        Spawned.Where(c => c.Key == ToCheck).FirstOrDefault().Value.GetComponent<BoxCollider2D>().enabled = true;
        Spawned.Where(c => c.Key == BasePoint).FirstOrDefault().Value.GetComponent<BoxCollider2D>().enabled = true;

        return false;
    }

    private int GetNumberCorridor()
    {
        int randResult = UnityEngine.Random.Range(0,100);

        if(randResult < 25)
            return 1;
        else if(randResult < 75)
            return 2;
        else
            return 3;
    }
    
    private void SpawnCorridor()
    {
        foreach (var item in Corridors)
        {
            foreach (var item2 in item.Value)
            {
                if(item2 != null)
                {
                    var temp = item.Key.transform.position;
                    Vector3 Start = new Vector3(temp.x, temp.y, 75);
                    var l = Instantiate(Line, Start, Quaternion.identity,item.Key.transform);
                    l.name = "corridor";
                    var temp2 = item2.transform.position;
                    Vector3 End = new Vector3(temp2.x, temp2.y, 75);
                    var lineRenderer = l.GetComponent<LineRenderer>();
                    //lineRenderer.useWorldSpace = false;
                    lineRenderer.SetPosition(0,Start);
                    lineRenderer.SetPosition(1,End);
                    //lineRenderer.positionCount = 10;
                    var dottedLine = l.GetComponent<DottedLineRenderer>();
                    if (dottedLine != null) 
                        dottedLine.ScaleMaterial();
                    if ((Vector2)item2.transform.position != Vector2.zero)
                    {
                        Lines.Add(l);
                        item.Key.GetComponent<Room>().OwnedCorridors.Add(l);
                        item2.GetComponent<Room>().OwnedCorridors.Add(l);
                        if(!item.Key.GetComponent<Room>().ConnectedRooms.Contains(item2.GetComponent<Room>()))
                            item.Key.GetComponent<Room>().ConnectedRooms.Add(item2.GetComponent<Room>());
                        if(!item2.GetComponent<Room>().ConnectedRooms.Contains(item.Key.GetComponent<Room>()))
                            item2.GetComponent<Room>().ConnectedRooms.Add(item.Key.GetComponent<Room>());
                    }
                    else
                        Destroy(l);
                }
            }
        }
    }

    //public void AddLineConnection(MapNode from, MapNode to)
    //{
    //    var lineObject = Instantiate(linePrefab, nodeParent.transform);
    //    var lineRenderer = lineObject.GetComponent<LineRenderer>();
    //    lineRenderer.sortingOrder = 50;
    //    var fromPoint = from.transform.position +
    //                    (to.transform.position - from.transform.position).normalized * offsetFromNodes;

    //    var toPoint = to.transform.position +
    //                  (from.transform.position - to.transform.position).normalized * offsetFromNodes;

    //    // drawing lines in local space:
    //    lineObject.transform.position = fromPoint;
    //    lineRenderer.useWorldSpace = false;

    //    // line renderer with 2 points only does not handle transparency properly:
    //    lineRenderer.positionCount = linePointsCount;
    //    for (var i = 0; i < linePointsCount; i++)
    //    {
    //        lineRenderer.SetPosition(i,
    //            Vector3.Lerp(Vector3.zero, toPoint - fromPoint, (float)i / (linePointsCount - 1)));
    //    }
    //    var dottedLine = lineObject.GetComponent<DottedLineRenderer>();
    //    if (dottedLine != null) dottedLine.ScaleMaterial();

    //    lineConnections.Add(new LineConnection(lineRenderer, from, to));
    //}

    private void InitManager()
    {
        List<Room> ToInit = new List<Room>();

        foreach (var item in Spawned)
        {
            var room = item.Value.GetComponent<Room>();
            ToInit.Add(room);
        }

        ToInit[0].isStart = true;
        roomManager.Init(ToInit);
    }

    void ClearGen()
    {
        foreach (var item in Spawned)
        {
            Destroy(item.Value);
        }
        Spawned.Clear();
        
        foreach (var item in Lines)
        {
            Destroy(item);
        }
        Lines.Clear();

        Corridors.Clear();
    }

    void ClearUseless()
    {
        var t = SceneManager.GetSceneByName("Monde").GetRootGameObjects();
        List<GameObject> todestroy = new List<GameObject>();
        for (int i = 0; i < t.Count(); i++)
        {
            if(t[i].name == "New Game Object")
               todestroy.Add(t[i]);
        }
        foreach (var item in todestroy)
        {
            Destroy(item);
        }
    }
}