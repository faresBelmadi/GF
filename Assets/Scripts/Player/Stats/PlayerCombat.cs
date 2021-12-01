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

    public int LastDamageTaken;

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
            foreach (var item in debuffs)
            {
                foreach (var effect in item.effects)
                {
                    if(effect.type == BuffType.Def)
                    {
                        var tempResi = 1 - (effect.pourcentageEffet/100f);

                        dmg += dmg - Mathf.RoundToInt(tempResi *dmg); 
                    }
                    if(effect.type == BuffType.DefBrut)
                    {
                        dmg -= effect.pourcentageEffet;
                    }
                    if(effect.type == BuffType.EpineForceAme)
                    {
                        var dmgRetour = Mathf.RoundToInt((20 * stat.Dmg)/100f);
                        GameManager.instance.BattleMan.EnemyScripts.First(c => c.combatID == GameManager.instance.BattleMan.currentIdTurn).TakeDamage(dmgRetour,Source.Dot);
                    }
                }
            }
        }

        if(stat.Resilience != 0)
        {
            var tempResi = 1 - ((stat.Resilience*3)/100f);
            dmg = Mathf.RoundToInt(tempResi * dmg);
        }

        stat.HP -= dmg;

        LastDamageTaken = dmg;

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
        if(toAdd.IsDebuff)
            ReceiveTension(Source.Buff);
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
        foreach (var item in debuffs)
        {
            if (item.Decompte == EffetTypeDecompte.tour)
                item.nbTemps--;
        }

        debuffs.RemoveAll(c => c.nbTemps <= 0);
        ResetStat();

        foreach (var item in debuffs)
        {
            foreach (var effect in item.effects)
            {
                if(effect.type == BuffType.DmgPVMax)
                {
                    stat.HP -= Mathf.RoundToInt((effect.pourcentageEffet * stat.MaxHP) / 100);
                }
                if(effect.type == BuffType.DégatsBrut)
                {
                    stat.HP -= effect.pourcentageEffet;
                }
                if(effect.type == BuffType.Résilience)
                {
                    stat.Resilience += effect.pourcentageEffet;
                }
                if(effect.type == BuffType.VitesseBrut)
                {
                    stat.Speed += effect.pourcentageEffet;
                }
                if(effect.type == BuffType.Vitesse)
                {
                    stat.Speed += Mathf.RoundToInt((effect.pourcentageEffet * stat.Speed) / 100f);
                }
                if(effect.type == BuffType.Clairvoyance)
                {
                    stat.Clairvoyance += effect.pourcentageEffet;
                }
                if(effect.type == BuffType.PVMax)
                {
                    stat.MaxHP += Mathf.RoundToInt((effect.pourcentageEffet * stat.MaxHP) / 100f);
                }
                
            }
        }


    }
    public void ResetStat()
    {
        stat.MaxHP = stat.MaxHPOriginal;
        stat.Speed = stat.SpeedOriginal;
        stat.Clairvoyance = stat.ClairvoyanceOriginal;
        stat.Resilience = stat.ResilienceOriginal;
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
