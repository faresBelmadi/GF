using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new MusicData", menuName ="Audio/MusicData")]
public class MusicData : ScriptableObject
{
    [SerializeField]
    private List<Music> _musics;

    public AudioClip Clip (MusicType type)
    {
        for (int i=0; i< _musics.Count; i++)
        {
            if (_musics[i].Type == type)
            {
                return _musics[i].Clip;
            }
        }
        return null;
    }
}
