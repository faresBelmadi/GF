using NUnit.Framework;
using System;
using UnityEngine;

public partial class StatsTests
{
    PlayerStatsHandler statToTest;
    public CharacterStat CreateStatSO(int initialValue)
    {
        CharacterStat stat = ScriptableObject.CreateInstance<CharacterStat>();

        stat.Calme = initialValue;
        stat.Conviction = initialValue;
        stat.Resilience = initialValue;
        stat.Essence = initialValue;
        stat.ForceAme = initialValue;
        stat.Radiance = initialValue;
        stat.RadianceMax = initialValue;


        return stat;
    }
    private void InitStat(int baseValue)
    {

        var baseStat = StatsTests.CreatePlayerStatSO(baseValue, 3, 5);
        statToTest = new PlayerStatsHandler(baseStat);

        var obj = GameObject.FindObjectOfType<GameManager>();
        GameObject go = GameObject.Instantiate(obj.gameObject);
        GameManager gm = go.GetComponent<GameManager>();
        GameManager.Instance = gm;
    }
    #region TEST SO
    [Test]
    public void ModifStatUp()
    {
        CharacterStat stat = CreateStatSO(0);
        CharacterStat modifStat = CreateStatSO(10);
        

        stat.ModifStateAll(modifStat);

        Assert.AreEqual(stat.ForceAme, 10);
        Assert.AreEqual(stat.Calme, 10);
        Assert.AreEqual(stat.Conviction, 10);
        Assert.AreEqual(stat.Resilience, 10);
        Assert.AreEqual(stat.Essence, 10);
        Assert.AreEqual(stat.Radiance, 10);
        Assert.AreEqual(stat.RadianceMax, 10);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         
    }
    [Test]
    public void ModifStatDown()
    {
        CharacterStat stat = CreateStatSO(0);
        CharacterStat modifStat = CreateStatSO(- 20);


        stat.ModifStateAll(modifStat);

        Assert.AreEqual(stat.ForceAme, -20);
        Assert.AreEqual(stat.Calme, -20);
        Assert.AreEqual(stat.Conviction, -20);
        Assert.AreEqual(stat.Resilience, -20);
        Assert.AreEqual(stat.Essence, -20);
        Assert.AreEqual(stat.Radiance, -20);
        Assert.AreEqual(stat.RadianceMax, -20);
    }
    [Test]
    public void RemoveStatUp()
    {
        CharacterStat stat = CreateStatSO(0);
        CharacterStat modifStat = CreateStatSO(-10);


        stat.removeStat(modifStat);

        Assert.AreEqual(stat.ForceAme, 10);
        Assert.AreEqual(stat.Calme, 10);
        Assert.AreEqual(stat.Conviction, 10);
        Assert.AreEqual(stat.Resilience, 10);
        Assert.AreEqual(stat.Essence, 10);
        Assert.AreEqual(stat.Radiance, 10);
        Assert.AreEqual(stat.RadianceMax, 10);
    }
    [Test]
    public void RemoveStatDown()
    {
        CharacterStat stat = CreateStatSO(0);
        CharacterStat modifStat = CreateStatSO(20);


        stat.removeStat(modifStat);

        Assert.AreEqual(stat.ForceAme, -20);
        Assert.AreEqual(stat.Calme, -20);
        Assert.AreEqual(stat.Conviction, -20);
        Assert.AreEqual(stat.Resilience, -20);
        Assert.AreEqual(stat.Essence, -20);
        Assert.AreEqual(stat.Radiance, -20);
        Assert.AreEqual(stat.RadianceMax, -20);
    }

    [Test]
    public void ResetStat()
    {
        CharacterStat stat = CreateStatSO(0);

        stat.ResetStat();

        Assert.AreEqual(stat.ForceAme, 0);
        Assert.AreEqual(stat.Calme, 0);
        Assert.AreEqual(stat.Conviction, 0);
        Assert.AreEqual(stat.Resilience, 0);
        Assert.AreEqual(stat.Essence, 0);
        Assert.AreEqual(stat.Radiance, 0);
        Assert.AreEqual(stat.RadianceMax, -0);
    }
    #endregion
    #region TEST StatsHandler
    [Test]
    public void ConstructorStatsHandler()
    {
        JoueurStat baseStat = (JoueurStat) CreateStatSO(0);
        
        var statHolder = new PlayerStatsHandler(baseStat);
        
        Assert.That(statHolder.CalmeTotal, Is.EqualTo(baseStat.Calme));
        Assert.That(statHolder.ConvictionTotal, Is.EqualTo(baseStat.Conviction));
        Assert.That(statHolder.ResilienceTotal, Is.EqualTo(baseStat.Resilience));
        Assert.That(statHolder.Essence, Is.EqualTo(baseStat.Essence));
        Assert.That(statHolder.ForceDameTotal, Is.EqualTo(baseStat.ForceAme));
        Assert.That(statHolder.Radiance, Is.EqualTo(baseStat.Radiance));
        Assert.That(statHolder.RadianceMaxTotal, Is.EqualTo(baseStat.RadianceMax));

        //Assert.That(statHolder.BaseCalme, Is.EqualTo(baseStat.Calme));
        //Assert.That(statHolder.BaseConviction, Is.EqualTo(baseStat.Conviction));
        //Assert.That(statHolder.BaseResilience, Is.EqualTo(baseStat.Resilience));
        //Assert.That(statHolder.BaseForceDame, Is.EqualTo(baseStat.ForceAme));
        //Assert.That(statHolder.BaseRadianceMax, Is.EqualTo(baseStat.RadianceMax));
    }
    [Test]
    public void UpdateStatsHandler()
    {
        int baseValue = 0;
        var baseStat = (JoueurStat)CreateStatSO(baseValue);
        var modifStat = CreateStatSO(10);

        PlayerStatsHandler statHolder = new PlayerStatsHandler(baseStat);
        statHolder.UpdateStat(modifStat);

        Assert.That(statHolder.CalmeTotal, Is.EqualTo(10));
        Assert.That(statHolder.ConvictionTotal, Is.EqualTo(10));
        Assert.That(statHolder.ResilienceTotal, Is.EqualTo(10));
        Assert.That(statHolder.Essence, Is.EqualTo(10));
        Assert.That(statHolder.ForceDameTotal, Is.EqualTo(10));
        Assert.That(statHolder.Radiance, Is.EqualTo(10));
        Assert.That(statHolder.RadianceMaxTotal  , Is.EqualTo(10));

        //Assert.That(statHolder.Calme, Is.Not.EqualTo(statHolder.BaseCalme));
        //Assert.That(statHolder.Conviction, Is.Not.EqualTo(statHolder.BaseConviction));
        //Assert.That(statHolder.Resilience, Is.Not.EqualTo(statHolder.BaseResilience));
        //Assert.That(statHolder.ForceAme, Is.Not.EqualTo(statHolder.BaseForceDame));
        //Assert.That(statHolder.RadianceMax, Is.Not.EqualTo(statHolder.BaseRadianceMax));

        //Assert.That(statHolder.BaseCalme, Is.EqualTo(baseValue));
        //Assert.That(statHolder.BaseConviction, Is.EqualTo(baseValue));
        //Assert.That(statHolder.BaseResilience, Is.EqualTo(baseValue));
        //Assert.That(statHolder.BaseForceDame, Is.EqualTo(baseValue));
        //Assert.That(statHolder.BaseRadianceMax, Is.EqualTo(baseValue));
    }
   // [Test]
    public void ResetStatHolder()
    {
        int baseValue = 0;
        var baseStat = (JoueurStat)CreateStatSO(baseValue);
        var modifStat = CreateStatSO(10);

        var statHolder = new PlayerStatsHandler(baseStat);
        statHolder.UpdateStat(modifStat);
        //statHolder.ResetStat();
        Assert.That(statHolder.CalmeTotal, Is.EqualTo(baseValue));
        Assert.That(statHolder.ConvictionTotal, Is.EqualTo(baseValue));
        Assert.That(statHolder.ResilienceTotal, Is.EqualTo(baseValue));
        Assert.That(statHolder.ForceDameTotal, Is.EqualTo(baseValue));
        Assert.That(statHolder.Radiance, Is.EqualTo(baseValue));
        Assert.That(statHolder.RadianceMaxTotal, Is.EqualTo(baseValue));

       
    }
    [Test]
    [TestCase(10, 0, 10)]
    [TestCase(0, 10, 10)]
    [TestCase(0, 20, 10)]
    [TestCase(1, 2, 3)]
    [TestCase(5, 2, 7)]
    [TestCase(5, -2, 3)]
    [TestCase(5, -5, 0)]
    [TestCase(3, -5, -2)]
    [TestCase(-3, -5, -8)]
    [TestCase(-3, -15, -10)]
    [TestCase(3, +15, 10)]
    [TestCase(-3, 0, -3)]
    [TestCase(-15, 0, -10)]
    [TestCase(13, 0, 10)]
    [TestCase(-13, 0, -10)]
    [TestCase(-3, 1, -2)]
    [TestCase(13, 1, 10)]
    [TestCase(13, -5, 8)]
    public void TestStatConviction (int baseValue, int modifier, int expected)
    {
        InitStat(baseValue);

        var modifStat = ScriptableObject.CreateInstance<JoueurStat>();
        modifStat.Conviction = modifier;

        statToTest.UpdateStat(modifStat);

        Assert.That(statToTest.ConvictionTotal, Is.EqualTo(expected));
    }
    [Test]
    [TestCase(10, 0, 10)]
    [TestCase(0, 10, 10)]
    [TestCase(0, 20, 10)]
    [TestCase(1, 2, 3)]
    [TestCase(5, 2, 7)]
    [TestCase(5, -2, 3)]
    [TestCase(5, -5, 0)]
    [TestCase(3, -5, -2)]
    [TestCase(-3, -5, -8)]
    [TestCase(-3, -15, -10)]
    [TestCase(3, +15, 10)]
    [TestCase(-3, 0, -3)]
    [TestCase(-15, 0, -10)]
    [TestCase(13, 0, 10)]
    [TestCase(-13, 0, -10)]
    [TestCase(-3, 1, -2)]
    [TestCase(13, 1, 10)]
    [TestCase(13, -5, 8)]
    public void TestStatResilience(int baseValue, int modifier, int expected)
    {
        InitStat(baseValue);

        var modifStat = ScriptableObject.CreateInstance<JoueurStat>();
        modifStat.Resilience = modifier;

        statToTest.UpdateStat(modifStat);

        Assert.That(statToTest.ResilienceTotal, Is.EqualTo(expected));
    }

    #endregion
}
