using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = " new SouvenirListData", menuName = "Souvenir/Create New SouvenirListData")]
public class SouvenirListData : ScriptableObject
{
    [field: SerializeField] public List<Souvenir> AllSouvenir { get; private set; }
}
