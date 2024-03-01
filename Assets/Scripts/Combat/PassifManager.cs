using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PassifManager
{
    private readonly PassifRules _rules = GameManager.instance.passifRules;
    private readonly BattleManager _refBattleManager = GameManager.instance.BattleMan;

    private List<EnnemyBehavior> _currentBattleEnemy;
    private List<JoueurBehavior> _currentBattleJoueur;
    public TimerPassif CurrentEvent;

    public PassifManager(List<JoueurBehavior> currentBattleJoueur, List<EnnemyBehavior> currentBattleEnnemiStats)
    {
        _currentBattleJoueur = currentBattleJoueur;
        _currentBattleEnemy = currentBattleEnnemiStats;
    }

    public void ResolvePassifs()
    {

        foreach (var behavior in _currentBattleEnemy)
        {
            foreach (var item in behavior.Stat.ListPassif)
            {
                if (item.timerPassif == CurrentEvent && behavior.IsTurn == true)
                {
                    switch (item.passif)
                    {
                        case TypePassif.PassifJeanneFinAction:
                            UpdateDivinInfoDisplay(behavior);
                            break;
                        case TypePassif.PassifJeanneDebutTour:
                            int rand;

                            if (behavior.nextAction.Name == "UltimeJeanne")
                                rand = Random.Range(1,4);
                            else
                                rand = Random.Range(0, 4);
                            if (rand == 0)
                            {
                                behavior.Stat.Tension += behavior.Stat.Divin;
                                behavior.Stat.Divin = 0;
                            }
                            else
                            {
                                behavior.Stat.Divin += rand * 10;
                            }
                            var pourcentagePVActuel = (behavior.Stat.Radiance / behavior.Stat.RadianceMax) * 100f;
                            behavior.Stat.RadianceMax += behavior.Stat.Divin * 10;
                            behavior.Stat.Radiance = Mathf.FloorToInt(pourcentagePVActuel / 100 * behavior.Stat.RadianceMax);

                            behavior.Stat.ForceAme += behavior.Stat.Divin * 2;

                            UpdateDivinInfoDisplay(behavior);
                            
                            break;
                        //case TypePassif.PassifCameleon:
                        //    break;
                        case TypePassif.PassifChefCultiste:
                            break;
                        case TypePassif.PassifCultiste:
                            break;
                        case TypePassif.PassifMartyr:
                            break;
                        case TypePassif.PassifClou:
                            break;
                        case TypePassif.PassifPapa:
                            foreach (var joueurBehavior in _currentBattleJoueur)
                            {
                                if (joueurBehavior.Stat.Radiance > behavior.Stat.Radiance)
                                {
                                    behavior.Stat.ForceAmeBonus = Mathf.FloorToInt(((_rules.nbPercentBuffForceAmePapa / 100f) * behavior.Stat._forceAme));
                                }
                                else if (joueurBehavior.Stat.Radiance < behavior.Stat.Radiance)
                                {
                                    behavior.Stat.ForceAmeBonus = Mathf.FloorToInt(((_rules.nbPercentDebuffForceAmePapa / 100f) * behavior.Stat._forceAme));
                                }
                            }
                            break;
                        case TypePassif.PassifPapy:
                            foreach (var joueurBehavior in _currentBattleJoueur)
                            {
                                if (joueurBehavior.ListBuffDebuffGO.Count >= _rules.nbBuffTriggerPapy)
                                {
                                    behavior.Stat.Tension += behavior.Stat.ValeurPalier;
                                    List<BuffDebuff> tempPapyDebuffs = new List<BuffDebuff>
                                    {
                                        _rules.DebuffClairvoyancePapy,
                                        _rules.DebuffFaPapy,
                                        _rules.DebuffVitessePapy
                                    };
                                    var randomInt = UnityEngine.Random.Range(0, tempPapyDebuffs.Count);
                                    joueurBehavior.AddDebuff(tempPapyDebuffs[randomInt], tempPapyDebuffs[randomInt].Decompte, tempPapyDebuffs[randomInt].timerApplication);
                                }
                            }
                            break;
                    }
                }
            }

        }

        foreach (var behavior in _currentBattleJoueur)
        {
            foreach (var item in behavior.Stat.ListPassif)
            {
                if (item.timerPassif == CurrentEvent)
                {
                    switch (item.passif)
                    {
                        case TypePassif.PassifGuerrier1:
                            // vous avez 1 points de résilience par point de conscience que vous possédez
                            var resilienceBonus = (behavior.Stat.Conscience / _rules.nbPtsConscience) * _rules.nbPtsResilience;
                            behavior.Stat.ResiliencePassif = Mathf.FloorToInt(resilienceBonus);
                            break;
                        case TypePassif.PassifGuerrier2:
                            //Lorsque vous terminez un affrontement sans avoir consommé d'Essences, vous récupérez 1 point de Conscience et le total d'Essences obtenu est augmenté de 10%.        
                            if (!_refBattleManager.ConsumedEssence)
                            {
                                var essenceAmount = _refBattleManager.ListEssence.First().GetComponent<Essence>().amount;
                                essenceAmount += (int)Math.Round((double)(_rules.PercentEssenceBonus * 100f) / essenceAmount);
                                _refBattleManager.ListEssence.First().GetComponent<Essence>().amount = essenceAmount;
                                behavior.Stat.Conscience += Mathf.RoundToInt(_rules.nbPtsConscienceEarned);
                            }
                            break;
                    }
                }
            }
        }

    }

    public void UpdateDivinInfoDisplay(EnnemyBehavior behavior)
    {
        if (!behavior.Stat.ListBuffDebuff.Any(x => x.Nom == "Current Divin"))
        {
            _rules.CurrentDivin.Description = behavior.Stat.Divin.ToString();
            behavior.AddBuffDebuff(_rules.CurrentDivin, behavior.Stat);
        }

        var currentDivin = behavior.ListBuffDebuffGO.FirstOrDefault(x =>
            x.GetComponentsInChildren<TextMeshProUGUI>().First(c => c.gameObject.name == "TextNom").text ==
            "Current Divin");
        currentDivin.GetComponent<DescriptionHoverTriggerBuffDebuff>().Description.text =
            "Divin : " + behavior.Stat.Divin;
    }
}