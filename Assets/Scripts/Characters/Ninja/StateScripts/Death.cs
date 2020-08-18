using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        NinjaController ninja = animator.GetComponentInParent<NinjaController>();
        animator.SetBool(TransitionParameters.Sprint.ToString(), false);
        animator.SetBool(TransitionParameters.Slide.ToString(), false);

        ninja.RIGID_BODY.velocity = Vector3.zero;
        ninja.RIGID_BODY.angularVelocity = Vector3.zero;
        ninja.RIGID_BODY.isKinematic = true;
         
        //GameMenu.current.Dead();
        GameManager.current.Timer.Stop();
        GameManager.current.IsPlayerStart = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
         
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 

    }
}
