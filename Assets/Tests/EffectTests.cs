using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;

public class EffectTests
{
    private static string[] TestedJoueurStatFieldNames =
        {
            "Clairvoyance",
            "Conscience",
            "ConscienceMax",
            "Volonter",
            "VolonterMax",
        };

    private MemberInfo[] TestedJoueurStatMembers =>
        typeof(JoueurStat).GetMembers()
            .Where(f => TestedJoueurStatFieldNames.Contains(f.Name))
            .ToArray();

    [Test]
    public void EnsureTestedFieldsAreInJoueurStat()
    {
        var joueurStatFields = TestedJoueurStatMembers;
        Assert.AreEqual(TestedJoueurStatFieldNames.Length, joueurStatFields.Length,
            $"Those fields expected to be tested weren't found in {nameof(JoueurStat)}:\n{string.Join("; ", TestedJoueurStatFieldNames.Except(joueurStatFields.Select(jf => jf.Name)))}");
    }

    private static JoueurStat ExpectedDefaultJoueurStat
    {
        get
        {
            JoueurStat defaultJoueurStat = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
            defaultJoueurStat.Lvl = 0;
            defaultJoueurStat.Volonter = 0;
            defaultJoueurStat.VolonterMax = 0;
            defaultJoueurStat.Conscience = 0;
            defaultJoueurStat.Clairvoyance = 0;
            defaultJoueurStat.ClairvoyanceOriginal = 0;
            defaultJoueurStat.SlotsSouvenir = 0;
            return defaultJoueurStat;
        }
    }

    private bool AreEqual(Type castType, object expected, object actual)
    {
        // generic Equals doesn't work for basic types
        // you'll have to add specific casts manually if needed
        return castType == typeof(int) && (int)expected == (int)actual
            || Equals(expected, actual);
    }

    private bool AreIdentical(JoueurStat expected, JoueurStat actual, out string error)
    {
        error = null;
        foreach (var member in TestedJoueurStatMembers)
        {
            if (member is FieldInfo field)
            {
                var fex = field.GetValue(expected);
                var fac = field.GetValue(actual);

                if (!AreEqual(field.GetType(), fex, fac))
                {
                    error = $"Field {field.Name} expected value {fex} but is {fac} (type {field.GetType().Name})";
                    return false;
                }
            }
            else if (member is PropertyInfo property)
            {
                var pex = property.GetValue(expected);
                var pac = property.GetValue(actual);

                if (!AreEqual(property.GetType(), pex, pac))
                {
                    error = $"Property {property.Name} expected value {pex} but is {pac} (type {property.GetType().Name})";
                    return false;
                }
            }
            else
            {
                Debug.Log($"Member {member} is of unhandled type {member.GetType()}");
            }
        }
        return true;
    }

    [Test]
    public void EnsureDefaultJoueurStatValues()
    {
        var newJoueurStat = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
        Assert.IsTrue(AreIdentical(ExpectedDefaultJoueurStat, newJoueurStat, out string error), error);
    }

    private static int TestVariationCount = 3;

    /// <summary>
    /// Expected results are for:
    /// * ValeurBrut = 0, NbAttaque = 0
    /// * ValeurBrut = 5, NbAttaque = 1
    /// * ValeurBrut = 5, NbAttaque = 2
    /// 
    /// If you add a test variation, edit TestVariationCount accordingly!
    /// </summary>
    private static Dictionary<TypeEffet, Func<JoueurStat>[]> ExpectedResultsByEffect = new Dictionary<TypeEffet, Func<JoueurStat>[]>
        {
            {
                TypeEffet.Clairvoyance,
                new Func<JoueurStat>[] {
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Clairvoyance = 0;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Clairvoyance = 5;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Clairvoyance = 10;
                        return js;
                    },
                }
            },
            {
                TypeEffet.Volonte,
                new Func<JoueurStat>[] {
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Volonter = 0;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Volonter = 5;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Volonter = 10;
                        return js;
                    },
                }
            },
            {
                TypeEffet.VolonteMax,
                new Func<JoueurStat>[] {
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.VolonterMax = 0;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.VolonterMax = 5;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.VolonterMax = 10;
                        return js;
                    },
                }
            },
            {
                TypeEffet.Conscience,
                new Func<JoueurStat>[] {
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Conscience = 0;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Conscience = 5;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Conscience = 10;
                        return js;
                    },
                }
            },
            {
                TypeEffet.ConscienceMax,
                new Func<JoueurStat>[] {
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.ConscienceMax = 0;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.ConscienceMax = 5;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.ConscienceMax = 10;
                        return js;
                    },
                }
            },
        };

    [Test]
    public void EnsureExpectationDefinitions()
    {
        foreach (var expectation in ExpectedResultsByEffect)
        {
            Assert.AreEqual(expectation.Value.Length, TestVariationCount, $"Type {expectation.Key} has {expectation.Value.Length} variations defined, when {TestVariationCount} are expected.");
        }
    }


    private static TypeEffet[] GetValidateEffetSource() => Enum.GetValues(typeof(TypeEffet)).Cast<TypeEffet>().ToArray();

    [Test, TestCaseSource("GetValidateEffetSource")]
    public void ResultEffet_ValeurBrut0_NbAttaque0(TypeEffet typeEffet)
    {
        var expected = FindExpectedResult(typeEffet, 0);
        var effet = CreateEffet(typeEffet, valeurBrute: 0, nbAttaques: 0);
        TestResultEffet(effet, expected);
    }

    [Test, TestCaseSource("GetValidateEffetSource")]
    public void ResultEffet_ValeurBrut5_NbAttaque1(TypeEffet typeEffet)
    {
        var expected = FindExpectedResult(typeEffet, 1);
        var effet = CreateEffet(typeEffet, valeurBrute: 5, nbAttaques: 1);
        TestResultEffet(effet, expected);
    }

    [Test, TestCaseSource("GetValidateEffetSource")]
    public void ResultEffet_ValeurBrut5_NbAttaque2(TypeEffet typeEffet)
    {
        var expected = FindExpectedResult(typeEffet, 2);
        var effet = CreateEffet(typeEffet, valeurBrute: 5, nbAttaques: 2);
        TestResultEffet(effet, expected);
    }

    private JoueurStat FindExpectedResult(TypeEffet typeEffet, int testIndex)
    {
        bool hasExpectaton = ExpectedResultsByEffect.TryGetValue(typeEffet, out var expectations);
        Assert.IsTrue(hasExpectaton, $"Expectations for effet {typeEffet} are not defined.");

        Assert.Less(testIndex, expectations.Length, $"Not enough expectations defined for effet {typeEffet}");
        JoueurStat expected = expectations[testIndex].Invoke();

        return expected;
    }

    private Effet CreateEffet(TypeEffet typeEffet,
        int valeurBrute, int nbAttaques)
    {
        var effet = ScriptableObject.CreateInstance("Effet") as Effet;
        effet.TypeEffet = typeEffet;
        effet.ValeurBrut = valeurBrute;
        effet.NbAttaque = nbAttaques;
        return effet;
    }

    private void TestResultEffet(Effet effet, JoueurStat expected)
    {
        var caster = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
        var result = effet.ResultEffet(caster);
        Assert.IsTrue(AreIdentical(expected, result, out string error), error);
    }
}