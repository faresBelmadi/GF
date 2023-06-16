using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatBehavior : MonoBehaviour
{

    public List<GameObject> ListBuffDebuffGO = new List<GameObject>();
    public GameObject BuffPrefab;
    public Transform BuffContainer;
    public GameObject DebuffPrefab;
    public Transform DebuffContainer;

    public Action EndTurnBM;

    public int LastDamageTaken;
    public bool gainedTension;

    public void AddBuffDebuff(BuffDebuff toAdd)
    {
        if (toAdd.IsDebuff)
        {
            var t = ListBuffDebuffGO.FirstOrDefault(c => c.GetComponentInChildren<TextMeshProUGUI>().text == toAdd.Nom);
            if (t == null)
            {
                t = Instantiate(DebuffPrefab, DebuffContainer.transform);
                t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNom").text = toAdd.Nom;
                t.GetComponent<DescriptionHoverTriggerBuffDebuff>().Description.text = toAdd.Description;
                ListBuffDebuffGO.Add(t);
            }
            else
            {
                var s = t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNb").text;
                int nb = int.Parse(s);
                s = nb + 1 + "";
                t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNb").text = s;
            }

        }
        else
        {
            var t = ListBuffDebuffGO.FirstOrDefault(c => c.GetComponentInChildren<TextMeshProUGUI>().text == toAdd.Nom);
            if (t == null)
            {
                t = Instantiate(BuffPrefab, BuffContainer.transform);
                t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNom").text = toAdd.Nom;
                t.GetComponent<DescriptionHoverTriggerBuffDebuff>().Description.text = toAdd.Description;
                ListBuffDebuffGO.Add(t);
            }
            else
            {
                var s = t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNb").text;
                int nb = int.Parse(s);
                s = nb + 1 + "";
                t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNb").text = s;
            }
        }
    }

    public List<BuffDebuff> DecompteDebuff(List<BuffDebuff> BuffDebuff, Decompte Timer)
    {
        foreach (var item in BuffDebuff)
        {
            if (item.Decompte == Timer)
                item.Temps--;

            if (item.Temps < 0)
            {
                var t = ListBuffDebuffGO.FirstOrDefault(c => c.GetComponentInChildren<TextMeshProUGUI>().text == item.Nom);
                if (t != null)
                {
                    var s = t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNb").text;
                    int nb = int.Parse(s);
                    nb -= 1;
                    s = nb + "";
                    if (nb <= 0)
                    {
                        ListBuffDebuffGO.Remove(t);
                        GameObject.Destroy(t);
                    }
                    else
                        t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNb").text = s;
                }
            }
        }

        BuffDebuff.RemoveAll(c => c.Temps < 0);
        return BuffDebuff;
    }

    public void UpdateBuffDebuffGameObject(List<BuffDebuff> BuffDebuff)
    {
        DecompteDebuff(BuffDebuff, Decompte.none);
    }

}
