using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new SpriteData", menuName = "SpriteData/Create New SpriteData")]
public class SpriteData : ScriptableObject
{
    [SerializeField]
    private Sprite _buffSprite;
    [SerializeField]
    private Sprite _debuffSprite;

    public Sprite Buff => _buffSprite;
    public Sprite Debuff => _debuffSprite;
}
