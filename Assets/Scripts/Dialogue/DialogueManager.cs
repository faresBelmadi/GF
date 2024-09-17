using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DialogueManager : MonoBehaviour
{
    #region UI Reference

    public GameObject UIDialogue;
    [Tooltip("Font size for dialogue options")]
    [SerializeField]
    private float _fontSize = 34f;
    public GameObject UIJoueur;
    [SerializeField]
    private Color _speakerColor;
    public TextMeshProUGUI MainText;
    public GameObject MainTextGO;
    public List<TextMeshProUGUI> Réponse;
    public List<GameObject> RéponseGO;
    public GameObject EndDialogue;
    public TextMeshProUGUI EndText;
    public BattleManager ManagerBattle;

    public AleaManager ManagerAlea;
    //public Button skipButton; 
    [SerializeField]
    private ClairvoyanceIconData _clairvoyanceIconData;
    #endregion UI Reference

    #region SO

    private Encounter _CurrentEncounterBattle;
    private EncounterAlea _CurrentEncounterAlea;
    internal DialogueSO _CurrentDialogue;

    #endregion SO

    #region Dialogue Property

    internal int DialogueIndex = 0;
    private int NextDialogueIndex = 0;
    private Dictionary<ClairvoyanceIconStatEnum, bool> _displayedClairvoyanceStats;

    #endregion Dialogue Property

    private void OnEnable()
    {
        if (Réponse.Count>= 1 && Réponse[0] != null)
            Réponse[0].GetComponent<TextDisplayer>().OnDisplayAnimFinish += StopSFX;
        if (EndText != null)
            EndText.GetComponent<TextDisplayer>().OnDisplayAnimFinish += StopSFX;
    }
    private void OnDisable()
    {
        if (Réponse.Count >= 1 && Réponse[0] != null)
            Réponse[0].GetComponent<TextDisplayer>().OnDisplayAnimFinish -= StopSFX;
        if (EndText != null)
            EndText.GetComponent<TextDisplayer>().OnDisplayAnimFinish -= StopSFX;
    }
    private void Start()
    {
        TMP_Text[] dialogArray = UIDialogue.GetComponentsInChildren<TMP_Text>(true);
        for (int i = 0; i< dialogArray.Length; i++)
        {
            dialogArray[i].enableAutoSizing = false;
            dialogArray[i].fontSize = _fontSize;
        }
    }
    public void SetupDialogue(Encounter encounterToSet)
    {
        _CurrentDialogue = encounterToSet.DialogueRencontre;
        _CurrentEncounterBattle = encounterToSet;
        UIJoueur.SetActive(false);
        UIDialogue.SetActive(true);
        startDialogue();
    }

    public void SetupDialogue(EncounterAlea encounterToSet)
    {
        _CurrentDialogue = encounterToSet.DialogueRencontre;
        _CurrentEncounterAlea = encounterToSet;
        startDialogue();
    }

    void startDialogue()
    {
        resetRéponse();

        
        GoNext();
    }

    void GoNext()
    {
        AudioManager.instance.SFX.PlaySFXClip(SFXType.DialogueSFX);
        DialogueIndex = NextDialogueIndex;
        MainText.text = TextePrincipal();
        MainTextGO.SetActive(true);
        TextDisplayer textDisplayer = MainText.GetComponent<TextDisplayer>();
        if (textDisplayer != null )
        {
            MainText.GetComponent<TextDisplayer>().OnDisplayAnimFinish += GetAnswerList;
        }
        else
        {
            GetAnswerList();
        }
    }

    private void GetAnswerList()
    {
        _displayedClairvoyanceStats = new Dictionary<ClairvoyanceIconStatEnum, bool>();
        bool[] displayed = new bool[Enum.GetValues(typeof(ClairvoyanceIconStatEnum)).Length];
        TextDisplayer textDisplayer = MainText.GetComponent<TextDisplayer>();
        if (textDisplayer != null)
        {
            MainText.GetComponent<TextDisplayer>().OnDisplayAnimFinish -= GetAnswerList;
        }
        var currentQuestionType = _CurrentDialogue.Questions[DialogueIndex].Question.type;
        var currentPossibleResponseList = _CurrentDialogue.Questions[DialogueIndex].ReponsePossible;
        if ( currentQuestionType == TypeQuestion.startCombat ||
             currentQuestionType == TypeQuestion.EndTutoDialogue)
        {
            string DialogueTrad;
            if (!string.IsNullOrEmpty(currentPossibleResponseList[0].IdStringReponse))
            {
                //DialogueTrad =
                //    TradManager.instance.DialogueDictionary[currentPossibleResponseList[0].IdStringReponse][
                //        TradManager.instance.IdLanguage];
                DialogueTrad = TradManager.instance.GetTranslation(currentPossibleResponseList[0].IdStringReponse, "ID_DIALOGUE_NOT_IMPLEMENTED");
            }
            else
            {
                DialogueTrad = "ID_DIALOGUE_NOT_IMPLEMENTED";
            }
            if (EndText != null)
            {
                //if(EndText.text == null || EndText.text == "")
                if (currentPossibleResponseList != null &&
                    currentPossibleResponseList.Count > 0 &&
                    !string.IsNullOrEmpty(DialogueTrad))
                    EndText.text = DialogueTrad;
                else
                {
                    EndText.text = "Continuer";
                }
            }
            else
            {
                var Text = EndDialogue.GetComponentInChildren<TextMeshProUGUI>();
                if (Text != null)
                {
                    Text.text = DialogueTrad;
                }
            }
            EndDialogue.SetActive(true);
        }
        else
        {
            for (int i = 0; i < currentPossibleResponseList.Count; i++)
            {
                string response;
                if (!string.IsNullOrEmpty(currentPossibleResponseList[i].IdStringReponse))
                {
                    //response =
                    //    TradManager.instance.DialogueDictionary[currentPossibleResponseList[i].IdStringReponse][
                    //        TradManager.instance.IdLanguage];
                    response = TradManager.instance.GetTranslation(currentPossibleResponseList[i].IdStringReponse, "ID_DIALOGUE_NOT_IMPLEMENTED");
                }
                else
                {
                    response = "ID_DIALOGUE_NOT_IMPLEMENTED";
                }
                Réponse[i].text = response;
                RéponseGO[i].SetActive(true);
                //Réponse[i].GetComponent<TextAnimation>().LaunchAnim();

                if (ManagerBattle.player.Stat.Clairvoyance >= currentPossibleResponseList[i].SeuilClairvoyanceStat)
                {
                    ShowConsequenceForAnswer(i, ref displayed);
                }
            }
        }
    }

    private string TextePrincipal()
    {
        string dialogueTrad;
        string colorCode = ColorUtility.ToHtmlStringRGB(_speakerColor);
        if (!string.IsNullOrEmpty(_CurrentDialogue.Questions[DialogueIndex].Question.IdStringQuestion))
        {
            //dialogueTrad =
            //    TradManager.instance.DialogueDictionary[_CurrentDialogue.Questions[DialogueIndex].Question.IdStringQuestion]
            //        [TradManager.instance.IdLanguage];
            dialogueTrad = TradManager.instance.GetTranslation(_CurrentDialogue.Questions[DialogueIndex].Question.IdStringQuestion, "ID_DIALOGUE_NOT_IMPLEMENTED");
        }
        else
        {
            dialogueTrad = "ID_DIALOGUE_NOT_IMPLEMENTED";
        }
        if (ManagerBattle == null && _CurrentEncounterAlea != null)
        {
            return "<allcaps><u><b><color=#"+ colorCode + ">" + _CurrentEncounterAlea.NamePnj + ": </color></b></u></allcaps> " + dialogueTrad;
        }
        else
        {
            string encounteurName = TradManager.instance.GetTranslation(_CurrentEncounterBattle.ToFight[_CurrentDialogue.Questions[DialogueIndex].Question.IDSpeaker].IdTradName,
                _CurrentEncounterBattle.ToFight[_CurrentDialogue.Questions[DialogueIndex].Question.IDSpeaker].Nom);
            return "<allcaps><u><b><color=#" + colorCode + ">" + encounteurName + ": </color></b></u></allcaps>" + dialogueTrad;
        }
    }
     
    void resetRéponse()
    {
        foreach (var item in RéponseGO)
        {
            item.SetActive(false);
        }
        EndDialogue.SetActive(false);

        MainTextGO.SetActive(false);
        foreach (var item in Réponse)
        {
            item.text = "";
        }
    }

    public void GetRéponse(int i)
    {
        if (_CurrentDialogue.Questions[DialogueIndex].ReponsePossible[i].conséquences.Count != 0)
        {
            ApplyConsequence(_CurrentDialogue.Questions[DialogueIndex].ReponsePossible[i].conséquences);
        }

        NextDialogueIndex = _CurrentDialogue.Questions[DialogueIndex].ReponsePossible[i].IDNextQuestion;
        resetRéponse();
        GoNext();
    }

    private string BuildSpriteIcon(Effet effet, ref bool[] displayed)
    {
        StringBuilder strb = new StringBuilder();
        strb.Append("<sprite name=\"");
        Color color = Color.white;
        Debug.Log($"EffectSprite : {effet.TypeEffet}, cible : {effet.Cible}");
      
        switch (effet.TypeEffet)
        {
            case TypeEffet.AttaqueFADebuff:
                
                color = (effet.Cible == Cible.joueur) ? Color.red : Color.green;
                if (effet.Cible == Cible.joueur)
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.ForceDameDown])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.ForceDameDown] = true;
                        strb.Append((_clairvoyanceIconData.StatForceDameDown != null) ? _clairvoyanceIconData.StatForceDameDown.name : "FA");
                    }
                    else return "";
                }
                else
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.ForceDameUp])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.ForceDameUp] = true;
                        strb.Append((_clairvoyanceIconData.StatForceDameUp != null) ? _clairvoyanceIconData.StatForceDameUp.name : "FA");
                    }
                    else return "";
                }
                break;
            case TypeEffet.AugmentationPourcentageFACible:
            case TypeEffet.AugmentationBrutFA:
            case TypeEffet.AugmentationPourcentageFA:
                color = (effet.Cible == Cible.joueur) ? Color.green : Color.red;
                if (effet.Cible != Cible.joueur)
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.ForceDameDown])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.ForceDameDown] = true;
                        strb.Append((_clairvoyanceIconData.StatForceDameDown != null) ? _clairvoyanceIconData.StatForceDameDown.name : "FA");
                    }
                    else return "";
                }
                else
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.ForceDameUp])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.ForceDameUp] = true;
                        strb.Append((_clairvoyanceIconData.StatForceDameUp != null) ? _clairvoyanceIconData.StatForceDameUp.name : "FA");
                    }
                    else return "";
                }
                break;
            case TypeEffet.RadianceMax:
                color = (effet.ValeurBrut > 0) ? Color.green : Color.red;
                if (effet.ValeurBrut < 0)
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.RadianceDown])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.RadianceDown] = true;
                        strb.Append((_clairvoyanceIconData.StatRadianceDown != null) ? _clairvoyanceIconData.StatRadianceDown.name : "Rad");
                    }
                    else return "";
                }
                else
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.RadianceUp])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.RadianceUp] = true;
                        strb.Append((_clairvoyanceIconData.StatRadianceUp != null) ? _clairvoyanceIconData.StatRadianceUp.name : "Rad");
                    }
                    else return "";
                }
                break;
            case TypeEffet.Resilience:
                color = (effet.ValeurBrut > 0) ? Color.green : Color.red;
                if (effet.ValeurBrut < 0)
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.ResilienceDown])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.ResilienceDown] = true;
                        strb.Append((_clairvoyanceIconData.StatResilienceDown != null) ? _clairvoyanceIconData.StatResilienceDown.name : "Res");
                    }
                    else return "";
                }
                else
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.ResilienceUp])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.ResilienceUp] = true;
                        strb.Append((_clairvoyanceIconData.StatResilienceUp != null) ? _clairvoyanceIconData.StatResilienceUp.name : "Res");
                    }
                    else return "";
                }
                break;
            case TypeEffet.Clairvoyance:
                color = (effet.ValeurBrut > 0) ? Color.green : Color.red;
                if (effet.ValeurBrut < 0)
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.ClairvoyaneDown])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.ClairvoyaneDown] = true;
                        strb.Append((_clairvoyanceIconData.StatClairvoyanceDown != null) ? _clairvoyanceIconData.StatClairvoyanceDown.name : "Cla");
                    }
                    else return "";
                }
                else
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.ClairvoyaneUp])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.ClairvoyaneUp] = true;
                        strb.Append((_clairvoyanceIconData.StatClairvoyanceUp != null) ? _clairvoyanceIconData.StatClairvoyanceUp.name : "Cla");
                    }
                    else return "";
                }
                break;
            case TypeEffet.Vitesse:
                color = (effet.ValeurBrut > 0) ? Color.green : Color.red;
                if (effet.ValeurBrut < 0)
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.VitesseDown])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.VitesseDown] = true;
                        strb.Append((_clairvoyanceIconData.StatVitesseDown != null) ? _clairvoyanceIconData.StatVitesseDown.name : "Vit");
                    }
                    else return "";
                }
                else
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.VitesseUp])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.VitesseUp] = true;
                        strb.Append((_clairvoyanceIconData.StatVitesseUp != null) ? _clairvoyanceIconData.StatVitesseUp.name : "Vit");
                    }
                    else return "";
                }
                break;
            case TypeEffet.Conviction:
                color = (effet.ValeurBrut > 0) ? Color.green : Color.red;
                if (effet.ValeurBrut < 0)
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.ConvictionDown])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.ConvictionDown] = true;
                        strb.Append((_clairvoyanceIconData.StatConvictionDown != null) ? _clairvoyanceIconData.StatConvictionDown.name : "Con");
                    }
                    else return "";
                }
                else
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.ConvictionUp])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.ConvictionUp] = true;
                        strb.Append((_clairvoyanceIconData.StatConvictionUp != null) ? _clairvoyanceIconData.StatConvictionUp.name : "Con");
                    }
                    else return "";
                }
                break;
            case TypeEffet.Conscience:
                color = (effet.ValeurBrut > 0) ? Color.green : Color.red;
                if (effet.ValeurBrut < 0)
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.ConscienceDown])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.ConscienceDown] = true;
                        strb.Append((_clairvoyanceIconData.StatConscienceDown != null) ? _clairvoyanceIconData.StatConscienceDown.name : "con");
                    }
                    else return "";
                }
                else
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.ConscienceUp])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.ConscienceUp] = true;
                        strb.Append((_clairvoyanceIconData.StatConscienceUp != null) ? _clairvoyanceIconData.StatConscienceUp.name : "con");
                    }
                    else return "";
                }
                break;
            case TypeEffet.DegatPVMax:
            case TypeEffet.DegatsBrut:
            case TypeEffet.DegatsBrutConsequence:
            case TypeEffet.Volonte:
            case TypeEffet.VolonteMax:
                color = (effet.ValeurBrut > 0) ? Color.green : Color.red;
                if (effet.ValeurBrut < 0)
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.VolonteDown])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.VolonteDown] = true;
                        strb.Append((_clairvoyanceIconData.StatVolonteDown != null) ? _clairvoyanceIconData.StatVolonteDown.name : "Vol");
                    }
                    else return "";
                }
                else
                {
                    if (!displayed[(int)ClairvoyanceIconStatEnum.VolonteUp])
                    {
                        displayed[(int)ClairvoyanceIconStatEnum.VolonteUp] = true;
                        strb.Append((_clairvoyanceIconData.StatVolonteUp != null) ? _clairvoyanceIconData.StatVolonteUp.name : "Vol");
                    }
                    else return "";
                }
                break;
            case TypeEffet.DegatsForceAme:
            case TypeEffet.Colere:
            case TypeEffet.AugmentFADernierDegatsSubi:
            case TypeEffet.MultiplDegat:
            case TypeEffet.MultiplSoin:
            case TypeEffet.MultiplDef:
            case TypeEffet.TensionStep:
            case TypeEffet.TensionValue:
            case TypeEffet.TensionGainAttaqueValue:
            case TypeEffet.TensionGainDebuffValue:
            case TypeEffet.TensionGainSoinValue:
            case TypeEffet.TensionGainDotValue:
            case TypeEffet.ConscienceMax:
            case TypeEffet.Soin:
            case TypeEffet.SoinFA:
            case TypeEffet.SoinFANbEnnemi:
            case TypeEffet.SoinRadianceMax:
            case TypeEffet.SoinRadianceActuelle:
            case TypeEffet.RandomAttaque:
            case TypeEffet.AugmentationFaRadianceActuelle:
            case TypeEffet.ConsommeTensionAugmentationFA:
            case TypeEffet.RemoveDebuff:
            case TypeEffet.AttaqueStackAmant:
            case TypeEffet.GainResilienceIncrementale:
            case TypeEffet.DamageLastPhase:
            case TypeEffet.NoEssence:
            case TypeEffet.DoubleBuffDebuff:
            case TypeEffet.AugmentationRadianceMaxPourcentage:
            case TypeEffet.BuffFaCoupRecu:
            case TypeEffet.BuffResilienceCoupRecu:
            case TypeEffet.ConsommeTensionDmgAllExceptCaster:
            case TypeEffet.Provocation:
            case TypeEffet.VolEssence:
            case TypeEffet.RandomChanceCastSpellSelf:
            case TypeEffet.SwapMostLeastBuffDebuff:
            case TypeEffet.RadianceRepartition:
            case TypeEffet.RandomAttaqueDebuff:
            case TypeEffet.DegatsRetourSurAttaque:
            case TypeEffet.RedirectionDegatsOnCasteur:
            case TypeEffet.CancelPourcentageDamage:
            case TypeEffet.RedirectionCancel:
            case TypeEffet.DispellBuffJoueurDamage:
            case TypeEffet.DispellDebuffCasterDamage:
            case TypeEffet.DamageAllEvenly:
            case TypeEffet.DamageUpTargetLowRadiance:
            case TypeEffet.OnKillStunAll:
            case TypeEffet.UntilDeath:
            case TypeEffet.AugmentationFARadianceManquante:
            case TypeEffet.DamageFaBuff:
            case TypeEffet.DamageFaBuffCible:
            case TypeEffet.DamageDebuffCible:
            case TypeEffet.RemoveAllTensionProcDamage:
            case TypeEffet.RemoveAllTensionProcBuffDebuff:
            case TypeEffet.RemoveAllDebuffProcBuffDebuf:
            case TypeEffet.RemoveAllDebuffSelfProcBuffDebuf:
            case TypeEffet.RemoveAllBuffProcBuffDebuf:
            case TypeEffet.RemoveAllDebuffProcDamage:
            case TypeEffet.RemoveAllDebuffSelfProcDamage:
            case TypeEffet.RemoveAllBuffProcDamage:
            case TypeEffet.NoCapaPossible:
            case TypeEffet.ConsommeTensionReduitFa:
            case TypeEffet.AugmentationDegatsHitJoueur:
            case TypeEffet.GainFaBuffCible:
            case TypeEffet.GainFaDebuffCible:
            case TypeEffet.Ponction:
            case TypeEffet.PonctionForceAme:
            case TypeEffet.DegatsFaRadianceManquanteCible:
            case TypeEffet.DegatsFaRadianceManquanteCaster:
            case TypeEffet.PremiereAttaqueJeanne:
            case TypeEffet.DeuxiemeAttaqueJeanne:
            case TypeEffet.SupportJeanne:
            case TypeEffet.UltimeJeanne:
                Debug.Log($"Effet non géré pour la clairvoyance : {effet.TypeEffet})");
                break;

        }
        strb.Append("\" color=#");
        strb.Append(ColorUtility.ToHtmlStringRGBA(color));
        strb.Append(">");
        return strb.ToString();
    }
    private void ShowConsequenceForAnswer(int selectedAnswer, ref bool[] displayed)
    {
        foreach (var consequence in _CurrentDialogue.Questions[DialogueIndex].ReponsePossible[selectedAnswer].conséquences)
        {
            foreach (var buffDebuff in consequence.Buffs)
            {
                foreach (Effet effet in buffDebuff.Effet)
                {
                    Réponse[selectedAnswer].text += BuildSpriteIcon(effet, ref displayed);
                }
            }
            foreach (var effet in consequence.Effects)
            {
                Réponse[selectedAnswer].text += BuildSpriteIcon(effet, ref displayed);
            }
        }
    }

    void ApplyConsequence(List<ConséquenceSO> consequence)
    {
        foreach (var Consequence in consequence)
        {
            if (ManagerBattle == null)
            {
                foreach (var BuffDebuff in Consequence.Buffs)
                {
                    ManagerAlea.Stat.ListBuffDebuff.Add(BuffDebuff);
                }

                foreach (var effet in Consequence.Effects)
                {
                    ManagerAlea.Stat.ModifStateAll(effet.ResultEffet(ManagerAlea.Stat));
                }
            }
            else
            {
                foreach (var BuffDebuff in Consequence.Buffs)
                {
                    ChoosePathOfExecution(Consequence, BuffDebuff);
                }

                foreach (var effet in Consequence.Effects)
                {
                    ChoosePathOfExecution(Consequence, effet);

                }
            }
        }
    }

    private void ChoosePathOfExecution(ConséquenceSO Consequence, ScriptableObject scriptableObject)
    {
        switch (Consequence.target)
        {
            case CibleDialogue.joueur:
                if (scriptableObject as BuffDebuff)
                {
                    ApplyBuffDebuffOnPlayer((BuffDebuff) scriptableObject);
                }
                else if (scriptableObject as Effet)
                {
                    ApplyEffectOnPlayer((Effet) scriptableObject);
                }

                break;
            case CibleDialogue.allEnnemi:
                if (scriptableObject as BuffDebuff)
                {
                    ApplyBuffDebuffOnEnemies((BuffDebuff) scriptableObject);
                }
                else if (scriptableObject as Effet)
                {
                    ApplyEffectOnEnemies((Effet) scriptableObject);
                }
                break;
            case CibleDialogue.ennemi:
                if (scriptableObject as BuffDebuff)
                {
                    ApplyBuffDebuffOneEnnemi((BuffDebuff) scriptableObject);
                }
                else if (scriptableObject as Effet)
                {
                    ApplyEffectOneEnnemi((Effet) scriptableObject);
                }
                break;
            case CibleDialogue.All:
                if (scriptableObject as BuffDebuff)
                {
                    ApplyBuffDebuffOnPlayer((BuffDebuff) scriptableObject);
                    ApplyBuffDebuffOnEnemies((BuffDebuff) scriptableObject);
                }
                else if (scriptableObject as Effet)
                {
                    ApplyEffectOnPlayer((Effet) scriptableObject);
                    ApplyEffectOnEnemies((Effet) scriptableObject);
                }
                break;
            case CibleDialogue.AllExceptSelf:
                break;
            case CibleDialogue.AllAllyExceptSelf:
                break;
            case CibleDialogue.Self:
                break;
        }
    }

    private void ApplyEffectOnPlayer(Effet scriptableObject)
    {
        ManagerBattle.player.Stat.ModifStateAll(scriptableObject.ResultEffet(ManagerBattle.player.Stat));
    }

    private void ApplyEffectOnEnemies(Effet scriptableObject)
    {
        foreach (var enemyScript in ManagerBattle.EnemyScripts)
        {
            enemyScript.Stat.ModifStateAll(scriptableObject.ResultEffet(enemyScript.Stat));
        }
    }    

    private void ApplyEffectOneEnnemi(Effet scriptableObject)
    {
        var enemyScript = ManagerBattle.EnemyScripts[Random.Range(0,ManagerBattle.EnemyScripts.Count)];
            enemyScript.Stat.ModifStateAll(scriptableObject.ResultEffet(((JoueurStat)(CharacterStat)enemyScript.Stat),enemyScript.LastDamageTaken,enemyScript.Stat));
    }

    private void ApplyBuffDebuffOnPlayer(BuffDebuff scriptableObject)
    {
        ManagerBattle.player.AddDebuff(Instantiate(scriptableObject), scriptableObject.Decompte, scriptableObject.timerApplication);
        ManagerBattle.player.AddBuffDebuff(scriptableObject, ManagerBattle.player.Stat);
    }

    private void ApplyBuffDebuffOnEnemies(BuffDebuff scriptableObject)
    {
        foreach (var enemyScript in ManagerBattle.EnemyScripts)
        {
            enemyScript.AddDebuff(Instantiate(scriptableObject), scriptableObject.Decompte, scriptableObject.timerApplication);
            enemyScript.AddBuffDebuff(scriptableObject, enemyScript.Stat);
        }
    }   
    private void ApplyBuffDebuffOneEnnemi(BuffDebuff scriptableObject)
    {
        var enemyScript = ManagerBattle.EnemyScripts[Random.Range(0,ManagerBattle.EnemyScripts.Count)];
        enemyScript.AddDebuff(Instantiate(scriptableObject), scriptableObject.Decompte, scriptableObject.timerApplication);
        enemyScript.AddBuffDebuff(scriptableObject, enemyScript.Stat);
    }
    public void StopSFX()
    {
        AudioManager.instance.SFX.StopPlaying();
    }
    public void StartCombat()
    {
        AudioManager.instance.SFX.StopPlaying();
        if (TutoManager.Instance != null)
        {
           var gO = GameObject.Find("TutoPanel");
           var child = gO.transform.GetChild(0);
           child.gameObject.SetActive(true);
           UIDialogue.SetActive(false);
           gO.GetComponent<TutoPanel>().ShowExplication();
        }
        else
        {
            UIJoueur.SetActive(true);
            UIDialogue.SetActive(false);
            GameManager.instance.BattleMan.StartCombat();
        }
    }

    public void EndDialogueFonction()
    {
        AudioManager.instance.SFX.StopPlaying();
        GameManager.instance.AleaMan.EndAlea();
    }
}