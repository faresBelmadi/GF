using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class CombatBehavior : MonoBehaviour
{

    public List<GameObject> ListBuffDebuffGO = new List<GameObject>();
    public GameObject BuffPrefab;
    public Transform BuffContainer;
    public GameObject DebuffPrefab;
    public Transform DebuffContainer;

    public Action EndTurnBM;

    public int LastDamageTaken;

    public void AddBuffDebuff(BuffDebuff toAdd)
    {
        if (toAdd.IsDebuff)
        {
            var t = ListBuffDebuffGO.FirstOrDefault(c => c.GetComponentInChildren<TextMeshProUGUI>().text == toAdd.Nom);
            if (t == null)
            {
                t = Instantiate(DebuffPrefab, DebuffContainer.transform);
                t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNom").text = toAdd.Nom;
                t.GetComponent<DescriptionHoverTrigger>().Description.text = toAdd.Description;
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
                t.GetComponent<DescriptionHoverTrigger>().Description.text = toAdd.Description;
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

    public ObservableList<BuffDebuff> DecompteDebuff(ObservableList<BuffDebuff> BuffDebuff, Decompte Timer)
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

        var test = BuffDebuff.Where(c => c.Temps < 0).ToList();
        BuffDebuff.Remove(test.ToArray());
        return BuffDebuff;
    }

    public void UpdateBuffDebuffGameObject(ObservableList<BuffDebuff> BuffDebuff)
    {
        DecompteDebuff(BuffDebuff, Decompte.none);
    }


    //TODO Remove when souvenir and emotions working ( should be done directly in Joie.cs)

    #region eventAddBuff

    public static int nbBuff = 0;
    public void ListBuffDebuff_ItemAdded(ObservableList<BuffDebuff> sender, ListChangedEventArgs<BuffDebuff> e)
    {
        nbBuff++;
        Debug.Log("Buff Debuff Added " + e.index + " - " + e.item.Nom + " - " + nbBuff);
        Debug.Log(sender.Contains(e.item) + " est ce qu'il y est déja?");
        if (nbBuff == 2) //checker si les 5 ne sont pas déja appliqué, et si ils ne l'ont pas été de tout le combat?
        {
            ApplyBonusJoie();
            nbBuff = 0;
        }

    }

    public void ListBuffDebuff_ItemRemoved(ObservableList<BuffDebuff> sender, ListChangedEventArgs<BuffDebuff> e)
    {
        Debug.Log("Buff Debuff Removed " + e.index + " - " + e.item.Nom);
    }

    public void ApplyBonusJoie()
    {
        Debug.Log("5 bonus diff reçue");
        //appliquer un debuff a l'ennemie (sur la prochaine action) de degat de xx par cout d'activation
    }

    #endregion

}
