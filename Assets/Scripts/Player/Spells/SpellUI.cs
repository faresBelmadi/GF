using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class SpellUI : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public List<GameObject> Lines = new List<GameObject>();

    public Spell LinkedSpell;

    public TextMeshProUGUI buttonText;


    public void FinishSetUp()
    {
        ResetLine();
        UpdateButton();

        buttonText.text = LinkedSpell.Nom;
    }

    public void UpdateVisual() {
        ResetLine();
        UpdateButton();
    }

    public void ButtonPressed()
    {
        if(GameManager.instance.playerStat.Essence >= LinkedSpell.CostUnlock)
        {
            LinkedSpell.SpellStatue = SpellStatus.bought;
            GameManager.instance.SkillTreeUI.UpdateSkillTree(this);
            UpdateVisual();
        }
    }

    private void ResetLine()
    {
        foreach (var item in Lines)
        {
            switch (LinkedSpell.SpellStatue)
            {
                case SpellStatus.unlocked:
                    item.GetComponent<UILineRenderer>().color = Color.blue;  
                    break;
                case SpellStatus.locked:
                    item.GetComponent<UILineRenderer>().color = Color.red;  
                    break;
                case SpellStatus.bought:
                    item.GetComponent<UILineRenderer>().color = Color.green;  
                    break;
            }
        }
        
    }    
    private void UpdateButton()
    {
        if(LinkedSpell.SpellStatue == SpellStatus.unlocked)
            this.GetComponent<Button>().interactable = true;
        else
            this.GetComponent<Button>().interactable = false;
            
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.instance.SkillTreeUI.ShowInspectorOver(LinkedSpell);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.instance.SkillTreeUI.HideInspector();
    }
}
