using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New dialogue", menuName = "Remake/Character/Create New Ennemi Class or Boss", order = 11)]
public class EnnemiClassBoss : EnnemiStat
{
    public List<Passif> ListPassif;
}
