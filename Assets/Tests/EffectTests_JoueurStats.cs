using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;

/*TODO*/
/*
public partial class EffectTests
{
    private static string[] TestedJoueurStatFieldNames =
        {
            nameof(JoueurStat.Clairvoyance),
            nameof(JoueurStat.Conscience),
            nameof(JoueurStat.ConscienceMax),
            nameof(JoueurStat.Conviction),
            nameof(JoueurStat._forceAme),
            nameof(JoueurStat.MultiplDef),
            nameof(JoueurStat.MultiplDegat),
            nameof(JoueurStat.MultiplSoin),
            nameof(JoueurStat.PalierChangement),
            nameof(JoueurStat.Radiance),
            nameof(JoueurStat.RadianceMax),
            nameof(JoueurStat._resilience),
            nameof(JoueurStat.Tension),
            nameof(JoueurStat.TensionAttaque),
            nameof(JoueurStat.TensionDebuff),
            nameof(JoueurStat.TensionDot),
            nameof(JoueurStat.TensionSoin),
            nameof(JoueurStat.Vitesse),
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
            defaultJoueurStat.Tension = 0;
            defaultJoueurStat.TensionAttaque = 4;
            defaultJoueurStat.TensionDebuff = 3;
            defaultJoueurStat.TensionDot = 1;
            defaultJoueurStat.TensionSoin = -1;
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
}
*/