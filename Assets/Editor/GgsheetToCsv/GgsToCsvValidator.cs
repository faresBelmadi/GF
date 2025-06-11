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

            if (tradMgr.LoadTrad())
            {
                Debug.Log("Validation says trad files are all good!");
            }
        }
    }
}