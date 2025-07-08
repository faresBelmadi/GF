using NUnit.Framework;
using System;
using UnityEngine;

public partial class StatsTests
{

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
    #region TEST STATSHOLDER
    [Test]
    public void ConstructorStatsHolder()
    {
        var baseStat = CreateStatSO(0);
        
        var statHolder = new StatsHolder(baseStat);
        
        Assert.That(statHolder.Calme, Is.EqualTo(baseStat.Calme));
        Assert.That(statHolder.Conviction, Is.EqualTo(baseStat.Conviction));
        Assert.That(statHolder.Resilience, Is.EqualTo(baseStat.Resilience));
        Assert.That(statHolder.Essence, Is.EqualTo(baseStat.Essence));
        Assert.That(statHolder.ForceAme, Is.EqualTo(baseStat.ForceAme));
        Assert.That(statHolder.Radiance, Is.EqualTo(baseStat.Radiance));
        Assert.That(statHolder.RadianceMax, Is.EqualTo(baseStat.RadianceMax));

        Assert.That(statHolder.BaseCalme, Is.EqualTo(baseStat.Calme));
        Assert.That(statHolder.BaseConviction, Is.EqualTo(baseStat.Conviction));
        Assert.That(statHolder.BaseResilience, Is.EqualTo(baseStat.Resilience));
        Assert.That(statHolder.BaseForceDame, Is.EqualTo(baseStat.ForceAme));
        Assert.That(statHolder.BaseRadianceMax, Is.EqualTo(baseStat.RadianceMax));
    }
    [Test]
    public void UpdateStatsHolder()
    {
        int baseValue = 0;
        var baseStat = CreateStatSO(baseValue);
        var modifStat = CreateStatSO(10);

        var statHolder = new StatsHolder(baseStat);
        statHolder.UpdateStat(modifStat);

        Assert.That(statHolder.Calme, Is.EqualTo(10));
        Assert.That(statHolder.Conviction, Is.EqualTo(10));
        Assert.That(statHolder.Resilience, Is.EqualTo(10));
        Assert.That(statHolder.Essence, Is.EqualTo(10));
        Assert.That(statHolder.ForceAme, Is.EqualTo(10));
        Assert.That(statHolder.Radiance, Is.EqualTo(10));
        Assert.That(statHolder.RadianceMax, Is.EqualTo(10));

        Assert.That(statHolder.Calme, Is.Not.EqualTo(statHolder.BaseCalme));
        Assert.That(statHolder.Conviction, Is.Not.EqualTo(statHolder.BaseConviction));
        Assert.That(statHolder.Resilience, Is.Not.EqualTo(statHolder.BaseResilience));
        Assert.That(statHolder.ForceAme, Is.Not.EqualTo(statHolder.BaseForceDame));
        Assert.That(statHolder.RadianceMax, Is.Not.EqualTo(statHolder.BaseRadianceMax));

        Assert.That(statHolder.BaseCalme, Is.EqualTo(baseValue));
        Assert.That(statHolder.BaseConviction, Is.EqualTo(baseValue));
        Assert.That(statHolder.BaseResilience, Is.EqualTo(baseValue));
        Assert.That(statHolder.BaseForceDame, Is.EqualTo(baseValue));
        Assert.That(statHolder.BaseRadianceMax, Is.EqualTo(baseValue));
    }
    [Test]
    public void ResetStatHolder()
    {
        int baseValue = 0;
        var baseStat = CreateStatSO(baseValue);
        var modifStat = CreateStatSO(10);

        var statHolder = new StatsHolder(baseStat);
        statHolder.UpdateStat(modifStat);
        statHolder.ResetStat();
        Assert.That(statHolder.Calme, Is.EqualTo(baseValue));
        Assert.That(statHolder.Conviction, Is.EqualTo(baseValue));
        Assert.That(statHolder.Resilience, Is.EqualTo(baseValue));
        Assert.That(statHolder.ForceAme, Is.EqualTo(baseValue));
        Assert.That(statHolder.Radiance, Is.EqualTo(baseValue));
        Assert.That(statHolder.RadianceMax, Is.EqualTo(baseValue));

       
    }
            
    #endregion
}
