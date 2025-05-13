using UnityEngine;
using UnityEngine.UI;

public class AutoScroll : MonoBehaviour
{
    [SerializeField]
    private ScrollRect _scrollView;
    [SerializeField]
    private float _timeScroll;
    [SerializeField]
    private float _waitingTime;

    private float _currentTime = 0;
    private bool _isScrolling = false;

    // Start is called before the first frame update
    void Start()
    {
        _isScrolling = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isScrolling)
        {
            _scrollView.normalizedPosition = Vector2.Lerp(new Vector2(0, 0), new Vector2(0, 1), _currentTime / _timeScroll);
            _currentTime += Time.deltaTime;
            if (_currentTime >= _timeScroll)
            {
                _currentTime = 0;
                _isScrolling = false;               // Pause dans le scrolling
            }
        }
        else
        {
            _currentTime += Time.deltaTime;
            if (_currentTime >= _timeScroll)
            {
                _currentTime = 0;
                _isScrolling = true;                // On reprend le scrolling depuis le haut
                _scrollView.normalizedPosition = new Vector2(0, 0);
            }
        }
    }
}