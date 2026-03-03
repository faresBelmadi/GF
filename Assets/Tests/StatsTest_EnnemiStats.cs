using NUnit.Framework;
using UnityEngine;

public partial class StatsTests
{
    public static EnnemiStat CreateEnnemiStatSO(int initialValue, int radiance, int radianceMax, int dissimulation)
    {
        EnnemiStat stat = ScriptableObject.CreateInstance<EnnemiStat>();

        stat.Calme = initialValue;
        stat.Conviction = initialValue;
        stat.Resilience = initialValue;
        stat.Essence = initialValue;
        stat.ForceAme = initialValue;
        stat.Radiance = radiance;
        stat.RadianceMax = radianceMax;
        stat.Vitesse = initialValue;

        stat.Dissimulation = dissimulation;
        return stat;
    }
    [Test]
    public void ConstructorEnnemiStatsHandler()
    {
        var baseStat = CreateEnnemiStatSO(5, 200, 500, 4);

        var statHolder = new EnemyStatsHandler(baseStat);

        Assert.That(statHolder.CalmeTotal, Is.EqualTo(baseStat.Calme));
        Assert.That(statHolder.ConvictionTotal, Is.EqualTo(baseStat.Conviction));
        Assert.That(statHolder.ResilienceTotal, Is.EqualTo(baseStat.Resilience));
        Assert.That(statHolder.Essence, Is.EqualTo(baseStat.Essence));
        Assert.That(statHolder.ForceDameTotal, Is.EqualTo(baseStat.ForceAme));
        Assert.That(statHolder.Radiance, Is.EqualTo(baseStat.Radiance));
        Assert.That(statHolder.RadianceMaxTotal, Is.EqualTo(baseStat.RadianceMax));

        Assert.That(statHolder.CalmeTotal, Is.EqualTo(5));
        Assert.That(statHolder.ConvictionTotal, Is.EqualTo(5));
        Assert.That(statHolder.ResilienceTotal, Is.EqualTo(5));
        Assert.That(statHolder.Essence, Is.EqualTo(5));
        Assert.That(statHolder.ForceDameTotal, Is.EqualTo(5));
        Assert.That(statHolder.Radiance, Is.EqualTo(200));
        Assert.That(statHolder.RadianceMaxTotal, Is.EqualTo(200));

        Assert.That(statHolder.Dissimulation, Is.EqualTo(baseStat.Dissimulation));
        Assert.That(statHolder.Dissimulation, Is.EqualTo(4));

        Assert.That(statHolder.BaseCalme, Is.EqualTo(baseStat.Calme));
        Assert.That(statHolder.BaseConviction, Is.EqualTo(baseStat.Conviction));
        Assert.That(statHolder.BaseResilience, Is.EqualTo(baseStat.Resilience));
        Assert.That(statHolder.BaseForceDame, Is.EqualTo(baseStat.ForceAme));
        Assert.That(statHolder.BaseRadianceMax, Is.EqualTo(baseStat.RadianceMax));

    }
    [TestCase(50, 200, 500, 250)]
    [TestCase(0, 200, 500, 200)]
    [TestCase(200, 200, 500, 400)]
    [TestCase(2000, 200, 500, 500)]
    public void TestHealEnemy(int amount, int initialRad, int radMax, int expected)
    {
        var baseStat = CreateEnnemiStatSO(5, initialRad, radMax, 4);

        var statHolder = new EnemyStatsHandler(baseStat);

        EnnemiStat modif = ScriptableObject.CreateInstance<EnnemiStat>();
        modif.Radiance = amount;

        statHolder.UpdateStat(modif);

        
        Assert.That(statHolder.Radiance, Is.EqualTo(expected));
        Assert.That(statHolder.RadianceMaxTotal, Is.EqualTo(radMax));
    }
}
