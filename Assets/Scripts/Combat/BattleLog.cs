using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleLog : MonoBehaviour
{
    [SerializeField]
    private GameObject _battleLogLinePrefabGO;
    [SerializeField]
    private GameObject _contentHolder;
    [SerializeField]
    private int _maxLine = 10;
    private List<GameObject> _logList;


    private void OnEnable()
    {
        _logList = new List<GameObject>();
    }
    private void OnDisable()
    {
        for(int i=_logList.Count -1; i>=0;i++)
        {
            Destroy(_logList[i]);
        }
        _logList.Clear();
    }


    public void AddBattleLogLine(CombatBehavior launcher, IBattleLogSpell spell)
    {
        GameObject line = Instantiate(_battleLogLinePrefabGO, _contentHolder.transform);

        line.GetComponent<TMP_Text>().text = $"{launcher.Name} launch {spell.TradName}";
        if (_logList.Count >= _maxLine) 
        {
            Destroy(_logList[0]);
            _logList.RemoveAt(0);
        }
        _logList.Add(line);
    }
}
