using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour
{
    [Header("Text")]
    [SerializeField]
    private Image _textImage;
    [SerializeField]
    private Sprite _englishTextImage;
    [SerializeField]
    private Sprite _frenchTextImage;
    [Space, Header("Buttons")]
    [SerializeField] private Button _steam;
    [SerializeField] private Button _linktree;
    [SerializeField] private Button _continue;
    [SerializeField] private Button _kickstarter;
    [SerializeField] private Button _discord;
    [Space, Header("Buttons Image")]
    [Header("English")]
    [SerializeField] private GameObject _englishButtons;
    [SerializeField] private Sprite _engDiscord;
    [SerializeField] private Sprite _engSteam;
    [SerializeField] private Sprite _englinktree;
    [SerializeField] private Sprite _engContinue;
    [SerializeField] private Sprite _engKickstarter;
    [Header("French")]
    [SerializeField] private GameObject _frenchButtons;
    [SerializeField] private Sprite _frenchDiscord;
    [SerializeField] private Sprite _frenchSteam;
    [SerializeField] private Sprite _frenchlinktree;
    [SerializeField] private Sprite _frenchContinue;
    [SerializeField] private Sprite _frenchKickstarter;

    private void OnEnable()
    {
        switch (TradManager.instance.Language)
        {
            case TradManager.SUPPORTEDLANGUAGES.FR:
                _englishButtons.SetActive(false);
                _frenchButtons.SetActive(true);
                _textImage.sprite = _frenchTextImage;
                //_steam.image.sprite = _frenchSteam;
                //_linktree.image.sprite = _frenchlinktree;
                //_discord.image.sprite = _frenchDiscord;
                _continue.image.sprite = _frenchContinue;
                _kickstarter.image.sprite = _frenchKickstarter;
                break;
            default:
                _englishButtons.SetActive(true);
                _frenchButtons.SetActive(false);
                _textImage.sprite = _englishTextImage;
                //_steam.image.sprite = _engSteam;
                //_linktree.image.sprite = _englinktree;
                //_discord.image.sprite = _engDiscord;
                _continue.image.sprite = _engContinue;
                _kickstarter.image.sprite = _engKickstarter;
                break;
        }
    }

   
}
