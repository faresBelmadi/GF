using System.Drawing;
using UnityEngine;

[CreateAssetMenu(fileName = "New Alea Encounter", menuName = "Encounter/Create New Alea", order = 11)]
public class EncounterAlea : ScriptableObject
{
    [Header("Dialogue")]
    public DialogueSO DialogueRencontre;
    [Header("PNJ")]
    public GameObject Pnj;
    public string NamePnj;
    [field: SerializeField]
    public string IdTradName;
}
