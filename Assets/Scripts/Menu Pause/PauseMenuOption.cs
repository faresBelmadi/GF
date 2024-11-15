using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuOption : MonoBehaviour
{
    private enum OptionPanel
    {
        Audio,
        Video,
        Interface
    }

    [SerializeField]
    private GameObject _interfacePanel;
    [SerializeField]
    private GameObject _audioPanel;
    [SerializeField]
    private GameObject _videoPanel;
    [SerializeField]
    private Button _interfaceButton;
    [SerializeField]
    private Button _audioButton;
    [SerializeField]
    private Button _videoButton;

    private Color _unselectedColor = new Color(1, 1, 1, .5f);
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        ShowInterfacePanel();
    }
    private void ShowPanel(OptionPanel panelToShow)
    {
        switch (panelToShow)
        {
            case OptionPanel.Interface:
                _interfacePanel.SetActive(true);
                _audioPanel.SetActive(false);
                _videoPanel.SetActive(false);
                _interfaceButton.GetComponent<Image>().color = Color.white;
                _audioButton.GetComponent<Image>().color = _unselectedColor;
                _videoButton.GetComponent<Image>().color = _unselectedColor;
                break;
            case OptionPanel.Audio:
                _interfacePanel.SetActive(false);
                _audioPanel.SetActive(true);
                _videoPanel.SetActive(false);
                _interfaceButton.GetComponent<Image>().color = _unselectedColor;
                _audioButton.GetComponent<Image>().color = Color.white;
                _videoButton.GetComponent<Image>().color = _unselectedColor;
                break;
            case OptionPanel.Video:
                _interfacePanel.SetActive(false);
                _audioPanel.SetActive(false);
                _videoPanel.SetActive(true);
                _interfaceButton.GetComponent<Image>().color = _unselectedColor;
                _audioButton.GetComponent<Image>().color = _unselectedColor;
                _videoButton.GetComponent<Image>().color = Color.white;
                break;
           
        }
    }
    public void ShowAudioPanel()
    {
        ShowPanel(OptionPanel.Audio);
    }
    public void ShowVideoPanel()
    {
        ShowPanel(OptionPanel.Video);
    }
    public void ShowInterfacePanel()
    {
        ShowPanel(OptionPanel.Interface);
    }
}
