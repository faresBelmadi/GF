using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public partial class StatsTests
{


    public static JoueurStat CreatePlayerStatSO(int initialValue, int volonte, int volonteMax)
    {
        JoueurStat stat = ScriptableObject.CreateInstance<JoueurStat>();

        stat.Calme = initialValue;
        stat.Conviction = initialValue;
        stat.Resilience = initialValue;
        stat.Essence = initialValue;
        stat.ForceAme = initialValue;
        stat.Radiance = initialValue;
        stat.RadianceMax = initialValue;
        stat.Vitesse = initialValue;

        stat.Volonter = volonte;
        stat.Conscience = initialValue;
        stat.ConscienceMax = initialValue;
        stat.Clairvoyance = initialValue;
        stat.VolonterMax = volonteMax;

        stat.SlotsSouvenir = 10;
        return stat;
    }
  //  [Test]
    public void ConstructorPlayerStatsHandler()
    {
        var baseStat = CreatePlayerStatSO(0, 3, 5);

        var statHolder = new PlayerStatsHandler(baseStat);

        Assert.That(statHolder.CalmeTotal, Is.EqualTo(baseStat.Calme));
        Assert.That(statHolder.ConvictionTotal, Is.EqualTo(baseStat.Conviction));
        Assert.That(statHolder.ResilienceTotal, Is.EqualTo(baseStat.Resilience));
        Assert.That(statHolder.Essence, Is.EqualTo(baseStat.Essence));
        Assert.That(statHolder.ForceDameTotal, Is.EqualTo(baseStat.ForceAme));
        Assert.That(statHolder.Radiance, Is.EqualTo(baseStat.Radiance));
        Assert.That(statHolder.RadianceMaxTotal, Is.EqualTo(baseStat.RadianceMax));

        Assert.That(statHolder.Volonte, Is.EqualTo(baseStat.Volonter));
        Assert.That(statHolder.Conscience, Is.EqualTo(baseStat.Conscience));
        Assert.That(statHolder.ConscienceMax, Is.EqualTo(baseStat.ConscienceMax));
        Assert.That(statHolder.Clairvoyance, Is.EqualTo(baseStat.ClairvoyanceOriginal));
        Assert.That(statHolder.VolonteMax, Is.EqualTo(baseStat.VolonterMax));

        Assert.That(statHolder.BaseCalme, Is.EqualTo(baseStat.Calme));
        Assert.That(statHolder.BaseConviction, Is.EqualTo(baseStat.Conviction));
        Assert.That(statHolder.BaseResilience, Is.EqualTo(baseStat.Resilience));
        Assert.That(statHolder.BaseForceDame, Is.EqualTo(baseStat.ForceAme));
        Assert.That(statHolder.BaseRadianceMax, Is.EqualTo(baseStat.RadianceMax));

        Assert.That(statHolder.BaseConscienceMax, Is.EqualTo(baseStat.ConscienceMax));
        Assert.That(statHolder.BaseClairvoyance, Is.EqualTo(baseStat.ClairvoyanceOriginal));
        
    }

  //  [Test]
    public void UpdatePlayerStatsHandler()
    {
        int baseValue = 0;
        var baseStat = CreatePlayerStatSO(baseValue, 3, 5);
        var modifStat = CreatePlayerStatSO(10, 0, 5);

        var statHolder = new PlayerStatsHandler(baseStat);
        var obj = GameObject.FindObjectOfType<GameManager>();
        GameObject go = GameObject.Instantiate(obj.gameObject);
        GameManager gm = go.GetComponent<GameManager>();
        GameManager.Instance = gm;
        statHolder.UpdateStat(modifStat);
        GameObject.DestroyImmediate(go);

        Assert.That(statHolder.CalmeTotal, Is.EqualTo(10));
        Assert.That(statHolder.ConvictionTotal, Is.EqualTo(10));
        Assert.That(statHolder.ResilienceTotal, Is.EqualTo(10));
        Assert.That(statHolder.Essence, Is.EqualTo(10));
        Assert.That(statHolder.ForceDameTotal, Is.EqualTo(10));
        Assert.That(statHolder.Radiance, Is.EqualTo(10));
        Assert.That(statHolder.RadianceMaxTotal, Is.EqualTo(10));

        Assert.That(statHolder.CalmeTotal, Is.Not.EqualTo(statHolder.BaseCalme));
        Assert.That(statHolder.ConvictionTotal, Is.Not.EqualTo(statHolder.BaseConviction));
        Assert.That(statHolder.ResilienceTotal, Is.Not.EqualTo(statHolder.BaseResilience));
        Assert.That(statHolder.ForceDameTotal, Is.Not.EqualTo(statHolder.BaseForceDame));
        Assert.That(statHolder.RadianceMaxTotal, Is.Not.EqualTo(statHolder.BaseRadianceMax));

        Assert.That(statHolder.BaseCalme, Is.EqualTo(baseValue));
        Assert.That(statHolder.BaseConviction, Is.EqualTo(baseValue));
        Assert.That(statHolder.BaseResilience, Is.EqualTo(baseValue));
        Assert.That(statHolder.BaseForceDame, Is.EqualTo(baseValue));
        Assert.That(statHolder.BaseRadianceMax, Is.EqualTo(baseValue));

        Assert.That(statHolder.Volonte, Is.EqualTo(baseStat.Volonter));
        Assert.That(statHolder.Conscience, Is.EqualTo(10));
        Assert.That(statHolder.Clairvoyance, Is.EqualTo(10));
        Assert.That(statHolder.VolonteMax, Is.EqualTo(10));
    }

  //  [Test]
    public void UpdateBasePlayerStatsHandler()
    {
        int baseValue = 5;
        var baseStat = CreatePlayerStatSO(baseValue, 3, 5);
        var modifStat =  ScriptableObject.CreateInstance<JoueurStat>();

        modifStat.ForceAme = 5;

        var statHolder = new PlayerStatsHandler(baseStat);
        var obj = GameObject.FindObjectOfType<GameManager>();
        GameObject go = GameObject.Instantiate(obj.gameObject);
        GameManager gm = go.GetComponent<GameManager>();
        GameManager.Instance = gm;

        statHolder.UpdateBaseStat(modifStat);
        GameObject.DestroyImmediate(go);

        Assert.That(statHolder.ForceDameTotal, Is.EqualTo(10));
        Assert.That(statHolder.BaseForceDame, Is.EqualTo(10));
    }

  //  [Test]
    public void PercentStatHolder()
    {
        int baseValue = 10;
        var baseStat = CreatePlayerStatSO(baseValue, 3, 5);
        var statHolder = new PlayerStatsHandler(baseStat);
        
        Assert.That(statHolder.GetPercentValue(StatEnum.ForceDame, 50f), Is.EqualTo(5f));
        Assert.That(statHolder.GetPercentValue(StatEnum.ForceDame, 25f), Is.EqualTo(2.5f));
        Assert.That(statHolder.GetPercentValue(StatEnum.Volonte, 25f), Is.EqualTo(0f));

    }
   // [Test]
    public void TestForceDame()
    {
        int baseValue = 10;
        var baseStat = CreatePlayerStatSO(baseValue, 3, 5);
        var statHolder = new PlayerStatsHandler(baseStat);

        var obj = GameObject.FindObjectOfType<GameManager>();
        GameObject go = GameObject.Instantiate(obj.gameObject);
        GameManager gm = go.GetComponent<GameManager>();
        GameManager.Instance = gm;

        var modifStat = ScriptableObject.CreateInstance<JoueurStat>();

        modifStat.ForceAme = 10;
        Assert.That(statHolder.ForceDameTotal, Is.EqualTo(10));
        statHolder.UpdateStat(modifStat);
        Assert.That(statHolder.ForceDameTotal, Is.EqualTo(20));
        
    } 
    static PlayerStatsHandler statHolder;
        private static void InitStats(int baseValue)
        {

            var baseStat = StatsTests.CreatePlayerStatSO(baseValue, 3, 5);
            statHolder = new PlayerStatsHandler(baseStat);

            var obj = GameObject.FindObjectOfType<GameManager>();
            GameObject go = GameObject.Instantiate(obj.gameObject);
            GameManager gm = go.GetComponent<GameManager>();
            GameManager.Instance = gm;
        }

    
    public static void TestCreateStat(int baseValue, int expected, Func<PlayerStatsHandler, int> getStat)
    {
        InitStats(baseValue);
        Assert.That(getStat(statHolder), Is.EqualTo(expected));
    }
    public static void TestAddStat(int baseValue, JoueurStat statModifier, int expected, Func<PlayerStatsHandler, int> getStat)
    {
        InitStats(baseValue);
        statHolder.UpdateStat(statModifier);
        Assert.That(getStat(statHolder), Is.EqualTo(expected));
    }

    public class VitesseStat
    {
       

        [TestCase(10,10)]
        [TestCase(-10, -10)]
        [TestCase(0, 0)]
        [TestCase(100000, 100000)]
        public void TestCreateVitesse(int baseValue, int expected) => TestCreateStat(baseValue, expected, f => f.VitesseTotal);
       

        [TestCase(10, 10, 20)]
        [TestCase(10, 0, 10)]
        [TestCase(0, 10, 10)]
        [TestCase(10, -10, 0)]
        [TestCase(-10, 10, 0)]
        [TestCase(-10, -10, -20)]
        [TestCase(100000, 1, 100001)]
        public void TestAddVitesse(int baseValue, int modifier, int expected)
        {
            var modifStat = ScriptableObject.CreateInstance<JoueurStat>();

            modifStat.Vitesse = modifier;
            TestAddStat(baseValue, modifStat, expected, f => f.VitesseTotal);
            
        }

        [Test]
        public void TestBaseVitesse()
        {
            InitStats(10);
            var modifStat = ScriptableObject.CreateInstance<JoueurStat>();
            
            modifStat.Vitesse = 5;
            statHolder.UpdateBaseStat(modifStat);
            Assert.That(statHolder.BaseVitesse, Is.EqualTo(15));
        }
    }  
  
    public class ForceDameStat
    {
        PlayerStatsHandler statHolder;
        private void InitStats(int baseValue)
        {

            var baseStat = StatsTests.CreatePlayerStatSO(baseValue, 3, 5);
            statHolder = new PlayerStatsHandler(baseStat);

            var obj = GameObject.FindObjectOfType<GameManager>();
            GameObject go = GameObject.Instantiate(obj.gameObject);
            GameManager gm = go.GetComponent<GameManager>();
            GameManager.Instance = gm;
        }
        [TestCase(10, 10)]
        [TestCase(-10, -10)]
        [TestCase(0, 0)]
        [TestCase(100000, 100000)]
        public void TestCreateForceDame(int baseValue, int expected)
        {
            InitStats(baseValue);
            Assert.That(statHolder.ForceDameTotal, Is.EqualTo(expected));
        }

        [TestCase(10, 10, 20)]
        [TestCase(10, 0, 10)]
        [TestCase(0, 10, 10)]
        [TestCase(10, -10, 0)]
        [TestCase(-10, 10, 0)]
        [TestCase(-10, -10, -20)]
        [TestCase(100000, 1, 100001)]
        public void TestAddForceDame(int baseValue, int modifier, int expected)
        {
            InitStats(baseValue);
            var modifStat = ScriptableObject.CreateInstance<JoueurStat>();

            modifStat.ForceAme = modifier;
            statHolder.UpdateStat(modifStat);
            Assert.That(statHolder.ForceDameTotal, Is.EqualTo(expected));
        }

        [Test]
        public void TestBaseForceDame()
        {
            InitStats(10);
            var modifStat = ScriptableObject.CreateInstance<JoueurStat>();

            modifStat.ForceAme = 5;
            statHolder.UpdateBaseStat(modifStat);
            Assert.That(statHolder.ForceDameTotal, Is.EqualTo(15));
        }
    }

    public class ResilienceStat
    {
        PlayerStatsHandler statHolder;
        private void InitStats(int baseValue)
        {
            var baseStat = StatsTests.CreatePlayerStatSO(baseValue, 3, 5);
            statHolder = new PlayerStatsHandler(baseStat);

            var obj = GameObject.FindObjectOfType<GameManager>();
            GameObject go = GameObject.Instantiate(obj.gameObject);
            GameManager gm = go.GetComponent<GameManager>();
            GameManager.Instance = gm;
        }

        [TestCase(10, 10)]
        [TestCase(-10, -10)]
        [TestCase(0, 0)]
        [TestCase(100000, 100000)]
        public void TestCreateForceDame(int baseValue, int expected)
        {
            InitStats(baseValue);
            Assert.That(statHolder.Clairvoyance, Is.EqualTo(expected));
        }

        [TestCase(10, 10, 20)]
        [TestCase(10, 0, 10)]
        [TestCase(0, 10, 10)]
        [TestCase(10, -10, 0)]
        [TestCase(-10, 10, 0)]
        [TestCase(-10, -10, -20)]
        [TestCase(100000, 1, 100001)]
        public void TestAddForceDame(int baseValue, int modifier, int expected)
        {
            InitStats(baseValue);
            var modifStat = ScriptableObject.CreateInstance<JoueurStat>();

            modifStat.ForceAme = modifier;
            statHolder.UpdateStat(modifStat);
            Assert.That(statHolder.ForceDameTotal, Is.EqualTo(expected));
        }

        [Test]
        public void TestBaseForceDame()
        {
            InitStats(10);
            var modifStat = ScriptableObject.CreateInstance<JoueurStat>();

            modifStat.ForceAme = 5;
            statHolder.UpdateBaseStat(modifStat);
            Assert.That(statHolder.ForceDameTotal, Is.EqualTo(15));
        }
    }
}
