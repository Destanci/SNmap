using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : StateMachineBehaviour
{
    NinjaController ninja;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ninja = animator.GetComponentInParent<NinjaController>();
        ninja.IsRespawnDone = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
        if (ninja.vertical != 0 || ninja.horizontal != 0)
        { 
            //GameManager.current.IsPlayerStart = true;
            animator.SetBool(TransitionParameters.Sprint.ToString(), true);
        } 
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    } 
}
