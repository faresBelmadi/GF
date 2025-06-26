using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New spell", menuName = "Capacité/Create New Spell", order = 11)]
public class Spell : ScriptableObject, IBattleLogSpell
{
    [SerializeField]
    public TradId TitleId;

    [SerializeField, DefaultTextStyle(TradIdDefaultTextStyles.TextArea)]
    public TradId DescriptionId;

    public int IDSpell;
    public List<int> IDChildren;
    public SpellStatus SpellStatue;
    public bool IsAvailable;
    public int CostUnlock;
    public List<Cost> Costs;
    public List<Effet> ActionEffet;
    public List<BuffDebuff> ActionBuffDebuff;
    public Sprite Sprite;
    public Sprite RoundSprite;
    [Header("Audio")]
    [SerializeField]
    private AudioClip _spellSFX;

    public AudioClip SpellSFX => _spellSFX;

    public string TradName => TradManager.instance.GetTranslation(TitleId.Id);

    #region VERSIONING

    [SerializeField, HideInInspector]
    private int _dataVersion = 0;

    [SerializeField, HideInInspector, Obsolete("Use TitleId instead")]
    public string idTradName;
    [SerializeField, HideInInspector, Obsolete("Use TitleId instead")]
    public string Nom;

    [SerializeField, HideInInspector, Obsolete("Use DescriptionId instead")]
    public string idTradDescription;
    [SerializeField, HideInInspector, Obsolete("Use DescriptionId instead")]
    public string Description;

#pragma warning disable CS0618 // Warning for Obsolete field usage
    [ExecuteAlways]
    public void OnEnable()
    {
        if (_dataVersion < 1)
        {
            TitleId.Id = idTradName;
            TitleId.DefaultText = Nom;

            DescriptionId.Id = idTradDescription;
            DescriptionId.DefaultText = Description;

            _dataVersion = 1;
        }
    }
#pragma warning restore CS0618 // Warning for Obsolete field usage

    #endregion
}
