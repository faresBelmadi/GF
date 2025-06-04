using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ClairvoyancePanel : MonoBehaviour
{
    [SerializeField]
    private GameObject _contentHolder;
    [SerializeField]
    private string _idTradClairvoyanceHint;
    [SerializeField]
    private string _idTradBonus;
    [SerializeField]
    private string _idTradMalus;

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
        
        stringBuilder.AppendLine(TradManager.instance.GetTranslation(_idTradClairvoyanceHint, "Your Clairyoyance tells you that this answer grants you : "));
        for (int i = 0; i < _effectList.Count; i++)
        {
            if (_effectList[i].Item2)
            {
                stringBuilder.Append(TradManager.instance.GetTranslation(_idTradBonus, "Bonus"));
            }
            else 
            {
                stringBuilder.Append(TradManager.instance.GetTranslation(_idTradMalus, "Malus"));
            }
            stringBuilder.Append($" {_effectList[i].Item1.GetTargetStat()}");
            stringBuilder.AppendLine();
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
