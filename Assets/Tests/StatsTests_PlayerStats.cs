using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public partial class StatsTests
{


    public JoueurStat CreatePlayerStatSO(int initialValue, int volonte, int volonteMax)
    {
        JoueurStat stat = ScriptableObject.CreateInstance<JoueurStat>();

        stat.Calme = initialValue;
        stat.Conviction = initialValue;
        stat.Resilience = initialValue;
        stat.Essence = initialValue;
        stat.ForceAme = initialValue;
        stat.Radiance = initialValue;
        stat.RadianceMax = initialValue;

        stat.Volonter = volonte;
        stat.Conscience = initialValue;
        stat.ConscienceMax = initialValue;
        stat.Clairvoyance = initialValue;
        stat.VolonterMax = volonteMax;

        return stat;
    }
    [Test]
    public void ConstructorPlayerStatsHandler()
    {
        var baseStat = CreatePlayerStatSO(0, 3, 5);

        var statHolder = new PlayerStatsHandler(baseStat);

        Assert.That(statHolder.Calme, Is.EqualTo(baseStat.Calme));
        Assert.That(statHolder.Conviction, Is.EqualTo(baseStat.Conviction));
        Assert.That(statHolder.Resilience, Is.EqualTo(baseStat.Resilience));
        Assert.That(statHolder.Essence, Is.EqualTo(baseStat.Essence));
        Assert.That(statHolder.ForceAme, Is.EqualTo(baseStat.ForceAme));
        Assert.That(statHolder.Radiance, Is.EqualTo(baseStat.Radiance));
        Assert.That(statHolder.RadianceMax, Is.EqualTo(baseStat.RadianceMax));

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

    [Test]
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

        Assert.That(statHolder.Volonte, Is.EqualTo(baseStat.Volonter));
        Assert.That(statHolder.Conscience, Is.EqualTo(10));
        Assert.That(statHolder.Clairvoyance, Is.EqualTo(10));
        Assert.That(statHolder.VolonteMax, Is.EqualTo(10));
    }

    [Test]
    public void UpdateBasePlayerStatsHandler()
    {
        int baseValue = 5;
        var baseStat = CreatePlayerStatSO(baseValue, 3, 5);
        var modifStat =  ScriptableObject.CreateInstance<JoueurStat>();

        modifStat.ConscienceMax = 5;

        var statHolder = new PlayerStatsHandler(baseStat);
        var obj = GameObject.FindObjectOfType<GameManager>();
        GameObject go = GameObject.Instantiate(obj.gameObject);
        GameManager gm = go.GetComponent<GameManager>();
        GameManager.Instance = gm;

        statHolder.UpdateBaseStat(modifStat);
        GameObject.DestroyImmediate(go);

        Assert.That(statHolder.ConscienceMax, Is.EqualTo(10));
        Assert.That(statHolder.BaseConscienceMax, Is.EqualTo(10));
    }

    [Test]
    public void PercentStatHolder()
    {
        int baseValue = 10;
        var baseStat = CreatePlayerStatSO(baseValue, 3, 5);
        var statHolder = new PlayerStatsHandler(baseStat);
        
        Assert.That(statHolder.GetPercentValue(StatEnum.ForceDame, 50f), Is.EqualTo(5f));
        Assert.That(statHolder.GetPercentValue(StatEnum.ForceDame, 25f), Is.EqualTo(2.5f));
        Assert.That(statHolder.GetPercentValue(StatEnum.Volonte, 25f), Is.EqualTo(0f));

    }

}
