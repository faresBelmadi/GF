using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New dialogue", menuName = "Capacité/Create New Passif", order = 11)]
public class Passif : ScriptableObject
{
    public TypePassif passif;

    public void ResultPassif()
    {
        switch (this.passif)
        {
            case TypePassif.CodeDeLHonneur:

                break;
            case TypePassif.SacrificeRituel:

                break;
            case TypePassif.Mannequin:

                break;
            case TypePassif.Martyr:

                break;
            case TypePassif.Cultiste:

                break;
            case TypePassif.Sauvage:

                break;
            case TypePassif.Fille:

                break;
            case TypePassif.Droit:

                break;
        }
    }
}
