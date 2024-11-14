using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private Animator _pauseMenuAnimator;
    [SerializeField]
    private List<GameObject> _glossaryPanels;
    [SerializeField]
    private GameObject _glossaryHolder;
    [SerializeField]
    private TMP_Text _glossaryPageText;
    [SerializeField]
    private Button _nextButton;
    [SerializeField]
    private Button _prevButton;

    private bool _isPaused = false;

    private int _pageShowed = 0;
    // Start is called before the first frame update
    void Start()
    {
        HideGlossary();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused)
                Unpause();
            else
                Pause();
        }
    }
    public void Pause()
    {
        HideGlossary();
        _pageShowed = 0;
        _pauseMenuAnimator.SetTrigger("Show");
         Time.timeScale = 0f;
        _isPaused = true;
    }
    public void Unpause()
    {
        HideGlossary();
        _pauseMenuAnimator.SetTrigger("Hide");
        Time.timeScale = 1f;
        _isPaused = false;
    }
    public void ShowGlossary()
    {
        _glossaryHolder.SetActive(true);
        ShowSelectedGlossaryPage(_pageShowed);
    }
    public void HideGlossary()
    {
        for (int i = 0; i < _glossaryPanels.Count; i++)
        {
            _glossaryPanels[i].SetActive(false);
        }
        _glossaryHolder.SetActive(false);
    }
    private void ShowSelectedGlossaryPage(int page)
    {
        _pageShowed = page;
        for (int i=0; i<_glossaryPanels.Count; i++)
        {
            _glossaryPanels[i].SetActive(false);
        }

        _glossaryPanels[_pageShowed].SetActive(true);
        _glossaryPageText.text = $"{_pageShowed + 1} / {_glossaryPanels.Count}";
        _nextButton.gameObject.SetActive(_pageShowed == _glossaryPanels.Count - 1 ? false : true);
        _prevButton.gameObject.SetActive(_pageShowed == 0 ? false : true);
    }
    public void NextPage()
    {
        ShowSelectedGlossaryPage(_pageShowed + 1);
    }
    public void PreviousPage()
    {
        ShowSelectedGlossaryPage(_pageShowed - 1);
    }
}
