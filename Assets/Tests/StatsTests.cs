using NUnit.Framework;
using UnityEngine;

public class StatsTests
{

    public CharacterStat CreateStat(int initialValue)
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

    [Test]
    public void ModifStatUp()
    {
        CharacterStat stat = CreateStat(0);
        CharacterStat modifStat = CreateStat(10);
        

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
        CharacterStat stat = CreateStat(0);
        CharacterStat modifStat = CreateStat(- 20);


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
        CharacterStat stat = CreateStat(0);
        CharacterStat modifStat = CreateStat(-10);


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
        CharacterStat stat = CreateStat(0);
        CharacterStat modifStat = CreateStat(20);


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
        CharacterStat stat = CreateStat(0);

        stat.ResetStat();

        Assert.AreEqual(stat.ForceAme, 0);
        Assert.AreEqual(stat.Calme, 0);
        Assert.AreEqual(stat.Conviction, 0);
        Assert.AreEqual(stat.Resilience, 0);
        Assert.AreEqual(stat.Essence, 0);
        Assert.AreEqual(stat.Radiance, 0);
        Assert.AreEqual(stat.RadianceMax, -0);
    }

}
