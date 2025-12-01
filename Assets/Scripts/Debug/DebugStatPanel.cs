using TMPro;
using UnityEngine;

public class DebugStatPanel : MonoBehaviour
{
    [SerializeField] private GameObject _debugPanel;
    [SerializeField] private TMP_Text _faValueText;
    [SerializeField] private TMP_Text _radValueText;
    [SerializeField] private TMP_Text _resValueText;
    [SerializeField] private TMP_Text _vitValueText;
    [SerializeField] private TMP_Text _convicValueText;
    [SerializeField] private TMP_Text _calmeValueText;
    [SerializeField] private TMP_Text _conscienceValueText;
    [SerializeField] private TMP_Text _conscienceMaxValueText;
    [SerializeField] private TMP_Text _clairvoyanceValueText;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.playerStatHandler != null)
        {
            _faValueText.text = GameManager.Instance.playerStatHandler.ForceDameTotal.ToString();
            _radValueText.text = GameManager.Instance.playerStatHandler.RadianceMaxTotal.ToString();
            _resValueText.text = GameManager.Instance.playerStatHandler.ResilienceTotal.ToString();
            _vitValueText.text = GameManager.Instance.playerStatHandler.VitesseTotal.ToString();
            _convicValueText.text = GameManager.Instance.playerStatHandler.ConvictionTotal.ToString();
            _calmeValueText.text = GameManager.Instance.playerStatHandler.CalmeTotal.ToString();
            _conscienceValueText.text = GameManager.Instance.playerStatHandler.Conscience.ToString();
            _conscienceMaxValueText.text = GameManager.Instance.playerStatHandler.ConscienceMaxTotal.ToString();
            _clairvoyanceValueText.text = GameManager.Instance.playerStatHandler.ClairvoyanceTotal.ToString();
        }
    }

    public void TogglePanel()
    {
        _debugPanel.SetActive(!_debugPanel.activeSelf);
    }
    
}