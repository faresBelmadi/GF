using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class BugReport : MonoBehaviour
{
    [Header("Google Form Information")]
    [SerializeField]
    private string _formURL;
    [SerializeField]
    private string _entryCategories;
    [SerializeField]
    private string _entryResponse;
    [Space]
    [Header("UI Elements")]
    [SerializeField]
    private TMP_Dropdown _category;
    //[SerializeField]
    //private TMP_Text _response;
    [SerializeField]
    private TMP_InputField _responseField;
    [Header("Translation keys")]
    [SerializeField]
    private string _idPlaceholder;
    [SerializeField]
    private string _idCatBug;
    [SerializeField]
    private string _idCatComment;
    [SerializeField]
    private string _idCatSuggest;
    [Space]
    [SerializeField]
    private UnityEvent _onReportScent;


    // Start is called before the first frame update
    void Start()
    {
        RefreshText();
    }
    private void OnEnable()
    {
        TradManager.OnRefreshTranslation += RefreshText;
        if (TradManager.instance != null)
            RefreshText();
    }
    private void OnDisable()
    {
        TradManager.OnRefreshTranslation -= RefreshText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void RefreshText()
    {
        string placeholderTranslation = TradManager.instance.GetTranslation(_idPlaceholder, "Type comment here");
        _responseField.placeholder.GetComponent<TMP_Text>().text = $"{placeholderTranslation}";
        _category.options[0].text = TradManager.instance.GetTranslation(_idCatBug, "Bug");
        _category.options[1].text = TradManager.instance.GetTranslation(_idCatComment, "Comment");
        _category.options[2].text = TradManager.instance.GetTranslation(_idCatSuggest, "Suggestion");


    }
    public void SendReportForm()
    {
        StartCoroutine(SendReportToGoogleForm());
    }
    private IEnumerator SendReportToGoogleForm()
    {
        //We create a form to send to the google form.
        WWWForm formToSend = new WWWForm();
        //We add the fields to the form, the field name is the one we get from the google form, and the value is the one we get from the user input.
        formToSend.AddField(_entryCategories, _category.options[_category.value].text);
        formToSend.AddField(_entryResponse, _responseField.text);
        _responseField.text = "";
        _category.value = 0;


        //send the request to the google form.
        using (UnityWebRequest webRequest = UnityWebRequest.Post(_formURL, formToSend))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error sending bug report: " + webRequest.error);
            }
            else
            {
                Debug.Log("Bug report sent successfully!");
            }
        }
        _onReportScent.Invoke();
    }
}
