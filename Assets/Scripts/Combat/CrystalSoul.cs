using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class CrystalSoul : MonoBehaviour
{
    private Animator _animator;
    private bool _isEndingEssence = false;
    public int Amount { get; set; }
    public int Heal { get => (_isEndingEssence) ? Amount : Mathf.FloorToInt(Amount / 2); }
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }


    public void AddAmountOfEssence(int amount, bool isEndingCrystal = false)
    {
        Amount = amount;
        _isEndingEssence = isEndingCrystal;
    }

    public void ConsumeEssence()
    {
        StopPreviewOnHP();
        if (_isEndingEssence)
            GameManager.Instance.BattleMan.ConsumeEndBattle(Amount);
        else
            GameManager.Instance.BattleMan.Consume(Mathf.FloorToInt(Amount / 2));

        if (GameManager.Instance.BattleMan != null)
        {
            GameManager.Instance.BattleMan.ListEssence.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    public void ShowPreviewOnHP()
    {
        if (GameManager.Instance!= null)
        {
            GameManager.Instance.BattleMan.player.PreviewHPBarUpdate(
                GameManager.Instance.BattleMan.player.Stat.Radiance + Heal,
                GameManager.Instance.BattleMan.player.Stat.RadianceMax);

            GameManager.Instance.BattleMan.player.PreviewTensionBarUpddate();
        }
       
    }
    public void StopPreviewOnHP()
    {
        if (GameManager.Instance != null && GameManager.Instance.BattleMan != null)
        {
            GameManager.Instance.BattleMan.player.StopPReviewHPBarUpdate();
            GameManager.Instance.BattleMan.player.StopPreviewTensionBar();
        }
    }
}
