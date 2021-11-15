 using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    public PlayerStat stat;

    public List<BuffDebuff> debuffs = new List<BuffDebuff>();

    public List<GameObject> Spells;
    public Transform DamageSpawn;
    public Transform DebuffContainer;
    public GameObject DamagePrefab;
    public GameObject SoinPrefab;
    public GameObject SpellPrefab;
    public GameObject SpellsSpawn;
    public Button EndTurnButton;

    public Slider HP;
    public Slider Volonté;
    public Slider Tension;
    public Slider Conscience;

    public TextMeshProUGUI TensionText;
    public TextMeshProUGUI HpText;
    public TextMeshProUGUI VolontéText; 
    public TextMeshProUGUI ConscienceText; 
    public Action EndTurnBM;
    
    public Spell SelectedSpell;

    public AnimationControllerAttack AnimationController;

    bool DoTurn;

    public void EnervementTension()
    {
        Debug.Log("tension avant Enervement : " + stat.Tension);
        var t = (int)((stat.Tension/(stat.NbPalier*stat.ValeurPalier)) * stat.NbPalier);
        Debug.Log("Valeur de palier : " + t);
        if(t >= stat.NbPalier)
        t = stat.NbPalier;
        else
        t ++;

        stat.Tension = t*stat.ValeurPalier;
        Debug.Log("tension apres Enervement : " + stat.Tension);
    }

    public void ApaisementTension()
    {
        Debug.Log("tension avant apaisement : " + stat.Tension);
        
        var t = (int)((stat.Tension/(stat.NbPalier*stat.ValeurPalier)) * stat.NbPalier);
        Debug.Log("Valeur de palier : " + t);
        if(t <= 0)
        t = 0;
        else
        t --;

        stat.Tension = t*stat.ValeurPalier;
        Debug.Log("tension apres apaisement : " + stat.Tension);
    }

    public void TakeDamage(int dmg, Source sourceDamage)
    {
        if(Source.Soin != sourceDamage)
        {
        //     if(debuffs.Where(c => (c.Type == BuffType.Def ||c.Type == BuffType.DefBrut)).Count() > 0)
        //     {
        //         foreach (var item in debuffs.Where(c => (c.Type == BuffType.Def ||c.Type == BuffType.DefBrut)&& c.ValeurEffet < 0 ))
        //         {
        //             dmg += Mathf.FloorToInt((item.ValeurEffet/100f)*dmg); 
        //         }
        //     }
        }
        stat.HP -= dmg;

        if(dmg > 0)
        {
            var temp = Instantiate(DamagePrefab,DamageSpawn);
            temp.GetComponent<TextAnimDegats>().Value = dmg;
        }
        else
        {
            var temp = Instantiate(SoinPrefab,DamageSpawn);
            temp.GetComponent<TextAnimDegats>().Value = dmg;
        }

        if(stat.HP <= 0)
        {
            stat.HP = 0;
            Dead();
        }        
        if(stat.HP > stat.MaxHP)
        {
            stat.HP = stat.MaxHP;
        }

        ReceiveTension(sourceDamage);

        updateUI();
        
    }

    public void updateUI()
    {
        HP.value = stat.HP;
        HP.maxValue = stat.MaxHP;
        Tension.value = Mathf.FloorToInt((stat.Tension*stat.NbPalier)/stat.TensionMax);
        Tension.maxValue = stat.NbPalier;
        Volonté.value = stat.Volonté;
        Volonté.maxValue = stat.MaxVolonté;
        Conscience.value = stat.Conscience;
        Conscience.maxValue = stat.MaximumConscience;

        HpText.text = stat.HP +"/" + stat.MaxHP;

        ConscienceText.text = stat.Conscience +"/" + stat.MaximumConscience;

    }
    public void ReceiveTension(Source sourceDamage)
    {
        switch (sourceDamage)
        {
            case Source.Attaque:
            stat.Tension += stat.TensionAttaque;
            break;
            case Source.Dot:
            stat.Tension += stat.TensionDot;
            break;
            case Source.Buff:
            stat.Tension += stat.TensionDebuff;
            break;
            case Source.Soin:
            stat.Tension += stat.TensionSoin;
            break;
        }
    }

    public void AddDebuff(BuffDebuff toAdd)
    {
        debuffs.Add(toAdd);
        // if(toAdd.ValeurEffet < 0 || toAdd.Debuff)
        //     ReceiveTension(Source.Buff);
    }

    public bool CanHaveAnotherTurn()
    {
        if(stat.Tension >= stat.ValeurPalier*stat.NbPalier)
        {
            stat.Tension = stat.ValeurPalier*stat.NbPalier;
            return true;
        }

        return false;
    }

    public void StartTurn()
    {
        DoTurn = true;
        DecompteDebuff();//Peut changer en fin de tour..............
        foreach (var item in Spells)
        {
            item.GetComponent<spellCombat>().isTurn = true;
        }
        EndTurnButton.interactable = true;
        stat.Volonté = stat.MaxVolonté;
        updateUI();
    }


    public void EndTurn()
    {
        DoTurn = false;
        DesactivateSpells();

        EndTurnBM();
    }

    public void DesactivateSpells()
    {
        foreach (var item in Spells)
        {
            item.GetComponent<spellCombat>().isTurn = false;
        }
        EndTurnButton.interactable = false;
    }

    private void DecompteDebuff()
    {
        for (int i = 0; i < debuffs.Count; i++)
        {

            if (debuffs[i].Decompte == EffetTypeDecompte.tour)
                debuffs[i].nbTemps--;
        }

        debuffs.RemoveAll(c => c.nbTemps <= 0);
    }

    public void StartPhase()
    {
        
        for (int i = 0; i < debuffs.Count; i++)
        {
            
            if(debuffs[i].Decompte == EffetTypeDecompte.round)
               debuffs[i].nbTemps--;
        }

        debuffs.RemoveAll(c => c.nbTemps <= 0);
    }

    public void StartUp()
    {
        Debug.Log(stat.AvailableSpell.Count());
        foreach (var item in stat.AvailableSpell)
        {
            var temp = Instantiate(SpellPrefab,SpellsSpawn.transform);
            temp.GetComponent<spellCombat>().Action = item;
            temp.GetComponent<spellCombat>().Act = DoAction;
            temp.GetComponent<spellCombat>().StartUp();

            Spells.Add(temp);
        }
    }

    private void DoAction(Spell toDo)
    {
        SelectedSpell = toDo;
        TakeTarget();
    }

    private void TakeTarget()
    {
        GameManager.instance.BattleMan.StartTargeting();
    }

    public void SendSpell()
    {
        Costs();
        AnimationController.StartAttack(AfterAnim);
        GameManager.instance.BattleMan.LaunchAnimAttacked();
    }

    void AfterAnim()
    {
        GameManager.instance.BattleMan.GetListEffectPlayer(SelectedSpell);
        
        updateUI();
    }

    public void Costs()
    {
        foreach(var price in SelectedSpell.Costs)
        {
            switch (price.typeCost)
            {
                case TypeCostSpell.Conscience:
                    stat.Conscience -= price.Value;
                break;
                case TypeCostSpell.Radiance:
                    stat.HP -= price.Value;
                break;
                case TypeCostSpell.Volonté:
                    stat.Volonté -= price.Value;
                break;
            }
        }
    }

    void Dead()
    {
        GameManager.instance.BattleMan.DeadPlayer();
    }
}
