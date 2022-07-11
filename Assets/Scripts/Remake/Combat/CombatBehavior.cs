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
    public int TensionAttaque = 4;
    public int TensionDebuff = 2;
    public int TensionDot = 1;
    public int TensionSoin = -3;
    public float Tension;
    public float TensionMax;
    public float ValeurPalier;
    public int NbPalier = 3;

    public List<GameObject> ListBuffDebuff = new List<GameObject>();
    public GameObject BuffPrefab;
    public Transform BuffContainer;
    public GameObject DebuffPrefab;
    public Transform DebuffContainer;

    public Action EndTurnBM;

    public void AddUIBuffDebuff(BuffDebuffRemake toAdd)
    {
        if (toAdd.IsDebuff)
        {
            var t = ListBuffDebuff.FirstOrDefault(c => c.GetComponentInChildren<TextMeshProUGUI>().text == toAdd.Nom);
            if (t == null)
            {
                t = Instantiate(DebuffPrefab, DebuffContainer.transform);
                t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNom").text = toAdd.Nom;
                t.GetComponent<DescriptionHoverTrigger>().Description.text = toAdd.Description;
                ListBuffDebuff.Add(t);
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
            var t = ListBuffDebuff.FirstOrDefault(c => c.GetComponentInChildren<TextMeshProUGUI>().text == toAdd.Nom);
            if (t == null)
            {
                t = Instantiate(BuffPrefab, BuffContainer.transform);
                t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNom").text = toAdd.Nom;
                t.GetComponent<DescriptionHoverTrigger>().Description.text = toAdd.Description;
                ListBuffDebuff.Add(t);
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
}
