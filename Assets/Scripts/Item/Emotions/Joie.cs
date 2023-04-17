using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering;
using Debug = UnityEngine.Debug;

public class Joie : Emotion
{
    public static int nbBuff;

    public Joie()
    {
        EmotionName = "Joie";
        EmotionTypeEnum = EmotionTypeEnum.Joie;
        nbBuff = 0;
        GameManager.instance.playerStat.ListBuffDebuff.ItemAdded += ListBuffDebuff_ItemAdded;
        GameManager.instance.playerStat.ListBuffDebuff.ItemRemoved += ListBuffDebuff_ItemRemoved; // Ou alors on passe par unity event comme pour fierte
    }

    public void ListBuffDebuff_ItemAdded(ObservableList<BuffDebuff> sender, ListChangedEventArgs<BuffDebuff> e)
    {
        nbBuff++;
        Debug.Log("Buff Debuff Added " + e.index + " - " + e.item.Nom + " - " + nbBuff);
        Debug.Log(sender.Contains(e.item) + " est ce qu'il y est déja?");
        if (nbBuff == 5)
        {
            ApplyBonusJoie();
            nbBuff = 0;
        }

    }

    public void ListBuffDebuff_ItemRemoved(ObservableList<BuffDebuff> sender, ListChangedEventArgs<BuffDebuff> e)
    {
        Debug.Log("Buff Debuff Removed " + e.index + " - " + e.item.Nom);
    }

    public void ApplyBonusJoie()
    {
        Debug.Log("5 bonus diff reçue");
        //appliquer un debuff a l'ennemie (sur la prochaine action) de degat de xx par cout d'activation
    }
}
//Après avoir reçu 5 avantages (buffs) qui ne vous étaient pas déjà appliqués, la prochaine
//compétence utilisée réduira les dégâts infligés par la prochaine attaque reçue de XX par points
//utilisés pour la lancer (Volonté et/ou Conscience).
//*design pattern observer : faire de l'ajout de buff un obersvervable(provider) et register le truc de joie comme observer
//*aprés 5 buff,appliquer un debuff a l'ennemie (sur la prochaine action) de degat de xx par cout d'activation