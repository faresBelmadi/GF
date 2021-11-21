using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum nextActionEnum
{
    Attaque,
    Attaque2,
    Buff,
    Debuff
}


[RequireComponent(typeof(EnemyCombatGen))]
[RequireComponent(typeof(Animator))]
public class EnemyCombatBehaviour : MonoBehaviour
{
    public Enemy current;
    public EnemyCombatGen UICombat;

    List<BuffDebuff> Debuffs = new List<BuffDebuff>();
    public int TensionUI;
    public float currentTension;
    public float TensionPalier;
    public float TensionMax;

    public Action EndTurnBM;
    public Action<List<ActionResult>,int,int,bool,bool> actResult;
    public Action<List<BuffDebuff>,int,int> actDebuff;

    public int combatID;
    public EnemySpell nextAction;
    public GameObject EssencePrefab;
    nextActionEnum nextActionType;
    List<EnemySpell> Spells;

    public void SetUp()
    {
        updateUI();

        var animator = this.GetComponent<Animator>();

        string[] name = current.name.Split('(');
        Debug.Log(name[0]);
        animator.SetLayerWeight(animator.GetLayerIndex(name[0]),1);

        if(current.Att1 != null)
            current.Att1 = Instantiate(current.Att1);
        if(current.Att2 != null)
            current.Att2 =  Instantiate(current.Att2);
        if(current.Buff != null)
            current.Buff =  Instantiate(current.Buff);
        if(current.Debuff != null)
            current.Debuff =  Instantiate(current.Debuff);
    }

    public void ReceiveTension(Source sourceDamage)
    {
        
        switch (sourceDamage)
        {
            case Source.Attaque:
            currentTension += current.GainAttaque;
            break;
            case Source.Dot:
            currentTension += current.GainDot;
            break;
            case Source.Buff:
            currentTension += current.GainDebuff;
            break;
            case Source.Soin:
            currentTension += current.GainHeal;
            break;
        }
        if(currentTension >= TensionPalier*current.NbPalier)
            currentTension = TensionPalier*current.NbPalier + 0.2f;
    }

    public void TakeDamage(int dmg,Source sourceDamage)
    {   
        foreach (var item in Debuffs)
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
                
            }
        }
        //résilience
        if(current.résilience != 0)
        {
            var tempResi = 1 - ((current.résilience*3)/100f);
            dmg = Mathf.RoundToInt(tempResi * dmg);
        }


        current.HP -= dmg;
        if(current.HP <=0)
        {  
            Dead();
        }
        else
            UICombat.SpawnDegatSoin(dmg);
            
        ReceiveTension(sourceDamage);
    }

    private void Dead()
    {
    foreach (var item in Debuffs)
        {
            foreach (var effect in item.effects)
            {
            
                if(effect.type == BuffType.DeathTrigger)
                {
                    //to do : change name to official debuff name for debuff intouchable
                    if(item.NomDebuff == "Débuff intouchable")
                    {
                        current.EssenceDrop = 0;
                    }
                }
            }
        }
        var t = Instantiate(EssencePrefab,this.transform.parent);
        if(current.EssenceDrop != 0)
        {
            t.GetComponent<Essence>().AddEssence(current.EssenceDrop);
            GameManager.instance.BattleMan.ListEssence.Add(t);
        }
        GameManager.instance.BattleMan.DeadEnemy(combatID);
    }

    public void AddDebuff(BuffDebuff toAdd)
    {
        Debuffs.Add(toAdd);
        if(toAdd.IsDebuff)
            ReceiveTension(Source.Buff);
    }
    private void Update() {
        updateUI();
    }

    void updateUI()
    {
        TensionUI = Mathf.FloorToInt((currentTension * current.NbPalier) / TensionMax); 
        UICombat.updateHp(current.HP,current.MaxHP);
        UICombat.updateTension(TensionUI,current.NbPalier);
        string[] t = current.name.Split('(');
        UICombat.updateNom(t[0]);
        UICombat.RaiseEvent = TargetAcquired;
    }

    public bool CanHaveAnotherTurn()
    {
        if(currentTension >= TensionPalier*current.NbPalier)
        {
            return true;

        }
        return false;
    }

    public void StartTurn()
    {
        DecompteDebuff();
        DoAction();
    }

    public void EndAnim()
    {
        EndTurn();
    }

    public void EndAnimHurt()
    {
        this.GetComponent<Animator>().SetBool("IsAttacked",false);
    }
    private void DoAction()
    {
        List<ActionResult> temp = new List<ActionResult>();
        bool enerv,apais;
        GetResult(temp,out enerv,out apais);
        actDebuff(nextAction.debuffsBuffs,combatID,-1);
        actResult(temp,combatID,-1,enerv,apais);
        LaunchAnimBool();
    }

    public void GetResult(List<ActionResult> temp,out bool enervement, out bool apaisement)
    {
        enervement = false;
        apaisement = false;
        var Fa = current.ForceAme;

        foreach (var item in Debuffs)
        {
            foreach (var effect in item.effects)
            {
            
                if(effect.type == BuffType.AttBrut)
                {
                    Fa += effect.pourcentageEffet;
                }
                if(effect.type == BuffType.Att)
                {
                    
                    var tempResi = 1 - (effect.pourcentageEffet/100f);

                    Fa += Fa - Mathf.RoundToInt(tempResi * Fa); 
                }
                if(effect.type == BuffType.AttUpPVMiss)
                {
                    if(item.target == Cible.ennemi)
                    {
                        var pvMiss = (100 - ((GameManager.instance.BattleMan.player.stat.HP * 100) / GameManager.instance.BattleMan.player.stat.MaxHP));
                        Fa += Fa - Mathf.RoundToInt((pvMiss * ((2*Fa)/100) + Fa));
                    }
                }
            }
        }


        foreach (var item in nextAction.Effet)
        {
            int tempHp = 0;
            int nbattack = 0;
            item.DoAction(Fa,out tempHp, out nbattack);
            temp.Add(new ActionResult(){HpModif = tempHp, target=item.target,nbAttaque = nbattack});
        }
        if(nextAction.Effet.Any(c => c.type == BuffType.Enervement))
            enervement = true;
        else if(nextAction.Effet.Any(c => c.type == BuffType.Apaisement))
            apaisement = true;

    }

    void LaunchAnimBool()
    {
        switch(nextActionType)
        {
            case nextActionEnum.Attaque:
                this.GetComponent<Animator>().SetBool("LaunchAttaque",true);
                break;
            case nextActionEnum.Attaque2:
                this.GetComponent<Animator>().SetBool("LaunchAttaque2",true);
                break;
            case nextActionEnum.Buff:
                this.GetComponent<Animator>().SetBool("LaunchBuff",true);
                break;
            case nextActionEnum.Debuff:
                this.GetComponent<Animator>().SetBool("LaunchDebuff",true);
                break;
            default:
                break;
        }

    }

    public void getAttacked()
    {
        this.GetComponent<Animator>().SetBool("IsAttacked",true);
    }
    void EndAnimBool()
    {        
        switch(nextActionType)
        {
            case nextActionEnum.Attaque:
                this.GetComponent<Animator>().SetBool("LaunchAttaque",false);
                break;
            case nextActionEnum.Attaque2:
                this.GetComponent<Animator>().SetBool("LaunchAttaque2",false);
                break;
            case nextActionEnum.Buff:
                this.GetComponent<Animator>().SetBool("LaunchBuff",false);
                break;
            case nextActionEnum.Debuff:
                this.GetComponent<Animator>().SetBool("LaunchDebuff",false);
                break;
            default:
                break;
        }
    }
    
    public void EndTurn()
    {
        EndAnimBool();
        ChooseNextAction();
        EndTurnBM();

    }

    private void DecompteDebuff()
    {
        var skip = false;
        foreach (var item in Debuffs)
        {
            foreach (var effect in item.effects)
            {
                if(effect.type == BuffType.DmgPVMax)
                {
                    current.HP -= Mathf.RoundToInt((effect.pourcentageEffet * current.MaxHP) / 100);
                }
                if(effect.type == BuffType.DégatsBrut)
                {
                    current.HP -= effect.pourcentageEffet;
                }
                if(effect.type == BuffType.Doute || effect.type == BuffType.Stun)
                {
                    UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
                    var temp = UnityEngine.Random.Range(0,100);
                    if(temp <= effect.pourcentageEffet)
                        skip = true;
                }
            }
            

            if (item.Decompte == EffetTypeDecompte.tour)
                item.nbTemps--;
        }

        Debuffs.RemoveAll(c => c.nbTemps <= 0);
        if(skip)
            EndTurn();
    }

    public void StartPhase()
    {
        
        foreach (var item in Debuffs)
        {
            foreach (var effect in item.effects)
            {
                if(effect.type == BuffType.DmgPVMax)
                {
                    current.HP -= Mathf.RoundToInt((effect.pourcentageEffet * current.MaxHP) / 100);
                }
                if(effect.type == BuffType.DégatsBrut)
                {
                    current.HP -= effect.pourcentageEffet;
                }
            }
            

            if (item.Decompte == EffetTypeDecompte.round)
                item.nbTemps--;
        }

        Debuffs.RemoveAll(c => c.nbTemps <= 0);
    }

    public void CreateSpellList()
    {
        Spells = new List<EnemySpell>();
        
        if(current.Att1 != null)
            Spells.Add(current.Att1);
        if(current.Att2 != null)
            Spells.Add(current.Att2);
        if(current.Buff != null)
            Spells.Add(current.Buff);
        if(current.Debuff != null)
            Spells.Add(current.Debuff);

        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
        foreach (var item in Spells)
        {
            item.Weight += UnityEngine.Random.Range(0,4);
        }
    }
    
    public void ChooseNextAction()
    {
        bool colere = false;
        foreach (var item in Debuffs)
        {
            foreach (var effect in item.effects)
            {
                if(effect.type == BuffType.Colère)
                {
                    UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
                    var temp = UnityEngine.Random.Range(0,100);
                    if(temp <= effect.pourcentageEffet)
                        colere = true;
                }                
            }
        }
        if(Spells == null)
            CreateSpellList();

        nextAction = Spells.First();
        foreach (var item in Spells)
        {
            if(nextAction.IsAttaque && colere)
            {

            }
            else if(item.Weight < nextAction.Weight)
                nextAction = item;
        }

        nextAction.Weight += nextAction.AddedWeight;
        foreach (var item in Spells)
        {
            if(item != nextAction)
                item.Weight --;
        }
        NextActionType();
        updateIntention();
    }
    
    void updateIntention()
    {
        UICombat.ChangeIntention(nextAction.ImageIntentionSpell);
    }
    void NextActionType()
    {
         if(current.Att1 != null)
            if(current.Att1 == nextAction)
                nextActionType = nextActionEnum.Attaque;
         if(current.Att2 != null)
            if(current.Att2 == nextAction)
                nextActionType = nextActionEnum.Attaque2;
         if(current.Buff != null)
            if(current.Buff == nextAction)
                nextActionType = nextActionEnum.Buff;
         if(current.Debuff != null)
            if(current.Debuff == nextAction)
                nextActionType = nextActionEnum.Debuff;
    }

    #region TargetingMode
    public void SetTargetingMode()
    {
        UICombat.TargetingMode = true;
    }

    public void TargetAcquired()
    {
        GameManager.instance.BattleMan.idTarget = combatID;

    }

    public void EndTargetingMode()
    {
        UICombat.TargetingMode = false;
    }
#endregion
    
}