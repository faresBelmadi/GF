using UnityEngine;

[CreateAssetMenu(fileName = "new RoomData", menuName = "Room/Create New RoomData")]
public class RoomData : ScriptableObject
{

    [SerializeField] private Sprite _spriteUNKNOWN;
    [SerializeField] private string _labelUNKNOWN;
    [Space]
    [SerializeField] private Sprite _spriteSTART;
    [SerializeField] private string _labelSTART;
    [Space]
    [SerializeField] private Sprite _spriteLOOT;
    [SerializeField] private string _labelLOOT;
    [Space]
    [SerializeField] private Sprite _spriteAUTEL;
    [SerializeField] private string _labelAUTEL;
    [Space]
    [SerializeField] private Sprite _spriteRANDOM;
    [SerializeField] private string _labelRANDOM;
    [Space]
    [SerializeField] private Sprite _spriteCLASSRANDOM;
    [SerializeField] private string _labelCLASSRANDOM;
    [Space]
    [SerializeField] private Sprite _spriteENCOUNTER;
    [SerializeField] private string _labelENCOUNTER;
    [Space]
    [SerializeField] private Sprite _spriteCLASSENCOUNTER;
    [SerializeField] private string _labelCLASSENCOUNTER;
    [Space]
    [SerializeField] private Sprite _spriteELITE;
    [SerializeField] private string _labelELITE;
    [Space]
    [SerializeField] private Sprite _spriteCLASSELITE;
    [SerializeField] private string _labelCLASSELITE;
    [Space]
    [SerializeField] private Sprite _spriteBOSS;
    [SerializeField] private string _labelBOSS;
    [Space]
    [SerializeField] private Sprite _spriteEXIT;
    [SerializeField] private string _labelEXIT;


    public Sprite spriteUnkown { get => _spriteUNKNOWN; }
    public Sprite spriteStart { get => _spriteSTART; }
    public Sprite spriteLoot { get => _spriteLOOT; }
    public Sprite spriteAutel { get => _spriteAUTEL; }
    public Sprite spriteRandom { get => _spriteRANDOM; }
    public Sprite spriteClassRandom { get => _spriteCLASSRANDOM; }
    public Sprite spriteEncounter { get => _spriteENCOUNTER; }
    public Sprite spriteClassEncounter { get => _spriteCLASSENCOUNTER; }
    public Sprite spriteElite { get => _spriteELITE; }
    public Sprite spriteClassElite { get => _spriteCLASSELITE; }
    public Sprite spriteBoss { get => _spriteBOSS; }
    public Sprite spriteExit { get => _spriteEXIT; }


    public string labelUnkown { get => _labelUNKNOWN; }
    public string labelStart { get => _labelSTART; }
    public string labelLoot { get => _labelLOOT; }
    public string labelAutel { get => _labelAUTEL; }
    public string labelRandom { get => _labelRANDOM; }
    public string labelClassRandom { get => _labelCLASSRANDOM; }
    public string labelEncounter { get => _labelENCOUNTER; }
    public string labelClassEncounter { get => _labelCLASSENCOUNTER; }
    public string labelElite { get => _labelELITE; }
    public string labelClassElite { get => _labelCLASSELITE; }
    public string labelBoss { get => _labelBOSS; }
    public string labelExit { get => _labelEXIT; }

}   
