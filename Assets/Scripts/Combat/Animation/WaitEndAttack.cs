using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitEndAttack : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //After animation, reactivate spells.
        if (GameManager.Instance != null)
            GameManager.Instance.BattleMan.ActivatePlayer();
        else
            TutoManager.Instance.BattleManager.ActivatePlayer();
    }
}
