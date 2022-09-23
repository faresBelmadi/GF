using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#if UNITY_EDITOR
public class SkillTreeController : EditorWindow 
{

    public static SkillTreeController current;
    private List<Node> nodes;
    private List<Connection> connections;

    private GUIStyle nodeStyle;
    private GUIStyle selectedNodeStyle;
    private GUIStyle inPointStyle;
    private GUIStyle outPointStyle;

    private ConnectionPoint selectedInPoint;
    private ConnectionPoint selectedOutPoint;

    private Vector2 offset;
    private Vector2 drag;

    // Rect for buttons to Clear, Save and Load 
    private Rect rectButtonClear;
    // Rect for buttons to Clear, Save and Load 
    private Rect rectTextFieldClass;
    private Rect rectButtonSave;
    private Rect rectButtonLoad;

    private string idClassToSaveLoad;

    // Count for nodes created
    private int nodeCount;

    public ClassTreeEditor classTree;

    [MenuItem("LivingMemory/Class Tree Editor")]
    private static void OpenWindow()
    {
        current = GetWindow<SkillTreeController>();
        current.titleContent = new GUIContent("Class Tree Editor");
    }

    private void OnEnable()
    {

        nodeStyle = new GUIStyle();
        nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/lightskin/images/node5.png") as Texture2D;
        nodeStyle.border = new RectOffset(12, 12, 12, 12);

        selectedNodeStyle = new GUIStyle();
        selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node5 on.png") as Texture2D;
        selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);
        
        inPointStyle = new GUIStyle();
        inPointStyle.normal.background = Resources.Load("green") as Texture2D;
        inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
        inPointStyle.border = new RectOffset(4, 4, 12, 12);

        outPointStyle = new GUIStyle();
        outPointStyle.normal.background = Resources.Load("red") as Texture2D;
        outPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
        outPointStyle.border = new RectOffset(4, 4, 12, 12);

        // Create buttons for clear, save and load
        rectButtonClear = new Rect(new Vector2(10, 10), new Vector2(60,20));
        rectTextFieldClass = new Rect(new Vector2(80, 10), new Vector2(60, 20));
        rectButtonSave = new Rect(new Vector2(150, 10), new Vector2(60, 20));
        rectButtonLoad = new Rect(new Vector2(220, 10), new Vector2(60, 20));

        // Initialize nodes with saved data
        LoadNodes();
    }

    private void OnGUI()
    {
        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);

        // We draw our new buttons (Clear, Load and Save)
        DrawButtons();

        DrawNodes();
        DrawConnections();

        DrawConnectionLine(Event.current);

        ProcessNodeEvents(Event.current);
        ProcessEvents(Event.current);

        if (GUI.changed)
            Repaint();
    }

    private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
    {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        offset += drag * 0.5f;
        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

        for (int i = 0; i < widthDivs; i++)
        {
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
        }

        for (int j = 0; j < heightDivs; j++)
        {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
        }

        Handles.color = Color.red;
        Handles.EndGUI();
    }

    private void DrawNodes()
    {
        if (nodes != null)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Draw();
            }
        }
    }

    private void DrawConnections()
    {
        if (connections != null)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                connections[i].Draw();
            }
        }
    }

    // Draw our new buttons for managing the skill tree
    private void DrawButtons()
    {
        if (GUI.Button(rectButtonClear, "Clear"))
            ClearNodes();
        idClassToSaveLoad = GUI.TextField(rectTextFieldClass,idClassToSaveLoad);
        if (GUI.Button(rectButtonSave, "Save"))
        {
            if(idClassToSaveLoad != null)
                SaveNodes();
            else
                EditorUtility.DisplayDialog("Erreur","Veuillez rentrer un id de class a charger","réessayer");
        }
        if (GUI.Button(rectButtonLoad, "Load"))
        {
            if(idClassToSaveLoad != null)
                LoadNodes();
            else
                EditorUtility.DisplayDialog("Erreur","Veuillez rentrer un id de class a charger","réessayer");
            
        }
    }

    private void ProcessEvents(Event e)
    {
        drag = Vector2.zero;

        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    ClearConnectionSelection();
                }

                if (e.button == 1)
                {
                    ProcessContextMenu(e.mousePosition);
                }
                break;

            case EventType.MouseDrag:
                if (e.button == 0)
                {
                    OnDrag(e.delta);
                }
                break;
        }
    }

    private void ProcessNodeEvents(Event e)
    {
        if (nodes != null)
        {
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                bool guiChanged = nodes[i].ProcessEvents(e);

                if (guiChanged)
                {
                    GUI.changed = true;
                }
            }
        }
    }

    private void DrawConnectionLine(Event e)
    {
        if (selectedInPoint != null && selectedOutPoint == null)
        {
            Handles.DrawBezier(
                selectedInPoint.rect.center,
                e.mousePosition,
                selectedInPoint.rect.center,
                e.mousePosition,
                Color.red,
                null,
                2f
            );

            GUI.changed = true;
        }

        if (selectedOutPoint != null && selectedInPoint == null)
        {
            Handles.DrawBezier(
                selectedOutPoint.rect.center,
                e.mousePosition,
                selectedOutPoint.rect.center,
                e.mousePosition,
                Color.red,
                null,
                2f
            );

            GUI.changed = true;
        }
    }

    private void ProcessContextMenu(Vector2 mousePosition)
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Add node"), false, () => CreatePopUp(mousePosition));
        genericMenu.ShowAsContext();
    }

    private void OnDrag(Vector2 delta)
    {
        drag = delta;

        if (nodes != null)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Drag(delta);
            }
        }

        GUI.changed = true;
    }
    public void CreatePopUp(Vector2 mousePosition)
    {
        PopUpSpell.pos = mousePosition;
        PopUpSpell.ShowWindow();
    }
    public void OnClickAddNode(Vector2 mousePosition,string id)
    {
        if (nodes == null)
        {
            nodes = new List<Node>();
        }
        
        // We create the node with the default info for the node
        nodes.Add(new Node(mousePosition, 200, 100, nodeStyle, selectedNodeStyle,
            inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode,
            classTree.classe.spellClass.First(c => c.IDSpell.ToString() == id)));
        ++nodeCount;
    }

    private void OnClickInPoint(ConnectionPoint inPoint)
    {
        selectedInPoint = inPoint;

        if (selectedOutPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }

    private void OnClickOutPoint(ConnectionPoint outPoint)
    {
        selectedOutPoint = outPoint;
        if (selectedInPoint != null)
        {
            
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }

    private void OnClickRemoveNode(Node node)
    {
        if (connections != null)
        {
            List<Connection> connectionsToRemove = new List<Connection>();

            for (int i = 0; i < connections.Count; i++)
            {
                if (connections[i].inPoint == node.inPoint || connections[i].outPoint == node.outPoint)
                {
                    connectionsToRemove.Add(connections[i]);
                }
            }

            for (int i = 0; i < connectionsToRemove.Count; i++)
            {
                connections.Remove(connectionsToRemove[i]);
            }

            connectionsToRemove = null;
        }

        nodes.Remove(node);
    }

    private void OnClickRemoveConnection(Connection connection)
    {
        if(connection.outPoint.node.SpellNode.IDChildren.Contains(connection.inPoint.node.SpellNode.IDSpell))
            connection.outPoint.node.SpellNode.IDChildren.Remove(connection.inPoint.node.SpellNode.IDSpell);
        connections.Remove(connection);
    }

    private void CreateConnection()
    {
        if (connections == null)
        {
            connections = new List<Connection>();
        }
        connections.Add(new Connection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));
        if(!selectedOutPoint.node.SpellNode.IDChildren.Contains(selectedInPoint.node.SpellNode.IDSpell))
            selectedOutPoint.node.SpellNode.IDChildren.Add(selectedInPoint.node.SpellNode.IDSpell);
    }

    private void ClearConnectionSelection()
    {
        selectedInPoint = null;
        selectedOutPoint = null;
    }
    
    // Function for clearing data from the editor window
    private void ClearNodes()
    {
        nodeCount = 0;
        if (nodes != null && nodes.Count > 0)
        {
            Node node;
            while (nodes.Count > 0)
            {
                node = nodes[0];

                OnClickRemoveNode(node);
            }
        }
    }
    private void SaveNodes()
    {
        DataClass data = new DataClass();
        NodeDataCollection nodeData = new NodeDataCollection();
        nodeData.nodeDataCollection = new NodeData[nodes.Count];

        for (int i = 0; i < nodes.Count; ++i)
        {
            nodeData.nodeDataCollection[i] = new NodeData();
            nodeData.nodeDataCollection[i].spellId = nodes[i].SpellNode.IDSpell;
            nodeData.nodeDataCollection[i].position = nodes[i].rect.position;
        }
        data.nodes = nodeData;
        data.classID = int.Parse(idClassToSaveLoad);
        string json = JsonUtility.ToJson(data);
        string path = "Assets/Resources/ClassJson/Class"+idClassToSaveLoad+".json";

        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.Write(json);
            }
        }
        UnityEditor.AssetDatabase.Refresh();
    }
    
    private void LoadNodes()
    {
        ClearNodes();
        if(idClassToSaveLoad != null)
        {
            var t = Resources.FindObjectsOfTypeAll<ClassPlayer>().ToList();
            classTree = new ClassTreeEditor();
            classTree.classe = t.First(c => c.ID.ToString() == idClassToSaveLoad);
            string path = "Assets/Resources/ClassJson/Class"+idClassToSaveLoad+".json";
            string dataAsJson;
            DataClass loadedData;
            if (File.Exists(path))
            {
                // Read the json from the file into a string
                dataAsJson = File.ReadAllText(path);

                // Pass the json to JsonUtility, and tell it to create a SkillTree object from it
                loadedData = JsonUtility.FromJson<DataClass>(dataAsJson);
                Vector2 pos = Vector2.zero;

            
                
                //Create nodes
                for (int j = 0; j < loadedData.nodes.nodeDataCollection.Length; ++j)
                {
                    pos = loadedData.nodes.nodeDataCollection[j].position;
                    LoadSkillCreateNode(loadedData.nodes.nodeDataCollection[j].spellId, pos);
                }
                
                
                LoadConnection();
                
            }
            else
                Debug.LogError("Wrong path");
        }
    }

    private void LoadSkillCreateNode(int spellID, Vector2 position)
    {
        if (nodes == null)
        {
            nodes = new List<Node>();
        }
        var spell = classTree.classe.spellClass.First(c => c.IDSpell == spellID);
        nodes.Add(new Node(position, 200, 100, nodeStyle, selectedNodeStyle,
            inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode, 
            spell));
        
        ++nodeCount;
    }

    public static void ReloadNode(Node toDelete, string newSpellID)
    {
        Vector2 nodePos = toDelete.rect.position;
        var newSpell = current.classTree.classe.spellClass.First(c => c.IDSpell.ToString() == newSpellID);
        current.nodes.Add(new Node(nodePos, 200, 100, current.nodeStyle,current.selectedNodeStyle,
                                   current.inPointStyle,current.outPointStyle,current.OnClickInPoint,
                                   current.OnClickOutPoint,current.OnClickRemoveNode,newSpell));
        current.OnClickRemoveNode(toDelete);
        current.LoadConnection();
    }

    private void LoadConnection()
    {
        Node outnode = null;
        for (int i = 0; i < nodes.Count; ++i)
        {
            for (int j = 0; j < nodes[i].SpellNode.IDChildren.Count; ++j)
            {
                for (int k = 0; k < nodes.Count; ++k)
                {
                    if (nodes[k].SpellNode.IDSpell == nodes[i].SpellNode.IDChildren[j])
                    {
                        outnode = nodes[k];
                        OnClickInPoint(outnode.inPoint);
                        break;
                    }
                }
                OnClickOutPoint(nodes[i].outPoint);
            }
        }
        ClearConnectionSelection();
    }
} 
#endif