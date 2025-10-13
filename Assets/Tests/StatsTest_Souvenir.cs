
using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class StatsTests
{

    private Souvenir LoadSouvenirObject(string nameSouvenir)
    {
        string scriptableObjectName = nameSouvenir;
        string[] guids = AssetDatabase.FindAssets($"t:{nameof(Souvenir)} {scriptableObjectName}");
        if (guids.Length == 0)
            Assert.Fail($"No {nameof(Souvenir)} found named {scriptableObjectName}");

        if (guids.Length > 0)
            Debug.LogWarning($"More than one {nameof(Souvenir)} found named {scriptableObjectName}, taking first one");

        return (Souvenir)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guids[0]), typeof(Souvenir));
    }

    //[Test]        Modifiait le SO de souvenir, donc test desactive
    public void EquipMemory()
    {
        int baseValue = 100;
        var baseStat = CreatePlayerStatSO(baseValue, 3, 5);
        var statHolder = new PlayerStatsHandler(baseStat);
        var statTmpHolder = new PlayerStatsHandler(baseStat);

        //Souvenir souv = ScriptableObject.CreateInstance<Souvenir>();
        //var o = new ModificationStatSouvenir();
        //o.StatModif = StatModif.ForceAme;
        //var paramStat = new ParametreModifStat
        //{
        //    ParametreStat = ParametreStat.ValeurBrut,
        //    Valeur = 10
        //};
        //o.ParametreModifStat = paramStat;
        //var p1 = new PourcentageEmotion
        //{
        //    Emotion = Emotion.Joie,
        //    Pourcentage = 70
        //};
        //var p2 = new PourcentageEmotion
        //{
        //    Emotion = Emotion.Rancune,
        //    Pourcentage = 30
        //};
        //souv.ProcEmotion = new List<PourcentageEmotion> { p1,p2 };
        //souv.ModificationStat = new List<ModificationStatSouvenir> { o };

        var obj = GameObject.FindObjectOfType<GameManager>();
        GameObject go = GameObject.Instantiate(obj.gameObject);
        GameManager gm = go.GetComponent<GameManager>();
        GameManager.Instance = gm;

        var msm = GameObject.FindObjectOfType<MenuStatManager>(true);
        GameObject msmgo = GameObject.Instantiate(msm.gameObject);
        MenuStatManager menuStatMng = msmgo.GetComponent<MenuStatManager>();

        menuStatMng.Stat = statHolder;
        menuStatMng.StatTemp = statTmpHolder;

        SouvenirUI souvUI = new SouvenirUI();
        Souvenir souv = LoadSouvenirObject("DebugSouvenir");
        souv.Equiped = false;
        souvUI.LeSouvenir = souv;
        menuStatMng.Equiped(souvUI);

        Assert.That(statHolder.ForceAme, Is.EqualTo(110));
    }
}
