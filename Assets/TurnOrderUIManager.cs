using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class TurnOrderUIManager : MonoBehaviour
{
    [SerializeField] private GameObject turnItemPrefab;
    [SerializeField] private RectTransform turnItemHolder;
    [SerializeField] private TextMeshProUGUI curentlyPlayingEntity;
    [SerializeField] private float switchTurnDelay;
    [SerializeField] private float turItemBaseScale;

    [SerializeField] private RectTransform curtain;
    [SerializeField] private float curtainMovmentDuration;
    [SerializeField] private float curtainSustainTime;
    [SerializeField] private AnimationCurve curtainAnimation;
    [SerializeField] private PulseBloom_System curtainBloomSystem;

    [SerializeField] private BattleManager battleManager;

    private List<Tuple<int, GameObject>> turnTuple = new List<Tuple<int, GameObject>>();
    private Coroutine evolveTurnOrderRoutine = null;
    private Coroutine generateNextTurnOrderRoutine = null;
    public void GenerateNextTurnOrder(List<CombatOrder> idOrder)
    {
        if (generateNextTurnOrderRoutine != null)
        {
            StopCoroutine(generateNextTurnOrderRoutine);
        }
        generateNextTurnOrderRoutine = StartCoroutine(GenerateNextTurnOrderCoroutine(idOrder));
    }

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
            Sprite icon = battleManager.player.Stat.Icon;
            Material charMaterial = null;
            if (IdOrder[i].id != battleManager.idPlayer)
            {
                entityName = ennemyBehavior.name.TrimEnd("Variant(Clone)".ToCharArray());
                cible = ennemyBehavior.GetComponent<UIEnnemi>().Ciblage;
                icon = ennemyBehavior.Stat.Icon;
                charMaterial = ennemyBehavior.characterMaterial;
            }

            GameObject turnItem = Instantiate(turnItemPrefab,turnItemHolder);
            turnItem.GetComponentInChildren<Image>().sprite = icon;
            turnItem.GetComponentInChildren<Image>().material= charMaterial;

            turnItem.transform.localScale = Vector3.one * turItemBaseScale;
            //turnItem.GetComponentInChildren<TextMeshProUGUI>().text = entityName;
            turnItem.GetComponent<TargetableTurnOrderItem>().Ciblage = cible;
            turnItem.GetComponent<TargetableTurnOrderItem>().entityName = entityName;
            turnTuple.Add(new Tuple<int,GameObject>(IdOrder[i].id, turnItem));
        }
        turnTuple[0].Item2.transform.localScale = Vector3.one;
        curentlyPlayingEntity.text = turnTuple[0].Item2.GetComponent<TargetableTurnOrderItem>().entityName;
    }
    public void EvovlveTurnOrder()
    {
        curentlyPlayingEntity.text = turnTuple[1].Item2.GetComponent<TargetableTurnOrderItem>().entityName;
        if (evolveTurnOrderRoutine != null)
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
            Vector3 defaultPos = GetComponent<RectTransform>().pivot;

            turnItemHolder.localPosition = defaultPos;//new Vector3(-barSemiSize, 0f, 0f);
            if(switchTurnDelay > 0f)
            {
                float time = 0f;
                while (time < switchTurnDelay)
                {
                    time += Time.deltaTime;
                    turnItemHolder.localPosition = defaultPos + Vector3.left * itemLength * (time/switchTurnDelay);
                    turnTuple[1].Item2.transform.localScale = Vector3.one * (turItemBaseScale + (1f - turItemBaseScale)* (time / switchTurnDelay));
                    yield return null;
                }
                /*
                while (turnItemHolder.localPosition.x > -(defaultPos.x + itemLength))
                {
                    turnItemHolder.localPosition += Vector3.left * (itemLength / switchTurnDelay) * Time.deltaTime;
                    yield return null;
                } 
                */
            }
            Destroy(turnTuple[0].Item2);
            turnTuple.RemoveAt(0);
            turnItemHolder.localPosition = defaultPos;//new Vector3(-(defaultPos.x + itemLength), 0f, 0f);
        }
        evolveTurnOrderRoutine = null;
        battleManager.StartNextTurn();
    }

    IEnumerator GenerateNextTurnOrderCoroutine(List<CombatOrder> idOrder)
    {
        float curtainHeight = curtain.rect.height;
        Vector2 defaultPos = Vector2.up * curtainHeight;
        Debug.Log($"DropCurtain, height:{curtainHeight}");
        curtain.localPosition = defaultPos;
        if (curtainMovmentDuration > 0f)
        {
            
            float time = 0f;
            while(time < curtainMovmentDuration)
            {
                time += Time.deltaTime;
                curtain.localPosition = Vector3.up * curtainHeight * curtainAnimation.Evaluate(time / curtainMovmentDuration);
                yield return null;
            }
        }
        curtain.localPosition = Vector3.zero;
        Debug.Log($"SustainCurtain");
        GenerateTurnItems(idOrder);
        curtainBloomSystem.TriggerBloom();
        yield return new WaitForSecondsRealtime(curtainSustainTime);

        Debug.Log($"RemoveCurtain");
        curtain.localPosition = Vector3.zero;
        if (curtainMovmentDuration > 0f)
        {
            
            float time = 0f;
            while (time < curtainMovmentDuration)
            {
                time += Time.deltaTime;
                curtain.localPosition = Vector3.up * curtainHeight * curtainAnimation.Evaluate(1f-(time / curtainMovmentDuration));
                yield return null;
            }
        }
        curtain.localPosition = defaultPos;
        Debug.Log($"Start Next Turn");
        generateNextTurnOrderRoutine = null;
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
