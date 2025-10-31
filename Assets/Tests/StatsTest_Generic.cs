

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
        public string ScenarioName;
        public int BaseValue;
        public int Modifier;
        public int Expected;
        public Func<PlayerStatsHandler, int> GetStat;
        public Func<PlayerStatsHandler, int> GetBaseStat;
        public Action<JoueurStat, int> SetStat;
        public override string ToString() => $"{StatName} [{ScenarioName}]";
    }
    private static (string name, Func<PlayerStatsHandler, int> get, Func<PlayerStatsHandler, int> getBase, Action<JoueurStat, int> set)[] AllStats = 
        {
            ("Radiance Max", f => f.RadianceMaxTotal, f => f.BaseRadianceMax, (x, v) => x.RadianceMax = v),
            ("Force D'ame", f => f.ForceDameTotal, f => f.BaseForceDame, (x, v) => x.ForceAme = v),
            ("Vitesse", f => f.VitesseTotal, f => f.BaseVitesse, (x, v) => x.Vitesse = v),
            ("Conviction", f => f.ConvictionTotal, f => f.BaseConviction, (x, v) => x.Conviction = v),
            ("Calme", f => f.CalmeTotal, f => f.BaseCalme, (x, v) => x.Calme = v),
            ("Resilience", f => f.ResilienceTotal, f => f.BaseResilience, (x, v) => x.Resilience = v)
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
                yield return new StatTestCase
                {
                    StatName = stat.name,
                    ScenarioName = scenar.name,
                    BaseValue = scenar.baseValue,
                    Modifier = scenar.modifier,
                    Expected = scenar.expected,
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
            ("Grande valeures", 12547896,  12547896)
        };

        foreach (var stat in AllStats)
        {
            foreach (var scenar in scenarii)
            {
                yield return new StatTestCase
                {
                    StatName = stat.name,
                    ScenarioName = scenar.name,
                    BaseValue = scenar.baseValue,
                    Expected = scenar.expected,
                    GetStat = stat.get,
                    SetStat = stat.set
                };
            }
        }
    }
    PlayerStatsHandler statToTest;
    private void InitStat(int baseValue)
    {

        var baseStat = StatsTests.CreatePlayerStatSO(baseValue, 3, 5);
        statToTest = new PlayerStatsHandler(baseStat);

        var obj = GameObject.FindObjectOfType<GameManager>();
        GameObject go = GameObject.Instantiate(obj.gameObject);
        GameManager gm = go.GetComponent<GameManager>();
        GameManager.Instance = gm;
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


}
