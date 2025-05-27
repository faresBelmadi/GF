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
            nameof(JoueurStat.Clairvoyance),
            nameof(JoueurStat.Conscience),
            nameof(JoueurStat.ConscienceMax),
            nameof(JoueurStat.Conviction),
            nameof(JoueurStat.Radiance),
            nameof(JoueurStat.Volonter),
            nameof(JoueurStat.VolonterMax),
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
            defaultJoueurStat.Calme = 0;
            defaultJoueurStat.Clairvoyance = 0;
            defaultJoueurStat.ClairvoyanceOriginal = 0;
            defaultJoueurStat.Conscience = 0;
            defaultJoueurStat.ConscienceMax = 0;
            defaultJoueurStat.Conviction = 0;
            defaultJoueurStat.ConvictionMin = -10;
            defaultJoueurStat.ConvictionMax = 10;
            defaultJoueurStat.ConvictionOriginal = 0;
            defaultJoueurStat.Essence = 0;
            defaultJoueurStat._forceAme = 0;
            defaultJoueurStat.ForceAmeBonus = 0;
            defaultJoueurStat.ForceAmeOriginal = 0;
            defaultJoueurStat.MultiplDef = 1;
            defaultJoueurStat.MultiplSoin = 1;
            defaultJoueurStat.MultiplDegat = 1;
            defaultJoueurStat.MultipleBuffDebuff = 1;
            defaultJoueurStat.MultipleTension = 1;
            defaultJoueurStat.Radiance = 0;
            defaultJoueurStat.RadianceMax = 0;
            defaultJoueurStat.RadianceMaxOriginal = 0;
            defaultJoueurStat.Resilience = 0;
            defaultJoueurStat.ResiliencePassif = 0;
            defaultJoueurStat.ResilienceMin = -10;
            defaultJoueurStat.ResilienceMax = 10;
            defaultJoueurStat.ResilienceOriginal = 0;
            defaultJoueurStat.SlotsSouvenir = 0;
            defaultJoueurStat.Vitesse = 0;
            defaultJoueurStat.VitesseOriginal = 0;
            defaultJoueurStat.Volonter = 0;
            defaultJoueurStat.VolonterMax = 0;
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

    private static int TestVariationCount = 9;

    /// <summary>
    /// Expected results are for:
    /// * Caster = default, ValeurBrut = 0, NbAttaque = 0, Pourcentage = 100
    /// * Caster = default, ValeurBrut = 5, NbAttaque = 1, Pourcentage = 100
    /// * Caster = default, ValeurBrut = 5, NbAttaque = 2, Pourcentage = 100
    /// * Caster = small,   ValeurBrut = 0, NbAttaque = 0, Pourcentage = 100
    /// * Caster = small,   ValeurBrut = 5, NbAttaque = 1, Pourcentage = 100
    /// * Caster = small,   ValeurBrut = 5, NbAttaque = 2, Pourcentage = 100
    /// * Caster = medium,  ValeurBrut = 0, NbAttaque = 0, Pourcentage = 100
    /// * Caster = medium,  ValeurBrut = 5, NbAttaque = 1, Pourcentage = 100
    /// * Caster = medium,  ValeurBrut = 5, NbAttaque = 2, Pourcentage = 100
    /// 
    /// If you add a test variation, edit TestVariationCount accordingly!
    /// </summary>
    private static Dictionary<TypeEffet, Func<object>[]> ExpectedResultsByEffect = new Dictionary<TypeEffet, Func<object>[]>
        {
            {
                TypeEffet.Clairvoyance, // Clairvoyance +=  ValeurBrut * NbAttaque
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
                TypeEffet.Volonte, // Volonte +=  ValeurBrut * NbAttaque
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
                TypeEffet.VolonteMax, // VolonteMax +=  ValeurBrut * NbAttaque
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
                TypeEffet.Conscience, // Conscience +=  ValeurBrut * NbAttaque
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
                TypeEffet.ConscienceMax, // ConscienceMax +=  ValeurBrut * NbAttaque
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
            {
                TypeEffet.DegatsForceAme, // Radiance += Pourcentage/100 * NbAttaque * caster.ForceAme * caster.MultiDegat
                new Func<JoueurStat>[] {
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Radiance = 0;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Radiance = 1;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Radiance = 3;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Radiance = 0;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Radiance = 10;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Radiance = 20;
                        return js;
                    },
                }
            },
            {
                TypeEffet.DegatsBrut, // Radiance += ValeurBrut * NbAttaque * caster.MultiDegat
                new Func<JoueurStat>[] {
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Radiance = 0;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Radiance = 5;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Radiance = 10;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Radiance = 0;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Radiance = 7;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Radiance = 15;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Radiance = 0;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Radiance = 10;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Radiance = 20;
                        return js;
                    },
                }
            },
            {
                TypeEffet.Conviction, // Conviction += ValeurBrut * NbAttaque
                new Func<JoueurStat>[] {
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Conviction = 0;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Conviction = 5;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Conviction = 10;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Conviction = 0;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Conviction = 5;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Conviction = 10;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Conviction = 0;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Conviction = 5;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Conviction = 10;
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
    [Tooltip("Pourcentage = 100")]
    public void ResultEffet_CasterDefault_ValeurBrut0_NbAttaque0(TypeEffet typeEffet)
    {
        var expected = FindExpectedResult(typeEffet, 0);
        var effet = CreateEffet(typeEffet, valeurBrute: 0, nbAttaques: 0, pourcentage: 100);
        var caster = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
        TestResultEffet(effet, caster, expected);
    }

    [Test, TestCaseSource("GetValidateEffetSource")]
    [Tooltip("Pourcentage = 100")]
    public void ResultEffet_CasterDefault_ValeurBrut5_NbAttaque1(TypeEffet typeEffet)
    {
        var expected = FindExpectedResult(typeEffet, 1);
        var effet = CreateEffet(typeEffet, valeurBrute: 5, nbAttaques: 1, pourcentage: 100);
        var caster = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
        TestResultEffet(effet, caster, expected);
    }

    [Test, TestCaseSource("GetValidateEffetSource")]
    [Tooltip("Pourcentage = 100")]
    public void ResultEffet_CasterDefault_ValeurBrut5_NbAttaque2(TypeEffet typeEffet)
    {
        var expected = FindExpectedResult(typeEffet, 2);
        var effet = CreateEffet(typeEffet, valeurBrute: 5, nbAttaques: 2, pourcentage: 100);
        var caster = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
        TestResultEffet(effet, caster, expected);
    }

    [Test, TestCaseSource("GetValidateEffetSource")]
    [Tooltip("Pourcentage = 100")]
    public void ResultEffet_CasterSmall_ValeurBrut0_NbAttaque0(TypeEffet typeEffet)
    {
        var expected = FindExpectedResult(typeEffet, 3);
        var effet = CreateEffet(typeEffet, valeurBrute: 0, nbAttaques: 0, pourcentage: 100);
        TestResultEffet(effet, CreateSmallCaster(), expected);
    }

    [Test, TestCaseSource("GetValidateEffetSource")]
    [Tooltip("Pourcentage = 100")]
    public void ResultEffet_CasterSmall_ValeurBrut5_NbAttaque1(TypeEffet typeEffet)
    {
        var expected = FindExpectedResult(typeEffet, 4);
        var effet = CreateEffet(typeEffet, valeurBrute: 5, nbAttaques: 1, pourcentage: 100);
        TestResultEffet(effet, CreateSmallCaster(), expected);
    }

    [Test, TestCaseSource("GetValidateEffetSource")]
    [Tooltip("Pourcentage = 100")]
    public void ResultEffet_CasterSmall_ValeurBrut5_NbAttaque2(TypeEffet typeEffet)
    {
        var expected = FindExpectedResult(typeEffet, 5);
        var effet = CreateEffet(typeEffet, valeurBrute: 5, nbAttaques: 2, pourcentage: 100);
        TestResultEffet(effet, CreateSmallCaster(), expected);
    }

    [Test, TestCaseSource("GetValidateEffetSource")]
    [Tooltip("Pourcentage = 100")]
    public void ResultEffet_CasterMedium_ValeurBrut0_NbAttaque0(TypeEffet typeEffet)
    {
        var expected = FindExpectedResult(typeEffet, 6);
        var effet = CreateEffet(typeEffet, valeurBrute: 0, nbAttaques: 0, pourcentage: 100);
        TestResultEffet(effet, CreateMediumCaster(), expected);
    }

    [Test, TestCaseSource("GetValidateEffetSource")]
    [Tooltip("Pourcentage = 100")]
    public void ResultEffet_CasterMedium_ValeurBrut5_NbAttaque1(TypeEffet typeEffet)
    {
        var expected = FindExpectedResult(typeEffet, 7);
        var effet = CreateEffet(typeEffet, valeurBrute: 5, nbAttaques: 1, pourcentage: 100);
        TestResultEffet(effet, CreateMediumCaster(), expected);
    }

    [Test, TestCaseSource("GetValidateEffetSource")]
    [Tooltip("Pourcentage = 100")]
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