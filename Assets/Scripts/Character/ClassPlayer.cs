using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewClass", menuName = "Create New Playable Class", order = 2)]
public class ClassPlayer : ScriptableObject
{
    public int ID;
    [Tooltip("Default value for missing translation")]
    [SerializeField]
    private string nameClass;
    [SerializeField]
    private string _idTradName;
    public JoueurStat PlayerStat;

    public List<Spell> spellClass;
    public int NbMaxSpell;

    public List<Competence> Competences;

    public string NameClass { get { return TradManager.instance.GetTranslation(_idTradName, nameClass); } }
}
