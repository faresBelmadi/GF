using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace GF.GgsToCsv
{
    public class GgsToCsvValidator
    {
        public void Validate()
        {
            var tradGO = new GameObject("EDITOR_TRADMANAGER");
            tradGO.hideFlags = HideFlags.HideAndDontSave;
            var tradMgr = tradGO.AddComponent<TradManager>();

            bool allGood = true;
            allGood &= tradMgr.LoadTrad();

            var allIds = tradMgr.DialogueIds.Select(id => new { Id = id, From = "Game" })
                .Concat(tradMgr.CapaIds.Select(id => new { Id = id, From = "Capa" }))
                .Concat(tradMgr.MiscIds.Select(id => new { Id = id, From = "Misc" }))
                .GroupBy(p => p.Id)
                .ToDictionary(g => g.Key, g => g.Select(item => item.From).ToArray());
            var duplicatedIds = allIds.Where(id => id.Value.Length > 1);
            if (duplicatedIds.Any())
            {
                allGood = false;
                foreach (var id in duplicatedIds)
                {
                    Debug.LogError($"Id {id.Key} is duplicated (found in {string.Join(" and ", id.Value)})");
                }
            }

            if (allGood)
            {
                Debug.Log("Validation says trad files are all good!");
            }
        }
    }
}