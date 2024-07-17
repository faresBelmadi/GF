using System;
using UnityEngine;

[Serializable]
public class Music
{
    [SerializeField]
    private MusicType _musicType;
    [SerializeField]
    private AudioClip _audioClip;

    public MusicType Type { get { return _musicType; } }
    public AudioClip Clip { get { return _audioClip; } }
}
