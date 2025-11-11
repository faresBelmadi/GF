

using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class StatsTests
{
    public struct StatTestCase
    {
        public string StatName;
        public StatEnum Stat;
        public string ScenarioName;
        public int BaseValue;
        public int Modifier;
        public int Expected;
        public Func<PlayerStatsHandler, int> GetStat;
        public Func<PlayerStatsHandler, int> GetBaseStat;
        public Action<JoueurStat, int> SetStat;
        public override string ToString() => $"{StatName} [{ScenarioName}]";
    }
    private static (string name, StatEnum stat, Func<PlayerStatsHandler, int> get, Func<PlayerStatsHandler, int> getBase, Action<JoueurStat, int> set)[] AllStats =
        {
            ("Radiance Max", StatEnum.RadianceMax, f => f.RadianceMaxTotal, f => f.BaseRadianceMax, (x, v) => x.RadianceMax = v),
            ("Force D'ame",StatEnum.ForceDame, f => f.ForceDameTotal, f => f.BaseForceDame, (x, v) => x.ForceAme = v),
            ("Vitesse", StatEnum.Vitesse, f => f.VitesseTotal, f => f.BaseVitesse, (x, v) => x.Vitesse = v),
            ("Conviction", StatEnum.Conviction, f => f.ConvictionTotal, f => f.BaseConviction, (x, v) => x.Conviction = v),
            ("Calme", StatEnum.Calme, f => f.CalmeTotal, f => f.BaseCalme, (x, v) => x.Calme = v),
            ("Resilience", StatEnum.Resilience, f => f.ResilienceTotal, f => f.BaseResilience, (x, v) => x.Resilience = v),
            ("Conscience Max", StatEnum.ConscienceMax, f => f.ConscienceMaxTotal, f=>f.BaseConscienceMax, (x,v) => x.ConscienceMax = v),
            ("Clairvoyance", StatEnum.Clairvoyance, f => f.ClairvoyanceTotal, f=>f.BaseClairvoyance, (x,v) => x.Clairvoyance = v)
        };
    public static IEnumerable<StatTestCase> AllAddCases()
    {
        
        var scenarii = new(string name, int baseValue, int modifier, int expected)[]
        {
            ("Ajout simple", 10, 10, 20),
            ("Sans ajout", 10, 0, 10),
            ("Soustraction simple", 10, -5, 5),
            ("Null", 0, 0, 0),
            ("Valeur négative", 10, -20, -10),
            ("Ajout Important", 1000, 5000, 6000),
        };

        foreach(var stat in AllStats)
        {
            foreach(var scenar in scenarii)
            {
                int expected = scenar.expected;
                if ((stat.name == "Force D'ame") && scenar.expected < 0)
                {
                    expected = 0;
                }
                else if ((stat.name == "Resilience") || (stat.name == "Conviction"))
                {
                    if (scenar.expected < -10) expected = -10;
                    else if (scenar.expected > 10) expected = 10;
                }
                yield return new StatTestCase
                {
                    StatName = stat.name,
                    ScenarioName = scenar.name,
                    BaseValue = scenar.baseValue,
                    Modifier = scenar.modifier,
                    Expected = expected,
                    GetStat = stat.get,
                    GetBaseStat = stat.getBase,
                    SetStat = stat.set
                };
            }
        }
    }
    public static IEnumerable<StatTestCase> AllCreateCases()
    {

        var scenarii = new (string name, int baseValue,  int expected)[]
        {
            ("Creation Simple", 10,  10),
            ("Creation Null", 0, 0),
            ("Creation négative", -7 , -7),
            ("Grande valeures positives", 12547896,  12547896),
            ("Grande valeures négatives", -12547896,  -12547896)
        };

        foreach (var stat in AllStats)
        {
            foreach (var scenar in scenarii)
            {
                int expected = scenar.expected;
                if ((stat.name == "Force D'ame") && scenar.expected < 0)
                {
                    expected = 0;
                }
                else if ((stat.name == "Resilience") || (stat.name == "Conviction"))
                {
                    if (scenar.expected < -10) expected = -10;
                    else if (scenar.expected > 10) expected = 10;
                }
                yield return new StatTestCase
                {
                    StatName = stat.name,
                    ScenarioName = scenar.name,
                    BaseValue = scenar.baseValue,
                    Expected = expected,
                    GetStat = stat.get,
                    SetStat = stat.set
                };
            }
        }
    }

    public static IEnumerable<StatTestCase> AllPercentCases()
    {

        var scenarii = new (string name, int baseValue, int percentValue, int expected)[]
        {
            ("0%", 10, 0, 0),
            ("10%", 10, 10, 1),
            ("22%", 10, 22, 2),
            ("25%", 10, 25, 2),
            ("28%", 10, 28, 3),
            ("100%", 10, 100, 10),
            ("200%", 10, 200, 20),
        };

        foreach (var stat in AllStats)
        {
            foreach (var scenar in scenarii)
            {
                yield return new StatTestCase
                {
                    StatName = stat.name,
                    Stat = stat.stat,
                    ScenarioName = scenar.name,
                    BaseValue = scenar.baseValue,
                    Modifier = scenar.percentValue,
                    Expected = scenar.expected,
                    GetStat = stat.get,
                    GetBaseStat = stat.getBase,
                    SetStat = stat.set
                };
            }
        }
    }

   
  

    [Test, TestCaseSource(nameof(AllAddCases))]
    public void TestAddStat(StatTestCase testCase)
    {
        InitStat(testCase.BaseValue);

        var modifStat = ScriptableObject.CreateInstance<JoueurStat>();
        testCase.SetStat(modifStat, testCase.Modifier);

        statToTest.UpdateStat(modifStat);

        int value = testCase.GetStat(statToTest);
        Assert.That(value, Is.EqualTo(testCase.Expected), 
            $"Erreur sur {testCase.StatName} ({testCase.ScenarioName}) : attendu {testCase.Expected}, obtenu {value}");
    }

    [Test, TestCaseSource(nameof(AllAddCases))]
    public void TestAddBaseStat(StatTestCase testCase)
    {
        InitStat(testCase.BaseValue);

        var modifStat = ScriptableObject.CreateInstance<JoueurStat>();
        testCase.SetStat(modifStat, testCase.Modifier);

        statToTest.UpdateBaseStat(modifStat);

        int value = testCase.GetBaseStat(statToTest);
        Assert.That(value, Is.EqualTo(testCase.Expected),
            $"Erreur sur {testCase.StatName} ({testCase.ScenarioName}) : attendu {testCase.Expected}, obtenu {value}");
    }

    [Test, TestCaseSource(nameof(AllCreateCases))]
    public void TestCreateStat(StatTestCase testCase)
    {
        InitStat(testCase.BaseValue);


        int value = testCase.GetStat(statToTest);
        Assert.That(value, Is.EqualTo(testCase.Expected),
            $"Erreur sur {testCase.StatName} ({testCase.ScenarioName}) : attendu {testCase.Expected}, obtenu {value}");
    }

    [Test, TestCaseSource(nameof(AllPercentCases))]
    public void TestPercentStat(StatTestCase testCase)
    {
        InitStat(testCase.BaseValue);

        int value = statToTest.GetPercentValue(testCase.Stat, testCase.Modifier);
        Assert.That(value, Is.EqualTo(testCase.Expected),
            $"Erreur sur {testCase.StatName} ({testCase.ScenarioName}) : attendu {testCase.Expected}, obtenu {value}");
    }

}
