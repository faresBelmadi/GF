using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public partial class EffectTests
{
    /// <summary>
    /// If you add a test variation, edit TestVariationCount accordingly!
    /// </summary>
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

#region Simple Effects

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

#endregion

#region With Floor rounding

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
                TypeEffet.AugmentationPourcentageFACaster, // ForceAme += Pourcentage/100 * NbAttaque * caster.ForceAme
                new Func<JoueurStat>[] {
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js._forceAme = 0;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js._forceAme = 0;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js._forceAme = 0;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js._forceAme = 0;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js._forceAme = 1;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js._forceAme = 2;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js._forceAme = 0;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js._forceAme = 5;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js._forceAme = 10;
                        return js;
                    },
                }
            },
            {
                TypeEffet.AugmentationPourcentageFACible, // ForceAme += Pourcentage/100 * NbAttaque * cible.ForceAme
                new Func<object>[] {
                    () => new NullReferenceException(),
                    () => new NullReferenceException(),
                    () => new NullReferenceException(),
                    () => new NullReferenceException(),
                    () => new NullReferenceException(),
                    () => new NullReferenceException(),
                    () => new NullReferenceException(),
                    () => new NullReferenceException(),
                    () => new NullReferenceException(),
                }
            },

#endregion

#region Different with or without cible

            {
                // With Cible:
                // Radiance += cible.RadianceMax * Pourcentage/100
                // RadianceMax += cible.RadianceMax * Pourcentage/100
                // Without Cible:
                // Radiance += caster.RadianceMax * Pourcentage/100
                // RadianceMax += caster.RadianceMax * Pourcentage/100
                TypeEffet.RadianceMax,
                new Func<JoueurStat>[] {
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Radiance = 0;
                        js.RadianceMax = 0;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Radiance = 0;
                        js.RadianceMax = 0;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Radiance = 0;
                        js.RadianceMax = 0;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Radiance = 5;
                        js.RadianceMax = 5;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Radiance = 5;
                        js.RadianceMax = 5;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Radiance = 5;
                        js.RadianceMax = 5;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Radiance = 10;
                        js.RadianceMax = 10;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Radiance = 10;
                        js.RadianceMax = 10;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js.Radiance = 10;
                        js.RadianceMax = 10;
                        return js;
                    },
                }
            },

#endregion

#region Others

            {
                TypeEffet.AugmentationBrutFA, // ForceAme += ValeurBrut * NbAttaque
                new Func<JoueurStat>[] {
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js._forceAme = 0;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js._forceAme = 5;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js._forceAme = 10;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js._forceAme = 0;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js._forceAme = 5;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js._forceAme = 10;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js._forceAme = 0;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js._forceAme = 5;
                        return js;
                    },
                    () => {
                        var js = ScriptableObject.CreateInstance("JoueurStat") as JoueurStat;
                        js._forceAme = 10;
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

#endregion

    };

    [Test]
    public void EnsureExpectationDefinitions()
    {
        foreach (var expectation in ExpectedResultsByEffect)
        {
            Assert.AreEqual(expectation.Value.Length, TestVariationCount, $"Type {expectation.Key} has {expectation.Value.Length} variations defined, when {TestVariationCount} are expected.");
        }
    }
}
