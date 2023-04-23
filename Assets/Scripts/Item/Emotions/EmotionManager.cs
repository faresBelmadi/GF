using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.Experimental.GraphView.Port;

public class EmotionManager
{
    public Emotion CreateEmotion(EmotionTypeEnum emotionType)
    {
        switch (emotionType)
        {
            case EmotionTypeEnum.Joie:
                return new Joie();
            case EmotionTypeEnum.Espoir:
                return new Espoir();
            case EmotionTypeEnum.Fierte:
                return new Fierte();
            case EmotionTypeEnum.Honte:
                return new Honte();
            case EmotionTypeEnum.Nostalgie:
                return new Nostalgie();
            case EmotionTypeEnum.Rancune:
                return new Rancune();
            case EmotionTypeEnum.Serenite:
                return new Serenite();
            case EmotionTypeEnum.Frustration:
                return new Frustration();
            default:
                return null;
        }
    }

    #region Joie

    public int nbBuff;

    public void BuffDebuffAddedEventTriggered()
    {
        Debug.Log("BuffDebuffAddedEventTriggered");
        if (GameManager.instance.playerStat.ListSouvenir.Any(x =>
                x.Equiped && x.Emotion.EmotionTypeEnum == EmotionTypeEnum.Joie))
        {
            nbBuff++;
            Debug.Log("emotion add " + nbBuff);
        }
        if (nbBuff == 5)
        {
            foreach (var souvenir in GameManager.instance.playerStat.ListSouvenir.Where(x => x.Equiped && x.Emotion.EmotionTypeEnum == EmotionTypeEnum.Joie))
            {
                ApplyBonusJoie();
            }
            nbBuff = 0;
        }

    }

    public void ApplyBonusJoie()
    {
        Debug.Log("5 bonus diff reçue");
        //appliquer un debuff a l'ennemie (sur la prochaine action) de degat de xx par cout d'activation
        // passer par un bool pour appliquer l'effet en plus ?
    }
    //Après avoir reçu 5 avantages (buffs) qui ne vous étaient pas déjà appliqués, la prochaine
    //compétence utilisée réduira les dégâts infligés par la prochaine attaque reçue de XX par points
    //utilisés pour la lancer (Volonté et/ou Conscience).
    //*design pattern observer : faire de l'ajout de buff un obersvervable(provider) et register le truc de joie comme observer
    //*aprés 5 buff,appliquer un debuff a l'ennemie (sur la prochaine action) de degat de xx par cout d'activation
    #endregion

    #region Fierte

    public static int nbCapacity = 0;
    public void CapacityUsed()
    {
        nbCapacity++;
        if (nbCapacity == 3)
        {
            Debug.Log("3 capacité Utilisé");
        }
    }
    //Lors de chaque affrontement, les 3 premières capacités utilisées (offensives ou non) obtiennent
    //  un bonus aux dégâts infligés équivalent à XX% de votre Radiance actuelle pour chaque pièce
    //visitée de l’étage actuel.

    #endregion

    #region Espoir

    public void AmplifyStats() // a appeler apres chaque combat, si le joeur possede le souvenir avec de l'espoir?
    {
        Debug.Log("Espoir trigered - not implemented yet");
    }
    //Après chaque affrontement (mais avant d’obtenir les récompenses), si vous avez plus de 50%
    //de votre Radiance max., les statistiques de X souvenir(s) aléatoire(s) sont amplifiées(les
    //statistiques apportées par le souvenir passent au palier supérieur : vitesse + 2 passe à +4 par
    //exemple et dans le sens inverse si c’est un malus).

    #endregion

    #region Honte

    //Lors de chaque affrontement, tant que vous attaquez uniquement des adversaires qui ont des
    //intentions agressives(attaque directe et application de débuff), vous gagnez un 1 point de
    //Conviction et XX% de Radiance max. par attaque effectuée (si la chaine est rompue, les bonus
    //sont perdus). //tant qu'on sais que l'ennemie vas attaquer, alors faire

    #endregion

    #region Nostalgie

    //A la fin d’une Phase, si vous avez dépensé un nombre de points de Volonté supérieur ou égal
    //au nombre de souvenirs que vous possédez, vous aurez +XX points de Vitesse et +YY points
    //de Clairvoyance lors de la prochaine Phase. Dans le cas contraire, vous récupérez ZZ% de
    //votre Radiance manquante par souvenirs que vous possédez.

    #endregion

    #region Rancune

    //Quand au moins un désavantage (débuff) vous affecte, le nombre de points de Conscience
    //nécessaires pour utiliser une capacité est réduit de 1. Les compétences dont le coût en
    //Conscience n’est pas annulé par cet effet obtiennent un bonus aux dégâts infligés équivalent à
    //XX% de votre Force d’âme.

    #endregion

    #region Serenite

    public bool Attacked = false;

    public void ApplyBonusFA()
    {
        Debug.Log("Serenite ApplyBonusFA trigered - not implemented yet");
    }

    public void ApplyTension()
    {
        Debug.Log("Serenite ApplyTension trigered - not implemented yet");
    }

    //fin du tour checker si variable "a attaqué" / a fait des degats?
    //=>application Bonus FA
    //si -25% santé, Tension x2
    //jusqu'a la fin de l'affrontement =>


    //Tant que votre jauge de Tension n’est pas pleine, si vous n’infligez pas de dégâts directs durant
    //votre Tour, votre Force d’âme est augmentée de XX% jusqu’à la fin de l’affrontement. Si vous
    //avez moins de 25% de votre Radiance max., les gains de Tension sont doublés.
    //Joueur behaviorf in de tour si existe

    #endregion

    #region Frustration

    public void GainBonusDegat()
    {
        Debug.Log("Frustration Start Tour trigered - not implemented yet");
    }

    public void GainTensionPlayer()
    {
        Debug.Log("Frustration End Turn trigered - not implemented yet");
    }
    //Au début de votre Tour, s’il vous reste moins de 2 points de Conscience, la première capacité
    //utilisée obtient un bonus aux dégâts infligés de XX%. A la fin de votre Tour, s’il vous reste au
    //moins 1 point de Volonté, vous gagnez YY points de Tension.


    #endregion

}