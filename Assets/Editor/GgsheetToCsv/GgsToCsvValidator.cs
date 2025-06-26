using System.Linq;
using UnityEngine;


namespace GF.GgsToCsv
{
    public class GgsToCsvValidator
    {
        public void Validate()
        {
            var tradMgr = TradManager.CreateEditorInstance();
            if (tradMgr.LoadTrad(logAllErrors:true))
            {
                Debug.Log("Validation says trad files are all good!");
            }
        }
    }
}