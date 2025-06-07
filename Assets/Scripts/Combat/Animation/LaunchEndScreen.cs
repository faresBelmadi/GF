using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchEndScreen : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.Instance.EndGame();
    }
    
}
