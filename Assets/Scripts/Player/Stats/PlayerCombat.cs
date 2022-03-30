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
    public List<GameObject> ListBuff = new List<GameObject>();

    public List<GameObject> Spells;
    public Transform DamageSpawn;
    public Transform BuffContainer;
    public Transform DebuffContainer;
    public GameObject DamagePrefab;
    public GameObject SoinPrefab;
    public GameObject BuffPrefab;
    public GameObject DebuffPrefab;
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
    public TextMeshProUGUI StatSpeedText;
    public TextMeshProUGUI StatForceAmeText;
    public TextMeshProUGUI StatResilienceText;
    public TextMeshProUGUI StatClairvoyanceText;
    public TextMeshProUGUI StatConvictionText;
    
    public Action EndTurnBM;
    
    public Spell SelectedSpell;

    public AnimationControllerAttack AnimationController;


    public int LastDamageTaken;

    public void EnervementTension()
    {
        var t = (int)((stat.Tension/(stat.NbPalier*stat.ValeurPalier)) * stat.NbPalier);
        if(t >= stat.NbPalier)
        t = stat.NbPalier;
        else
        t ++;

        stat.Tension = t*stat.ValeurPalier;
    }

    public void ApaisementTension()
    {
        
        var t = (int)((stat.Tension/(stat.NbPalier*stat.ValeurPalier)) * stat.NbPalier);
        if(t <= 0)
        t = 0;
        else
        t --;

        stat.Tension = t*stat.ValeurPalier;
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

        UpdateUI();
        
    }

    public void UpdateUI()
    {
        HP.value = stat.HP;
        HP.minValue = 0;
        HP.maxValue = stat.MaxHP;
        Tension.value = Mathf.FloorToInt((stat.Tension*stat.NbPalier)/stat.TensionMax);
        Tension.maxValue = stat.NbPalier;
        Volonté.value = stat.Volonté;
        Volonté.maxValue = stat.MaxVolonté;
        Conscience.value = stat.Conscience;
        Conscience.maxValue = stat.MaximumConscience;

        HpText.text = stat.HP +"/" + stat.MaxHP;

        ConscienceText.text = stat.Conscience +"/" + stat.MaximumConscience;

        StatClairvoyanceText.text = stat.Clairvoyance+"";

       if(stat.Clairvoyance > stat.ClairvoyanceOriginal)
            StatClairvoyanceText.color = Color.green;
        else if(stat.Clairvoyance < stat.ClairvoyanceOriginal)
            StatClairvoyanceText.color = Color.red;
        else
            StatClairvoyanceText.color = Color.black;


        StatForceAmeText.text = stat.Dmg+"";

        if(stat.Dmg > stat.ForceAmeOriginal)
            StatForceAmeText.color = Color.green;
        else if(stat.Dmg < stat.ForceAmeOriginal)
            StatForceAmeText.color = Color.red;
        else
            StatForceAmeText.color = Color.black;

        StatSpeedText.text = stat.Speed+"";
        
        if(stat.Speed > stat.SpeedOriginal)
            StatSpeedText.color = Color.green;
        else if(stat.Speed < stat.SpeedOriginal)
            StatSpeedText.color = Color.red;
        else
            StatSpeedText.color = Color.black;

        StatConvictionText.text = stat.Conviction+"";

        if(stat.Conviction > stat.ConvictionOriginal)
            StatConvictionText.color = Color.green;
        else if(stat.Conviction < stat.ConvictionOriginal)
            StatConvictionText.color = Color.red;
        else
            StatConvictionText.color = Color.black;

        StatResilienceText.text =stat.Resilience+"";

        if(stat.Resilience > stat.ResilienceOriginal)
            StatResilienceText.color = Color.green;
        else if(stat.Resilience < stat.ResilienceOriginal)
            StatResilienceText.color = Color.red;
        else
            StatResilienceText.color = Color.black;

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
        if(toAdd.IsDebuff)
        {
            ReceiveTension(Source.Buff);
            var t = ListBuff.FirstOrDefault(c => c.GetComponentInChildren<TextMeshProUGUI>().text == toAdd.NomDebuff);
            if(t == null)
            {
                t = Instantiate(DebuffPrefab, DebuffContainer.transform);
                t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNom").text = toAdd.NomDebuff;
                t.GetComponent<DescriptionHoverTrigger>().Description.text = toAdd.Description;
                ListBuff.Add(t);
            }
            else
            {
                var s = t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNb").text;
                int nb = int.Parse(s);
                s = nb+1+"";
                t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNb").text = s;
            }

        }
        else
        {
            var t = ListBuff.FirstOrDefault(c => c.GetComponentInChildren<TextMeshProUGUI>().text == toAdd.NomDebuff);
            if(t == null)
            {
                t = Instantiate(BuffPrefab, BuffContainer.transform);
                t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNom").text = toAdd.NomDebuff;
                t.GetComponent<DescriptionHoverTrigger>().Description.text = toAdd.Description;
                ListBuff.Add(t);
            }
            else
            {
                var s = t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNb").text;
                int nb = int.Parse(s);
                s = nb+1+"";
                t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNb").text = s;
            }
        }
        //instantiate parceque chaque buff/debuff est un scriptable object donc si on modifie les valeurs ca sera sauvegardé entre les lancements
        //instantiate permet de créer une copy du buff/debuff
        debuffs.Add(Instantiate(toAdd));
        
        ApplicationEffetStatBuffDebuff();
        ApplicationBuffDebuffDegats();

        UpdateUI();
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
        DecompteDebuff();//Peut changer en fin de tour..............
        foreach (var item in Spells)
        {
            item.GetComponent<spellCombat>().isTurn = true;
        }
        EndTurnButton.interactable = true;
        stat.Volonté = stat.MaxVolonté;
        UpdateUI();
    }


    public void EndTurn()
    {
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
    public void ActivateSpells()
    {
        foreach (var item in Spells)
        {
            item.GetComponent<spellCombat>().isTurn = true;
        }
        EndTurnButton.interactable = true;
    }


    private void DecompteDebuff()
    {
        foreach (var item in debuffs)
        {
            if (item.Decompte == EffetTypeDecompte.tour)
                item.nbTemps--;

            if (item.nbTemps < 0)
            {
                var t = ListBuff.FirstOrDefault(c => c.GetComponentInChildren<TextMeshProUGUI>().text == item.NomDebuff);
                if (t != null)
                {
                    var s = t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNb").text;
                    int nb = int.Parse(s);
                    nb -= 1;
                    s = nb + "";
                    if (nb <= 0)
                    {
                        ListBuff.Remove(t);
                        GameObject.Destroy(t);
                    }
                    else
                        t.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNb").text = s;
                }
            }
        }

        debuffs.RemoveAll(c => c.nbTemps < 0);

        ApplicationEffetStatBuffDebuff();
        ApplicationBuffDebuffDegats();

        UpdateUI();
    }

    private void ApplicationEffetStatBuffDebuff()
    {

        ResetStat();
        foreach (var item in debuffs)
        {
            foreach (var effect in item.effects)
            {

                if (effect.type == BuffType.Résilience)
                {
                    stat.Resilience += effect.pourcentageEffet;
                }
                if (effect.type == BuffType.Conviction)
                {
                    stat.Conviction += effect.pourcentageEffet;
                }
                if (effect.type == BuffType.VitesseBrut)
                {
                    stat.Speed += effect.pourcentageEffet;
                }
                if (effect.type == BuffType.Vitesse)
                {
                    stat.Speed += Mathf.RoundToInt((effect.pourcentageEffet * stat.Speed) / 100f);
                }
                if (effect.type == BuffType.Clairvoyance)
                {
                    stat.Clairvoyance += effect.pourcentageEffet;
                }

            }
        }
    }


    public void ApplicationBuffDebuffDegats()
    {
        foreach (var item in debuffs)
        {
            foreach (var effect in item.effects)
            {
                if (effect.type == BuffType.Ponction)
                {
                    stat.MaxHP += Mathf.RoundToInt((effect.pourcentageEffet * stat.MaxHP) / 100f);
                }
                if (effect.type == BuffType.PVMax)
                {
                    stat.MaxHP += Mathf.RoundToInt((effect.pourcentageEffet * stat.MaxHP) / 100f);
                }
                if (effect.type == BuffType.DmgPVMax)
                {
                    stat.HP -= Mathf.RoundToInt((effect.pourcentageEffet * stat.MaxHP) / 100);
                }
                if (effect.type == BuffType.DégatsBrut)
                {
                    stat.HP -= effect.pourcentageEffet;
                }
                if (effect.type == BuffType.Att)
                {
                    stat.Dmg += Mathf.RoundToInt((effect.pourcentageEffet * stat.Dmg) / 100f);
                }
                if (effect.type == BuffType.AttBrut)
                {
                    stat.Dmg += effect.pourcentageEffet;
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
        stat.Dmg = stat.ForceAmeOriginal;
        stat.Conviction = stat.ConvictionOriginal;
    }

    public void StartPhase()
    {
        
        for (int i = 0; i < debuffs.Count; i++)
        {
            
            if(debuffs[i].Decompte == EffetTypeDecompte.round)
               debuffs[i].nbTemps--;
        }

        debuffs.RemoveAll(c => c.nbTemps < 0);
    }

    public void StartUp()
    {
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
        DesactivateSpells();
        AnimationController.StartAttack(AfterAnim);
        GameManager.instance.BattleMan.LaunchAnimAttacked();
    }

    void AfterAnim()
    {
        GameManager.instance.BattleMan.GetListEffectPlayer(SelectedSpell);
        ActivateSpells();
        UpdateUI();
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
