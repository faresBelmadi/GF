using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class CrystalSoul : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _crystals;
    [SerializeField]
    private float _movingDuration = 1f;
    [SerializeField]
    private TMP_Text _textAmount;

    private Animator _animator;
    private bool _isEndingEssence = false;
    private int _movedCrystal = 0;
    public int Amount { get; set; }
    public int Heal { get => (_isEndingEssence) ? Amount : Mathf.FloorToInt(Amount / 2); }


    private void OnEnable()
    {
        BattleManager.OnGatherEssence += GatherCrystal;
    }
    private void OnDisable()
    {
        BattleManager.OnGatherEssence -= GatherCrystal;
    }

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _textAmount.gameObject.SetActive(false);
    }

    public void AddAmountOfEssence(int amount, bool isEndingCrystal = false)
    {
        Amount = amount;
        _isEndingEssence = isEndingCrystal;
        Debug.Log("Create Crystal Soul with value of " + amount + "ending = " + isEndingCrystal);
        _textAmount.text = Heal.ToString();
    }

    public void ConsumeEssence()
    {
        StopPreviewOnHP();
        if (_isEndingEssence)
            GameManager.Instance.BattleMan.ConsumeEndBattle(Amount);
        else
            GameManager.Instance.BattleMan.Consume(Heal);


        GameManager.Instance.BattleMan.ListEssence.Remove(this.gameObject);
        Destroy(this.gameObject);

    }

    public void ShowPreviewOnHP()
    {
        if (GameManager.Instance != null)
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
    private void GatherCrystal(Transform targetPosition)
    {
        _animator.enabled = false;

        for (int i = 0; i < _crystals.Count; i++)
        {
            StartCoroutine(MoveCrystal(i, Random.Range(0f, 0.5f), targetPosition));
        }
    }

    private IEnumerator MoveCrystal(int idCrystal, float waitingTime, Transform destination)
    {

        float timer = 0;
        yield return new WaitForSeconds(waitingTime);
        Vector3 inititialPos = _crystals[idCrystal].transform.position;
        while (timer < _movingDuration)
        {
            _crystals[idCrystal].transform.position = Vector3.Lerp(inititialPos, destination.position, timer / _movingDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        _crystals[idCrystal].transform.position = destination.position;
        _movedCrystal++;
        if (_movedCrystal >= _crystals.Count)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnMouseEnter()
    {
        ShowPreviewOnHP();
        _textAmount.gameObject.SetActive(true);
    }
    private void OnMouseExit()
    {
        StopPreviewOnHP();
        _textAmount.gameObject.SetActive(false);
    }
    private void OnMouseDown()
    {
        ConsumeEssence();
    }
}
