
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEditor;
using UnityEngine;

public partial class StatsTests
{
    public class MemoryTestCase
    {
        public string Name;
        public int InitialValue;
        public int ExpectedValue;
        public Func<PlayerStatsHandler, int> Get;
        public Func<PlayerStatsHandler, int> GetBase;
        public List<ModificationStatSouvenir> ModifStatList;
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
    private static (string name, int initialValue, int expectedValue, Func<PlayerStatsHandler, int> get, Func<PlayerStatsHandler, int> getBase, List<ModificationStatSouvenir> modifStatList)[] AllMemoryTestCase =
        {
            ("Radiance Max", 100, 110,f => f.RadianceMaxTotal, f => f.BaseRadianceMax, new List<ModificationStatSouvenir>{ new ModificationStatSouvenir
            {
                StatModif = StatModif.RadianceMax,
                ParametreModifStat = new ParametreModifStat { ParametreStat = ParametreStat.ValeurBrut, Valeur = 10 }
            } } ),
            ("Force D'ame",100, 110, f => f.ForceDameTotal, f => f.BaseForceDame,  new List<ModificationStatSouvenir>{ new ModificationStatSouvenir
            {
                StatModif = StatModif.ForceAme,
                ParametreModifStat = new ParametreModifStat { ParametreStat = ParametreStat.ValeurBrut, Valeur = 10 }
            } } ),
            ("Vitesse",100, 110, f => f.VitesseTotal, f => f.BaseVitesse, new List<ModificationStatSouvenir>{ new ModificationStatSouvenir
            {
                StatModif = StatModif.Vitesse,
                ParametreModifStat = new ParametreModifStat { ParametreStat = ParametreStat.ValeurBrut, Valeur = 10 }
            } } ),
            ("Conviction",5, 10, f => f.ConvictionTotal, f => f.BaseConviction,  new List<ModificationStatSouvenir>{ new ModificationStatSouvenir
            {
                StatModif = StatModif.Conviction,
                ParametreModifStat = new ParametreModifStat { ParametreStat = ParametreStat.ValeurBrut, Valeur = 5 }
            } } ),
            ("Calme",100, 110, f => f.CalmeTotal, f => f.BaseCalme,  new List<ModificationStatSouvenir>{ new ModificationStatSouvenir
            {
                StatModif = StatModif.Calme,
                ParametreModifStat = new ParametreModifStat { ParametreStat = ParametreStat.ValeurBrut, Valeur = 10 }
            } } ),
            ("Resilience",5, 10, f => f.ResilienceTotal, f => f.BaseResilience,  new List<ModificationStatSouvenir>{ new ModificationStatSouvenir
            {
                StatModif = StatModif.Resilience,
                ParametreModifStat = new ParametreModifStat { ParametreStat = ParametreStat.ValeurBrut, Valeur = 5 }
            } } ),
            ("ConscienceMax",100, 110, f => f.ConscienceMaxTotal, f => f.BaseConscienceMax,  new List<ModificationStatSouvenir>{ new ModificationStatSouvenir
            {
                StatModif = StatModif.ConscienceMax,
                ParametreModifStat = new ParametreModifStat { ParametreStat = ParametreStat.ValeurBrut, Valeur = 10 }
            } }),
            ("Clairvoyance",100, 110, f => f.ClairvoyanceTotal, f => f.BaseClairvoyance,  new List<ModificationStatSouvenir>{ new ModificationStatSouvenir
            {
                StatModif = StatModif.Clairvoyance,
                ParametreModifStat = new ParametreModifStat { ParametreStat = ParametreStat.ValeurBrut, Valeur = 10 }
            } } ),
            ("ForceD'ame Mult",100, 160, f => f.ForceDameTotal, f => f.BaseForceDame,  new List<ModificationStatSouvenir>
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

            } ),

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
    public void EquipMemory(MemoryTestCase testCase)
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

        Souvenir souv = CreateSouvenir(testCase.ModifStatList);
        
        menuStatMng.Equiped(souv);
        int modifiedBaseValue = testCase.GetBase(statTmpHolder);
        int modifiedTotalValue = testCase.Get(statTmpHolder);
        Assert.That(modifiedBaseValue, Is.EqualTo(testCase.ExpectedValue),
            $"Erreur sur {testCase.Name} : attendu {testCase.ExpectedValue}, obtenu (Base : {modifiedBaseValue}, Total : {modifiedTotalValue}");
        Assert.That(modifiedTotalValue, Is.EqualTo(testCase.ExpectedValue),
            $"Erreur sur {testCase.Name} : attendu {testCase.ExpectedValue}, obtenu (Base : {modifiedBaseValue}, Total : {modifiedTotalValue}");
    }
}
