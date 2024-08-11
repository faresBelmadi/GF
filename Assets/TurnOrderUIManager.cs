using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEditor.Progress;

public class TurnOrderUIManager : MonoBehaviour
{
    [SerializeField] private GameObject turnItemPrefab;
    [SerializeField] private RectTransform turnItemHolder;
    [SerializeField] private float switchTurnDelay;

    [SerializeField] private BattleManager battleManager;

    private List<Tuple<int, GameObject>> turnTuple = new List<Tuple<int, GameObject>>();
    private Coroutine evolveTurnOrderRoutine = null;
    public void GenerateTurnItems(List<CombatOrder> IdOrder)
    {
        foreach (Tuple<int, GameObject> turnItem in turnTuple)
        {
            if (turnItem == null) return;
            Destroy(turnItem.Item2);
        }
        turnTuple = new List<Tuple<int, GameObject>>();

        for (int i = 0; i < IdOrder.Count; i++)
        {
            EnnemyBehavior ennemyBehavior = battleManager.EnemyScripts.FirstOrDefault(c => c.combatID == IdOrder[i].id);
            string entityName = "Player";
            GameObject cible = null;
            if (IdOrder[i].id != battleManager.idPlayer)
            {
                entityName = ennemyBehavior.name.TrimEnd("Variant(Clone)".ToCharArray());
                cible = ennemyBehavior.GetComponent<UIEnnemi>().Ciblage;
            }

            GameObject turnItem = Instantiate(turnItemPrefab,turnItemHolder);
            turnItem.GetComponentInChildren<TextMeshProUGUI>().text = entityName;
            turnItem.GetComponent<TargetableTurnOrderItem>().Ciblage = cible;
            turnTuple.Add(new Tuple<int,GameObject>(IdOrder[i].id, turnItem));
        }
    }
    public void EvovlveTurnOrder()
    {
        if(evolveTurnOrderRoutine != null)
        {
            StopCoroutine(evolveTurnOrderRoutine);
        }
        StartCoroutine(EvovlveTurnOrderCoroutine());
    }
    IEnumerator EvovlveTurnOrderCoroutine()
    {
        if(turnTuple.Count > 0)
        {
            float itemLength = turnTuple[0].Item2.GetComponent<RectTransform>().sizeDelta.x;
            Vector2 defaultPos = GetComponent<RectTransform>().pivot;

            turnItemHolder.localPosition = defaultPos;//new Vector3(-barSemiSize, 0f, 0f);
            if(switchTurnDelay > 0f)
            {
                while (turnItemHolder.localPosition.x > -(defaultPos.x + itemLength))
                {
                    turnItemHolder.localPosition += Vector3.left * (itemLength / switchTurnDelay) * Time.deltaTime;
                    yield return null;
                }
            }
            Destroy(turnTuple[0].Item2);
            turnTuple.RemoveAt(0);
            turnItemHolder.localPosition = defaultPos;//new Vector3(-(defaultPos.x + itemLength), 0f, 0f);
        }
        evolveTurnOrderRoutine = null;
        battleManager.StartNextTurn();
    }
    public void RemoveDeadsTurn(int ennemyId)
    {
        Debug.Log($"Removing {ennemyId} from turnOrderUI");
        List<int> indexToRemove = new List<int>();
        for (int i = turnTuple.Count-1; i >= 0 ; i--)
        {
            if (turnTuple[i].Item1 == ennemyId)
            {
                indexToRemove.Add(i);
            }
        }
        foreach (int index in indexToRemove)
        {
            Destroy(turnTuple[index].Item2);
            turnTuple.RemoveAt(index);
        }
    }
    
}
