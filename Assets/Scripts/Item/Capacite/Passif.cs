using UnityEngine;

[CreateAssetMenu(fileName = "New passif", menuName = "Capacité/Create New Passif", order = 11)]
public class Passif : ScriptableObject
{
    public TypePassif passif;

    public string Nom;
    public string Description;
    public TimerPassif timerPassif;
    public Sprite Icon;
}
