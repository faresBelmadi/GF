using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Spell", menuName = "Capacité/Create New Enemy Spell", order = 1)]
public class EnnemiSpell : ScriptableObject, IBattleLogSpell
{
    public string Name;
    [SerializeField]
    private string _idTradName;
    public int ID;
    public bool IsAttaque;
    public Sprite ImageIntentionSpell;

    [Tooltip("Poids de départ du sort, plus il est bas, plus le sort aura de chance d'etre lancé en premier")]
    [Min(10)]
    public int Weight = 10;

    [Tooltip("Poids ajouté au sort, plus il est bas, plus le sort aura de chance d'etre lancé")]
    public int AddedWeight;

    public List<Effet> Effet;
    public List<BuffDebuff> debuffsBuffs;

    [Space]
    [Header("SFX")]
    [SerializeField]
    private AudioClip _spellSFX;

    public AudioClip SpellSFX;

    public string TradName => TradManager.instance.GetTranslation(_idTradName, Name);
}
