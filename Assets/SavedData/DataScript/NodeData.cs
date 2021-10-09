using UnityEngine;

[System.Serializable]
public class NodeData {

    public Vector2 position;
    public int spellId;
}

[System.Serializable]
public class NodeDataCollection
{
    public NodeData[] nodeDataCollection;
}

[System.Serializable]
public class DataClass
{
    public NodeDataCollection nodes;
    public int classID;
}
