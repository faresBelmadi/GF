using UnityEngine;

[CreateAssetMenu(fileName = "new RoomData", menuName = "Room/Create New RoomData")]
public class RoomData : ScriptableObject
{

    [SerializeField] private Sprite _salleCombatBoss;
    [SerializeField] private string _salleCombatBossLabel;
    [Space]
    [SerializeField] private Sprite _salleCombatElite;
    [SerializeField] private string _salleCombatEliteLabel;
    [Space]
    [SerializeField] private Sprite _salleCombatNormal;
    [SerializeField] private string _salleCombatNormalLabel;
    [Space]
    [SerializeField] private Sprite _salleLevelUp;
    [SerializeField] private string _salleLevelUpLabel;
    [Space]
    [SerializeField] private Sprite _salleHeal;
    [SerializeField] private string _salleHealLabel;
    [Space]
    [SerializeField] private Sprite _salleAlea;
    [SerializeField] private string _salleAleaLabel;
    [Space]
    [SerializeField] private Sprite _salleStart;
    [SerializeField] private string _salleStartLabel;
    [Space]
    [SerializeField] private Sprite _salleEnd;
    [SerializeField] private string _salleEndLabel;


    public Sprite SalleCombatBoss { get => _salleCombatBoss; }
    public Sprite SalleCombatElite { get => _salleCombatElite; }
    public Sprite SalleCombatNormal { get => _salleCombatNormal; }
    public Sprite SalleLevelUp { get => _salleLevelUp; }
    public Sprite SalleHeal { get => _salleHeal; }
    public Sprite SalleAlea { get => _salleAlea; }
    public Sprite SalleStart { get => _salleStart; }
    public Sprite SalleEnd { get => _salleEnd; }

    public string SalleCombatBossLabel { get => _salleCombatBossLabel; }
    public string SalleCombatEliteLabel { get => _salleCombatEliteLabel; }
    public string SalleCombatNormalLabel { get => _salleCombatNormalLabel; }
    public string SalleLevelUpLabel { get => _salleLevelUpLabel; }
    public string SalleHealLabel { get => _salleHealLabel; }
    public string SalleAleaLabel { get => _salleAleaLabel; }
    public string SalleStartLabel { get => _salleStartLabel; }
    public string SalleEndLabel { get => _salleEndLabel; }
}   
