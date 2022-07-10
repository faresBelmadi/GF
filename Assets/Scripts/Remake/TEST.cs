using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    public JoueurStatRemake TestJoueur;
    public JoueurStatRemake TestJoueurPrivate;
    public EffetRemake TestEffet;
    public EffetRemake TestEffet2;
    public BuffDebuffRemake TestBuff;
    public SpellRemake TestSpell;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("debut");

        TestJoueurPrivate = Instantiate(TestJoueur);
        JoueurStatRemake temp = ScriptableObject.CreateInstance("JoueurStatRemake") as JoueurStatRemake;
        temp = TestEffet.ResultEffet(TestJoueurPrivate);
        TestJoueurPrivate.ModifStateAll(temp);
        temp = TestEffet2.ResultEffet(TestJoueurPrivate);
        TestJoueurPrivate.ModifStateAll(temp);
        TestJoueurPrivate.ListSpell.Add(TestSpell);
        TestJoueurPrivate.ListBuffDebuff.Add(TestBuff);
        Debug.Log("fin");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
