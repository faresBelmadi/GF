using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class SkillTreePrinter : MonoBehaviour
{
    DataClass loadedData;
    public NodeDataCollection nodeToPrint;

    public GameObject SkillTreeContainer;
    public ScrollRect ScrollView;
    public GameObject InspectorContainer;
    public TextMeshProUGUI EssenceHeader;
    public TextMeshProUGUI ClassTitle;
    public TextMeshProUGUI SpellTitle;
    public TextMeshProUGUI SpellDescription;
    public TextMeshProUGUI SpellCost;
    public TextMeshProUGUI SpellCombatCost;

    public List<GameObject> SpawnedNodes;
    public List<GameObject> SpawnedLines;
    public GameObject NodePrefab;
    public GameObject LinePrefab;

    public List<TextAsset> ListJson;

    private void Awake() {
        SpawnedLines = new List<GameObject>();
        SpawnedNodes = new List<GameObject>();
        ClassTitle.text = GameManager.Instance.classSO.NameClass;
        EssenceHeader.text = "Essence : " + GameManager.Instance.playerStat.Essence;
        LoadNodes();
    }

    void LoadNodes()
    {
        string dataAsJson = ListJson.First(c => c.name == "Class"+GameManager.Instance.classSO.ID).text;

       
        loadedData = JsonUtility.FromJson<DataClass>(dataAsJson);

        nodeToPrint = loadedData.nodes;

        SpawnNodes();
        SpawnLines();
        foreach (var item in SpawnedNodes)
        {
            item.GetComponent<SpellUI>().FinishSetUp();
        }
    }

    void SpawnNodes()
    {
        for (int i = 0; i < nodeToPrint.nodeDataCollection.Length; i++)
        {
            var temp = Instantiate(NodePrefab,nodeToPrint.nodeDataCollection[i].position,Quaternion.identity,SkillTreeContainer.transform);
            temp.name = nodeToPrint.nodeDataCollection[i].spellId +"";
            ((RectTransform)temp.transform).localPosition = new Vector3(nodeToPrint.nodeDataCollection[i].position.x,-nodeToPrint.nodeDataCollection[i].position.y,0);
            SpawnedNodes.Add(temp);
            temp.GetComponent<SpellUI>().LinkedSpell = GameManager.Instance.classSO.spellClass.First(c => c.IDSpell == nodeToPrint.nodeDataCollection[i].spellId);
        }
        var t = ((RectTransform)SpawnedNodes.First(c => c.name == "0").transform).localPosition;
        ScrollView.content.localPosition -= new Vector3(t.x + 200,t.y-100,0); // ScrollRectExtensions.GetSnapToPositionToBringChildIntoView(ScrollView,(RectTransform)SpawnedNodes.First(c => c.name == "id : 0").transform);
    }

    void SpawnLines()
    {
        foreach (var item in SpawnedNodes)
        {
            Spell currentSpell = GameManager.Instance.classSO.spellClass.First(c => c.IDSpell.ToString() == item.name);
            for (int i = 0; i < currentSpell.IDChildren.Count; i++)
            {
                var destination = SpawnedNodes.First(d => d.name == currentSpell.IDChildren[i]+"");
                if(destination != null)
                {
                    var tempLine = Instantiate(LinePrefab,Vector2.zero,Quaternion.identity,SkillTreeContainer.transform);
                    tempLine.GetComponent<UILineRenderer>().Points = new Vector2[2];
                    tempLine.GetComponent<UILineRenderer>().Points[0] = ((RectTransform)item.transform).localPosition;
                    tempLine.GetComponent<UILineRenderer>().Points[1] = ((RectTransform)destination.transform).localPosition;
                    ((RectTransform)tempLine.transform).localPosition = Vector2.zero;
                    tempLine.transform.SetAsFirstSibling();
                    SpawnedLines.Add(tempLine);
                    item.GetComponent<SpellUI>().Lines.Add(tempLine);
                }
            }
        }
    }

    public void UpdateSkillTree(SpellUI ToUpdate)
    {
        GameManager.Instance.playerStat.Essence -= ToUpdate.LinkedSpell.CostUnlock;
        EssenceHeader.text = "Essence : " + GameManager.Instance.playerStat.Essence;
        foreach (var item in ToUpdate.LinkedSpell.IDChildren)
        {
            var t = SpawnedNodes.First(c => c.name == item + "");
            if(t !=null)
            {
                t.GetComponent<SpellUI>().LinkedSpell.SpellStatue = SpellStatus.unlocked;
                t.GetComponent<SpellUI>().UpdateVisual();
            }
        }
    }

    public void ShowInspectorOver(Spell toShow)
    {
        SpellTitle.text = toShow.Nom;
        SpellCost.text = "Price : " + toShow.CostUnlock;
        InspectorContainer.SetActive(true);
    }

    public void HideInspector()
    {
        InspectorContainer.SetActive(false);
    }
}
