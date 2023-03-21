using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New dialogue", menuName = "Remake/Item/Create New Souvenir", order = 11)]
public class Souvenir : ScriptableObject
{
    public Rarity Rarete;
    public int Slots;
    public bool Equiped;
    public List<PourcentageEmotion> ProcEmotion;
    public Emotion Emotion;
    public List<ModificationStatSouvenir> ModificationStat;

    public string Nom;
    [TextArea(5, 10)]
    public string Description;
    public Sprite Icon;

    private void Awake()
    {
        ProcEmotion.Sort((x, y) => x.Pourcentage.CompareTo(y.Pourcentage));
        ChoixEmotion();
        EnableSlot();
    }

    private void ChoixEmotion()
    {
        int PourcentageTotal = 0;
        for (int i = 0; i < ProcEmotion.Count; i++)
        {
            PourcentageTotal += ProcEmotion[i].Pourcentage;
        }
        int random = Random.Range(0, PourcentageTotal + 1);
        for (int i = 0; i < ProcEmotion.Count; i++)
        {
            if (random <= ProcEmotion[i].Pourcentage)
            {
                Emotion = ProcEmotion[i].Emotion;
                return;
            }
            else
            {
                random -= ProcEmotion[i].Pourcentage;
            }
        }
    }

    private void EnableSlot()
    {
        switch (Rarete)
        {
            case Rarity.Commun:
                Slots = 1;
                break;
            case Rarity.Rare:
                Slots = 2;
                break;
            case Rarity.Mythique:
                Slots = 3;
                break;
            case Rarity.Legendaire:
                Slots = 4;
                break;
        }
    }
}
