using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    public enum SelectedCharacter
    {
        None,
        Warrior,
        Witch
    }
    [Header("Warior")]
    [SerializeField]
    private GameObject _warriorGO;
    [SerializeField]
    private GameObject _warriorCristopher;
    [Header("Sorciere")]
    [SerializeField]
    private GameObject _witchGO;
    [SerializeField]
    private GameObject _witchCristopher;
    [SerializeField]
    private RotateObject _portal;

    private SelectedCharacter _selected;
    // Start is called before the first frame update
    void Start()
    {
        _selected= SelectedCharacter.None;
        _warriorGO.SetActive(false);
        _witchGO.SetActive(false);
        _warriorCristopher.SetActive(true);
        _witchCristopher.SetActive(true);
    }

    public void ShowWitch()
    {
        _witchGO.SetActive(true);
    }
    public void ShowWarrior()
    {
        _warriorGO.SetActive(true);
    }
    public void HideWitch()
    {
        if (_selected != SelectedCharacter.Witch)
        {
            _witchGO.SetActive(false);
        }
    }
    public void HideWarrior()
    {
        if (_selected != SelectedCharacter.Warrior)
        {
            _warriorGO.SetActive(false);
        }
    }
    public void SelectWarrior()
    {
        _selected = SelectedCharacter.Warrior;
        _warriorCristopher.SetActive(false);
        _portal.StartRotate();
    }
    public void SelectWitch()
    {
        _selected = SelectedCharacter.Witch;
        _witchCristopher.SetActive(false);
        _portal.StartRotate();
    }
}
