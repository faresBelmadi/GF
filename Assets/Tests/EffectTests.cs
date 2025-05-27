using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public partial class EffectTests
{
    private JoueurStat CreateSmallCaster()
    {
        var caster = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
        caster._forceAme = 1;
        caster.MultiplDegat = 1.5f;
        return caster;
    }

    private JoueurStat CreateMediumCaster()
    {
        var caster = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
        caster._forceAme = 5;
        caster.MultiplDegat = 2f;
        return caster;
    }

    private static TypeEffet[] GetValidateEffetSource() => Enum.GetValues(typeof(TypeEffet)).Cast<TypeEffet>().OrderBy(x => x.ToString()).ToArray();

    [Test, TestCaseSource("GetValidateEffetSource")]
    [Tooltip("Pourcentage = 100, Cible = null")]
    public void ResultEffet_CasterDefault_ValeurBrut0_NbAttaque0(TypeEffet typeEffet)
    {
        var expected = FindExpectedResult(typeEffet, 0);
        var effet = CreateEffet(typeEffet, valeurBrute: 0, nbAttaques: 0, pourcentage: 100);
        var caster = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
        TestResultEffet(effet, caster, expected);
    }

    [Test, TestCaseSource("GetValidateEffetSource")]
    [Tooltip("Pourcentage = 100, Cible = null")]
    public void ResultEffet_CasterDefault_ValeurBrut5_NbAttaque1(TypeEffet typeEffet)
    {
        var expected = FindExpectedResult(typeEffet, 1);
        var effet = CreateEffet(typeEffet, valeurBrute: 5, nbAttaques: 1, pourcentage: 100);
        var caster = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
        TestResultEffet(effet, caster, expected);
    }

    [Test, TestCaseSource("GetValidateEffetSource")]
    [Tooltip("Pourcentage = 100, Cible = null")]
    public void ResultEffet_CasterDefault_ValeurBrut5_NbAttaque2(TypeEffet typeEffet)
    {
        var expected = FindExpectedResult(typeEffet, 2);
        var effet = CreateEffet(typeEffet, valeurBrute: 5, nbAttaques: 2, pourcentage: 100);
        var caster = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
        TestResultEffet(effet, caster, expected);
    }

    [Test, TestCaseSource("GetValidateEffetSource")]
    [Tooltip("Pourcentage = 100, Cible = null")]
    public void ResultEffet_CasterSmall_ValeurBrut0_NbAttaque0(TypeEffet typeEffet)
    {
        var expected = FindExpectedResult(typeEffet, 3);
        var effet = CreateEffet(typeEffet, valeurBrute: 0, nbAttaques: 0, pourcentage: 100);
        TestResultEffet(effet, CreateSmallCaster(), expected);
    }

    [Test, TestCaseSource("GetValidateEffetSource")]
    [Tooltip("Pourcentage = 100, Cible = null")]
    public void ResultEffet_CasterSmall_ValeurBrut5_NbAttaque1(TypeEffet typeEffet)
    {
        var expected = FindExpectedResult(typeEffet, 4);
        var effet = CreateEffet(typeEffet, valeurBrute: 5, nbAttaques: 1, pourcentage: 100);
        TestResultEffet(effet, CreateSmallCaster(), expected);
    }

    [Test, TestCaseSource("GetValidateEffetSource")]
    [Tooltip("Pourcentage = 100, Cible = null")]
    public void ResultEffet_CasterSmall_ValeurBrut5_NbAttaque2(TypeEffet typeEffet)
    {
        var expected = FindExpectedResult(typeEffet, 5);
        var effet = CreateEffet(typeEffet, valeurBrute: 5, nbAttaques: 2, pourcentage: 100);
        TestResultEffet(effet, CreateSmallCaster(), expected);
    }

    [Test, TestCaseSource("GetValidateEffetSource")]
    [Tooltip("Pourcentage = 100, Cible = null")]
    public void ResultEffet_CasterMedium_ValeurBrut0_NbAttaque0(TypeEffet typeEffet)
    {
        var expected = FindExpectedResult(typeEffet, 6);
        var effet = CreateEffet(typeEffet, valeurBrute: 0, nbAttaques: 0, pourcentage: 100);
        TestResultEffet(effet, CreateMediumCaster(), expected);
    }

    [Test, TestCaseSource("GetValidateEffetSource")]
    [Tooltip("Pourcentage = 100, Cible = null")]
    public void ResultEffet_CasterMedium_ValeurBrut5_NbAttaque1(TypeEffet typeEffet)
    {
        var expected = FindExpectedResult(typeEffet, 7);
        var effet = CreateEffet(typeEffet, valeurBrute: 5, nbAttaques: 1, pourcentage: 100);
        TestResultEffet(effet, CreateMediumCaster(), expected);
    }

    [Test, TestCaseSource("GetValidateEffetSource")]
    [Tooltip("Pourcentage = 100, Cible = null")]
    public void ResultEffet_CasterMedium_ValeurBrut5_NbAttaque2(TypeEffet typeEffet)
    {
        var expected = FindExpectedResult(typeEffet, 8);
        var effet = CreateEffet(typeEffet, valeurBrute: 5, nbAttaques: 2, pourcentage: 100);
        TestResultEffet(effet, CreateMediumCaster(), expected);
    }

    private object FindExpectedResult(TypeEffet typeEffet, int testIndex)
    {
        bool hasExpectaton = ExpectedResultsByEffect.TryGetValue(typeEffet, out var expectations);
        Assert.IsTrue(hasExpectaton, $"Expectations for effet {typeEffet} are not defined.");

        Assert.Less(testIndex, expectations.Length, $"Not enough expectations defined for effet {typeEffet}");
        object expected = expectations[testIndex].Invoke();

        return expected;
    }

    private Effet CreateEffet(TypeEffet typeEffet,
        int valeurBrute, int nbAttaques, int pourcentage)
    {
        var effet = ScriptableObject.CreateInstance("Effet") as Effet;
        effet.TypeEffet = typeEffet;
        effet.ValeurBrut = valeurBrute;
        effet.NbAttaque = nbAttaques;
        effet.Pourcentage = pourcentage;
        return effet;
    }

    private void TestResultEffet(Effet effet, JoueurStat caster, object expected)
    {
        if (expected is Exception expectedException)
        {
            Assert.Throws(expectedException.GetType(), () => effet.ResultEffet(caster));
        }
        else if (expected is JoueurStat expectedStat)
        {
            var result = effet.ResultEffet(caster);
            Assert.IsTrue(AreIdentical(expectedStat, result, out string error), error);
        }
    }
}