

// ***** OUTDATED
// ***** the mechanic doesn't exist anymore
// ***** TO REMOVE
using System.Collections.Generic;

public class Acharnement
{
    public string SpellOrigin;
    public int IdOrigin;
    public EffetAcharnementHabitude Type;
    public AcharnementType TypeModif;
    public FacteurAcharnementHabitude FacteurModif;
    public int ValeurBase;
    public int ValeurModif;
    public int MaxValeurModif;

    public Acharnement(string origin,int id, EffetAcharnementHabitude type, AcharnementType typeModif, FacteurAcharnementHabitude facteur, int valeur, int max)
    {
        SpellOrigin = origin;
        IdOrigin = id;
        Type = type;
        TypeModif = typeModif;
        FacteurModif = facteur;
        ValeurBase = valeur;
        ValeurModif = valeur;
        MaxValeurModif = max;
    }

    public void UpgradeEffect()
    {
        switch (FacteurModif)
        {
            case FacteurAcharnementHabitude.Additive:
            ValeurModif += ValeurBase;
            if(ValeurModif > MaxValeurModif)
                ValeurModif = MaxValeurModif;
            break;
            case FacteurAcharnementHabitude.Soustractive:
            ValeurModif += ValeurBase;
            if(ValeurModif < MaxValeurModif)
                ValeurModif = MaxValeurModif;
            break;
            case FacteurAcharnementHabitude.Double:
            ValeurModif *=2;
            if(ValeurModif > MaxValeurModif)
                ValeurModif = MaxValeurModif;
            break;
        }
    }
}

[System.Serializable]
public class Cost
{
    public TypeCostSpell typeCost;
    public int Value;
}


public class ActionResult
{
    public int HpModif;
    public int nbAttaque;
    public Cible target;
    public List<BuffDebuff> DebuffBuff;
}