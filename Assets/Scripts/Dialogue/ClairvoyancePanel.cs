using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ClairvoyancePanel : MonoBehaviour
{
    [SerializeField]
    private GameObject _contentHolder;

    private List<(Effet, bool)> _effectList = new List<(Effet, bool)>();
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string PrintListOfEffect()
    {
        StringBuilder stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("Your Clairyoyance tells you that this answer grants you : ");
        for (int i = 0; i < _effectList.Count; i++)
        {
            stringBuilder.AppendLine($"One {(_effectList[i].Item2 ? "bonus" : "malus")} on {_effectList[i].Item1.GetTargetStat()}");
        }

        return stringBuilder.ToString();
    }
    public void AddEffect(Effet effectToAdd, bool isBonus)
    {
        _effectList.Add((effectToAdd, isBonus));
    }
    public void InitClairvoyancePanel()
    {
        _effectList = new List<(Effet, bool)>();
    }
    public void ClearClairvoyancePanel()
    {
        //for (int i = _effectList.Count -1;i>=0;i--)
        //{
        //    Destroy(_effectList[i].Item1);
        //}
        _effectList.Clear();
    }
}
