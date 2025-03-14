using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakComponent : MonoBehaviour
{
    [SerializeField]
    private GameObject _talkingBubble;
    [SerializeField]
    private GameObject _emotionBubble;

    private SpriteRenderer _emotionSpriteRenderer;
    // Start is called before the first frame update
    void Awake()
    {
        _emotionBubble.SetActive(false);
        _talkingBubble.SetActive(false);

        _emotionSpriteRenderer = _emotionBubble.GetComponent<SpriteRenderer>();
    }

    public void ShowTalking()
    {
        _emotionBubble.SetActive(false);
        _talkingBubble.SetActive(true);
    }
    public void HideTalking()
    {
        _emotionBubble.SetActive(false);
        _talkingBubble.SetActive(false);
    }
    public void ShowEmotion(Sprite spriteToShow)
    {
        _talkingBubble.SetActive(false);
        _emotionSpriteRenderer.sprite = spriteToShow;
        _emotionBubble.SetActive(true);
    }
    public void HideEmotion()
    {
        _talkingBubble.SetActive(false);
        _emotionBubble.SetActive(false);
    }
}
