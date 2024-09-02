using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New spell", menuName = "Capacité/Create New Spell", order = 11)]
public class Spell : ScriptableObject
{
    public string Nom;
    public string idTradName;
    [TextArea(5, 10)]
    public string Description;
    public string idTradDescription;
    public int IDSpell;
    public List<int> IDChildren;
    public SpellStatus SpellStatue;
    public bool IsAvailable;
    public int CostUnlock;
    public List<Cost> Costs;
    public List<Effet> ActionEffet;
    public List<BuffDebuff> ActionBuffDebuff;
    public Sprite Sprite;
    [Header("Audio")]
    [SerializeField]
    private AudioClip _spellSFX;
}
