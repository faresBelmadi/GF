using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New dialogue", menuName = "Remake/Capacité/Create New Spell", order = 11)]
public class SpellRemake : ScriptableObject
{
    public string Nom;
    public string Description;
    public int IDSpell;
    public List<int> IDChildren;
    public SpellStatusRemake SpellStatue;
    public bool IsAvailable;
    public int CostUnlock;
    public List<CostRemake> Costs;
    public List<EffetRemake> ActionEffet;
    public List<BuffDebuffRemake> ActionBuffDebuff;
    public Sprite Sprite;
}
