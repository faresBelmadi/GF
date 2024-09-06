using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitEndAttack : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //After animation, reactivate spells.
        GameManager.instance.BattleMan.player.ActivateSpells();
    }
}
