using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleLog : MonoBehaviour
{
    [SerializeField]
    private GameObject _battleLogLinePrefabGO;
    [SerializeField]
    private GameObject _contentHolder;
    [SerializeField]
    private int _maxLine = 10;
    private List<GameObject> _logList;
    [SerializeField]
    private ScrollRect _scrollRect;
    [SerializeField]
    private GameObject _battleLogObject;


    private void OnEnable()
    {
        _logList = new List<GameObject>();

    }
    private void OnDisable()
    {
        for(int i=_logList.Count -1; i>=0;i--)
        {
            Destroy(_logList[i]);
        }
        _logList.Clear();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            ToggleBattleLog(!_battleLogObject.activeSelf);
        }

    }

    public void ToggleBattleLog(bool toggle)
    {
        _battleLogObject.SetActive(toggle);
    }
    public void ShowBattleLog()
    {
        ToggleBattleLog(true);
    }
    public void HideBattleLog()
    {
        ToggleBattleLog(false);
    }
    public void AddBattleLaunchSpellLogLine(CombatBehavior launcher, IBattleLogSpell spell)
    {
        GameObject line = Instantiate(_battleLogLinePrefabGO, _contentHolder.transform);
        line.GetComponent<TMP_Text>().text = $"{launcher.Name} launches {spell.TradName}";
        if (_logList.Count >= _maxLine) 
        {
            Destroy(_logList[0]);
            _logList.RemoveAt(0);
        }
        _logList.Add(line);
        _scrollRect.verticalNormalizedPosition = 0;
        Canvas.ForceUpdateCanvases();
    }
    public void AddDamageLogLine(CombatBehavior target, CombatBehavior source, int amount)
    {
        if (amount == 0) return;
        GameObject line = Instantiate(_battleLogLinePrefabGO, _contentHolder.transform);
        if (amount < 0)
        {
            line.GetComponent<TMP_Text>().text = $"{target.Name} take {Mathf.Abs(amount)} {(source != null?" from " + source.Name:"")}"; 
        }
        else
            line.GetComponent<TMP_Text>().text = $"{target.Name} heals {amount} radiance point";
        if (_logList.Count >= _maxLine)
        {
            Destroy(_logList[0]);
            _logList.RemoveAt(0);
        }
        _logList.Add(line);
        _scrollRect.verticalNormalizedPosition = 0;
        Canvas.ForceUpdateCanvases();
    }

}
