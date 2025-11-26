
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class StatsTests
{
    public class MemoryTestCase
    {
        public string Name;
        public int InitialValue;
        public int ExpectedBaseValue;
        public int ExpectedValue;
        public Func<PlayerStatsHandler, int> Get;
        public Func<PlayerStatsHandler, int> GetBase;
        public List<List<ModificationStatSouvenir>> ModifStatList;
        public override string ToString() => $"{Name}";
    }
    private Souvenir CreateSouvenir(List<ModificationStatSouvenir> modifList)
    {
        Souvenir souvenir = ScriptableObject.CreateInstance<Souvenir>();
        souvenir.DefaultName = "Debug Souvenir";
        souvenir.ProcEmotion = new List<PourcentageEmotion> { new PourcentageEmotion
        {
            Pourcentage = 100,
            Emotion = Emotion.Joie
        } };
        souvenir.Emotion = Emotion.Joie;
        souvenir.Slots = 1;
        souvenir.ModificationStat = new List<ModificationStatSouvenir>(modifList);
        //{
        //    new ModificationStatSouvenir
        //    {
        //        StatModif = StatModif.ForceAme,
        //        ParametreModifStat = new ParametreModifStat { ParametreStat = ParametreStat.ValeurBrut, Valeur = 10 }
        //    } }
        //;
        souvenir.Rarete = Rarity.Commun;
        souvenir.Equiped = false;
        return souvenir;
    }
    private static (string name, int initialValue, int expectedValue, Func<PlayerStatsHandler, int> get, Func<PlayerStatsHandler, int> getBase, List<List<ModificationStatSouvenir>> modifStatList)[] AllMemoryTestCase =
        {
            ("Radiance Max Flat", 100, 110,f => f.RadianceMaxTotal, f => f.BaseRadianceMax, new List <List<ModificationStatSouvenir>> {new List<ModificationStatSouvenir>{ new ModificationStatSouvenir
            {
                StatModif = StatModif.RadianceMax,
                ParametreModifStat = new ParametreModifStat { ParametreStat = ParametreStat.ValeurBrut, Valeur = 10 }
            } } } ),
            ("Radiance Max Percent", 100, 125,f => f.RadianceMaxTotal, f => f.BaseRadianceMax, new List <List<ModificationStatSouvenir>> {new List<ModificationStatSouvenir>{ new ModificationStatSouvenir
            {
                StatModif = StatModif.RadianceMax,
                ParametreModifStat = new ParametreModifStat { ParametreStat = ParametreStat.Pourcentage, Valeur = 25 }
            } } } ),
             ("Radiance Max Percent non entier", 50, 62,f => f.RadianceMaxTotal, f => f.BaseRadianceMax, new List <List<ModificationStatSouvenir>> {new List<ModificationStatSouvenir>{ new ModificationStatSouvenir
            {
                StatModif = StatModif.RadianceMax,
                ParametreModifStat = new ParametreModifStat { ParametreStat = ParametreStat.Pourcentage, Valeur = 25 }
            } } } ),
            ("Force D'ame Flat",100, 110, f => f.ForceDameTotal, f => f.BaseForceDame, new List <List<ModificationStatSouvenir>> { new List<ModificationStatSouvenir>{ new ModificationStatSouvenir
            {
                StatModif = StatModif.ForceAme,
                ParametreModifStat = new ParametreModifStat { ParametreStat = ParametreStat.ValeurBrut, Valeur = 10 }
            } } } ),
             ("Force D'ame Percent",50, 55, f => f.ForceDameTotal, f => f.BaseForceDame, new List <List<ModificationStatSouvenir>> { new List<ModificationStatSouvenir>{ new ModificationStatSouvenir
            {
                StatModif = StatModif.ForceAme,
                ParametreModifStat = new ParametreModifStat { ParametreStat = ParametreStat.Pourcentage, Valeur = 10 }
            } } } ),
            ("Vitesse flat",100, 110, f => f.VitesseTotal, f => f.BaseVitesse, new List <List<ModificationStatSouvenir>> { new List<ModificationStatSouvenir>{ new ModificationStatSouvenir
            {
                StatModif = StatModif.Vitesse,
                ParametreModifStat = new ParametreModifStat { ParametreStat = ParametreStat.ValeurBrut, Valeur = 10 }
            } } }),
             ("Vitesse Pourcentage",100, 110, f => f.VitesseTotal, f => f.BaseVitesse, new List <List<ModificationStatSouvenir>> { new List<ModificationStatSouvenir>{ new ModificationStatSouvenir
            {
                StatModif = StatModif.Vitesse,
                ParametreModifStat = new ParametreModifStat { ParametreStat = ParametreStat.Pourcentage, Valeur = 10 }
            } } }),
            ("Conviction",5, 10, f => f.ConvictionTotal, f => f.BaseConviction, new List <List<ModificationStatSouvenir>> {  new List<ModificationStatSouvenir>{ new ModificationStatSouvenir
            {
                StatModif = StatModif.Conviction,
                ParametreModifStat = new ParametreModifStat { ParametreStat = ParametreStat.ValeurBrut, Valeur = 5 }
            } } } ),
            ("Calme",100, 110, f => f.CalmeTotal, f => f.BaseCalme,new List <List<ModificationStatSouvenir>> {  new List<ModificationStatSouvenir>{ new ModificationStatSouvenir
            {
                StatModif = StatModif.Calme,
                ParametreModifStat = new ParametreModifStat { ParametreStat = ParametreStat.ValeurBrut, Valeur = 10 }
            } } }),
             ("Calme Pourcentage",100, 110, f => f.CalmeTotal, f => f.BaseCalme,new List <List<ModificationStatSouvenir>> {  new List<ModificationStatSouvenir>{ new ModificationStatSouvenir
            {
                StatModif = StatModif.Calme,
                ParametreModifStat = new ParametreModifStat { ParametreStat = ParametreStat.Pourcentage, Valeur = 10 }
            } } }),
            ("Resilience",5, 10, f => f.ResilienceTotal, f => f.BaseResilience, new List <List<ModificationStatSouvenir>> { new List<ModificationStatSouvenir>{ new ModificationStatSouvenir
            {
                StatModif = StatModif.Resilience,
                ParametreModifStat = new ParametreModifStat { ParametreStat = ParametreStat.ValeurBrut, Valeur = 5 }
            } } }),
            ("ConscienceMax",100, 110, f => f.ConscienceMaxTotal, f => f.BaseConscienceMax,new List <List<ModificationStatSouvenir>> {  new List<ModificationStatSouvenir>{ new ModificationStatSouvenir
            {
                StatModif = StatModif.ConscienceMax,
                ParametreModifStat = new ParametreModifStat { ParametreStat = ParametreStat.ValeurBrut, Valeur = 10 }
            } } }),
            ("Clairvoyance flat",100, 110, f => f.ClairvoyanceTotal, f => f.BaseClairvoyance,new List <List<ModificationStatSouvenir>> {  new List<ModificationStatSouvenir>{ new ModificationStatSouvenir
            {
                StatModif = StatModif.Clairvoyance,
                ParametreModifStat = new ParametreModifStat { ParametreStat = ParametreStat.ValeurBrut, Valeur = 10 }
            } } }),
            ("Clairvoyance pourcentage",100, 110, f => f.ClairvoyanceTotal, f => f.BaseClairvoyance,new List <List<ModificationStatSouvenir>> {  new List<ModificationStatSouvenir>{ new ModificationStatSouvenir
            {
                StatModif = StatModif.Clairvoyance,
                ParametreModifStat = new ParametreModifStat { ParametreStat = ParametreStat.Pourcentage, Valeur = 10 }
            } } }),
            ("ForceD'ame Mult",100, 160, f => f.ForceDameTotal, f => f.BaseForceDame, new List <List<ModificationStatSouvenir>> { new List<ModificationStatSouvenir>
            {
                new ModificationStatSouvenir
                {
                    StatModif = StatModif.ForceAme,
                    ParametreModifStat = new ParametreModifStat { ParametreStat = ParametreStat.ValeurBrut, Valeur = 10 },
                },
                new ModificationStatSouvenir
                {
                    StatModif = StatModif.ForceAme,
                    ParametreModifStat = new ParametreModifStat { ParametreStat = ParametreStat.ValeurBrut, Valeur = 20 }
                },
                new ModificationStatSouvenir
                {
                    StatModif = StatModif.ForceAme,
                    ParametreModifStat = new ParametreModifStat { ParametreStat = ParametreStat.ValeurBrut, Valeur = 30 }
                }

            } }),

        };

    private static (string name, int initialValue, int expectedBaseValue, int expectedValue, Func<PlayerStatsHandler, int> get, Func<PlayerStatsHandler, int> getBase, List<List<ModificationStatSouvenir>> modifStatList)[] AllMultiplMemoryTestCase =
       {
            ("Radiance Max", 50, 100, 125,f => f.RadianceMaxTotal, f => f.BaseRadianceMax, new List <List<ModificationStatSouvenir>> {new List<ModificationStatSouvenir>{ new ModificationStatSouvenir
                {
                    StatModif = StatModif.RadianceMax,
                    ParametreModifStat = new ParametreModifStat { ParametreStat = ParametreStat.ValeurBrut, Valeur = 50 }
                }, new ModificationStatSouvenir
                {
                    StatModif = StatModif.RadianceMax,
                    ParametreModifStat = new ParametreModifStat { ParametreStat = ParametreStat.Pourcentage, Valeur = 25 }
                }

            } } ),
            ("Radiance Max Reverse Order", 50, 100, 125,f => f.RadianceMaxTotal, f => f.BaseRadianceMax, new List <List<ModificationStatSouvenir>> {new List<ModificationStatSouvenir>{ new ModificationStatSouvenir
            {
                StatModif = StatModif.RadianceMax,
                ParametreModifStat = new ParametreModifStat { ParametreStat = ParametreStat.Pourcentage, Valeur = 25 }
            }, new ModificationStatSouvenir
            {
                StatModif = StatModif.RadianceMax,
                ParametreModifStat = new ParametreModifStat { ParametreStat = ParametreStat.ValeurBrut, Valeur = 50 }
            }

            } } )

        };

    private static IEnumerable<MemoryTestCase> AllMemoryEquipeValueTestCase()
    {
        foreach (var memoryCase in AllMemoryTestCase)
        {
            yield return new MemoryTestCase
            {
                Name = memoryCase.name,
                InitialValue = memoryCase.initialValue,
                ExpectedValue = memoryCase.expectedValue,
                Get = memoryCase.get,
                GetBase = memoryCase.getBase,
                ModifStatList = memoryCase.modifStatList
            };
        }
    }
    private static IEnumerable<MemoryTestCase> AllMemoryMultiEquipeValueTestCase()
    {
        foreach (var memoryCase in AllMultiplMemoryTestCase)
        {
            yield return new MemoryTestCase
            {
                Name = memoryCase.name,
                InitialValue = memoryCase.initialValue,
                ExpectedBaseValue = memoryCase.expectedBaseValue,
                ExpectedValue = memoryCase.expectedValue,
                Get = memoryCase.get,
                GetBase = memoryCase.getBase,
                ModifStatList = memoryCase.modifStatList
            };
        }
    }

    private Souvenir LoadSouvenirObject(string nameSouvenir)
    {
        string scriptableObjectName = nameSouvenir;
        string[] guids = AssetDatabase.FindAssets($"t:{nameof(Souvenir)} {scriptableObjectName}");
        if (guids.Length == 0)
            Assert.Fail($"No {nameof(Souvenir)} found named {scriptableObjectName}");

        if (guids.Length > 0)
            Debug.LogWarning($"More than one {nameof(Souvenir)} found named {scriptableObjectName}, taking first one");

        return (Souvenir)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guids[0]), typeof(Souvenir));
    }

    
    [Test, TestCaseSource(nameof(AllMemoryEquipeValueTestCase))]
    public void MemoryEquip(MemoryTestCase testCase)
    {
        var baseStat = CreatePlayerStatSO(testCase.InitialValue, 3, 5);
        var statHolder = new PlayerStatsHandler(baseStat);
        var statTmpHolder = new PlayerStatsHandler(baseStat);

        var obj = GameObject.FindObjectOfType<GameManager>();
        GameObject go = GameObject.Instantiate(obj.gameObject);
        GameManager gm = go.GetComponent<GameManager>();
        GameManager.Instance = gm;

        var msm = GameObject.FindObjectOfType<MenuStatManager>(true);
        GameObject msmgo = GameObject.Instantiate(msm.gameObject);
        MenuStatManager menuStatMng = msmgo.GetComponent<MenuStatManager>();

        menuStatMng.Stat = statHolder;
        menuStatMng.StatTemp = statTmpHolder;

        Souvenir souv = CreateSouvenir(testCase.ModifStatList[0]);
        statTmpHolder.ListSouvenir.Add(souv);
        statHolder.ListSouvenir.Add(souv);
        menuStatMng.Equiped(souv);
        int modifiedBaseValue = testCase.GetBase(statTmpHolder);
        int modifiedTotalValue = testCase.Get(statTmpHolder);
        int expectedBaseValue = (testCase.ModifStatList[0][0].ParametreModifStat.ParametreStat == ParametreStat.Pourcentage)?testCase.InitialValue:testCase.ExpectedValue;
        Assert.That(modifiedBaseValue, Is.EqualTo(expectedBaseValue),
            $"Erreur sur {testCase.Name} : attendu {testCase.ExpectedValue}, obtenu (Base : {modifiedBaseValue}, Total : {modifiedTotalValue}");
        Assert.That(modifiedTotalValue, Is.EqualTo(testCase.ExpectedValue),
            $"Erreur sur {testCase.Name} : attendu {testCase.ExpectedValue}, obtenu (Base : {modifiedBaseValue}, Total : {modifiedTotalValue}");
        statHolder.ListSouvenir.Clear();
        statTmpHolder.ListSouvenir.Clear();
    }

    [Test, TestCaseSource(nameof(AllMemoryEquipeValueTestCase))]
    public void MemoryUnequip(MemoryTestCase testCase)
    {
        var baseStat = CreatePlayerStatSO(testCase.InitialValue, 3, 5);
        var statHolder = new PlayerStatsHandler(baseStat);
        var statTmpHolder = new PlayerStatsHandler(baseStat);

        var obj = GameObject.FindObjectOfType<GameManager>();
        GameObject go = GameObject.Instantiate(obj.gameObject);
        GameManager gm = go.GetComponent<GameManager>();
        GameManager.Instance = gm;

        var msm = GameObject.FindObjectOfType<MenuStatManager>(true);
        GameObject msmgo = GameObject.Instantiate(msm.gameObject);
        MenuStatManager menuStatMng = msmgo.GetComponent<MenuStatManager>();

        menuStatMng.Stat = statHolder;
        menuStatMng.StatTemp = statTmpHolder;

        // Equip
        Souvenir souv = CreateSouvenir(testCase.ModifStatList[0]);
        statTmpHolder.ListSouvenir.Add(souv);
        statHolder.ListSouvenir.Add(souv);
        menuStatMng.Equiped(souv);

        // Unequip
        menuStatMng.UnEquiped(souv);
        statTmpHolder.ListSouvenir.Remove(souv);
        statHolder.ListSouvenir.Remove(souv);

        int modifiedBaseValue = testCase.GetBase(statTmpHolder);
        int modifiedTotalValue = testCase.Get(statTmpHolder);
        Assert.That(modifiedBaseValue, Is.EqualTo(testCase.InitialValue),
            $"Erreur sur {testCase.Name} : attendu {testCase.InitialValue}, obtenu (Base : {modifiedBaseValue}, Total : {modifiedTotalValue}");
        Assert.That(modifiedTotalValue, Is.EqualTo(testCase.InitialValue),
            $"Erreur sur {testCase.Name} : attendu {testCase.InitialValue}, obtenu (Base : {modifiedBaseValue}, Total : {modifiedTotalValue}");
        statHolder.ListSouvenir.Clear();
        statTmpHolder.ListSouvenir.Clear();
    }

    [Test, TestCaseSource(nameof(AllMemoryMultiEquipeValueTestCase))]
    public void MemoryMultipEquip(MemoryTestCase testCase)
    {
        var baseStat = CreatePlayerStatSO(testCase.InitialValue, 3, 5);
        var statHolder = new PlayerStatsHandler(baseStat);
        var statTmpHolder = new PlayerStatsHandler(baseStat);

        var obj = GameObject.FindObjectOfType<GameManager>();
        GameObject go = GameObject.Instantiate(obj.gameObject);
        GameManager gm = go.GetComponent<GameManager>();
        GameManager.Instance = gm;

        var msm = GameObject.FindObjectOfType<MenuStatManager>(true);
        GameObject msmgo = GameObject.Instantiate(msm.gameObject);
        MenuStatManager menuStatMng = msmgo.GetComponent<MenuStatManager>();

        menuStatMng.Stat = statHolder;
        menuStatMng.StatTemp = statTmpHolder;

        foreach (var souvCase in testCase.ModifStatList)
        {
            Souvenir souv = CreateSouvenir(souvCase);
            statTmpHolder.ListSouvenir.Add(souv);
            statHolder.ListSouvenir.Add(souv);
            menuStatMng.Equiped(souv);
        }
        int modifiedBaseValue = testCase.GetBase(statTmpHolder);
        int modifiedTotalValue = testCase.Get(statTmpHolder);
        Assert.That(modifiedBaseValue, Is.EqualTo(testCase.ExpectedBaseValue),
            $"Erreur sur {testCase.Name} : attendu {testCase.ExpectedBaseValue}, obtenu (Base : {modifiedBaseValue}, Total : {modifiedTotalValue}");
        Assert.That(modifiedTotalValue, Is.EqualTo(testCase.ExpectedValue),
            $"Erreur sur {testCase.Name} : attendu {testCase.ExpectedValue}, obtenu (Base : {modifiedBaseValue}, Total : {modifiedTotalValue}");
        statTmpHolder.ListSouvenir.Clear();
        statHolder.ListSouvenir.Clear();
    }
}
