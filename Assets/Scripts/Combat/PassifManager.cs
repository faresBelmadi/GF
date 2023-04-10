using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
                if (item.timerPassif == CurrentEvent)
                {
                    switch (item.passif)
                    {
                        case TypePassif.PassifJeanne:
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
                                if (joueurBehavior.ListBuffDebuff.Count >= _rules.nbBuffTriggerPapy)
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
                            behavior.Stat.ResilienceBonus = resilienceBonus;
                            break;
                        case TypePassif.PassifGuerrier2:
                            //Lorsque vous terminez un affrontement sans avoir consommé d'Essences, vous récupérez 1 point de Conscience et le total d'Essences obtenu est augmenté de 10%.        
                            if (!_refBattleManager.ConsumedEssence)
                            {
                                var essenceAmount = _refBattleManager.ListEssence.First().GetComponent<Essence>().amount;
                                essenceAmount += (int)Math.Round((double)(_rules.PercentEssenceBonus * 100) / essenceAmount);
                                behavior.Stat.Conscience += Mathf.RoundToInt(_rules.nbPtsConscienceEarned);
                            }
                            break;
                    }
                }
            }
        }

    }



}
