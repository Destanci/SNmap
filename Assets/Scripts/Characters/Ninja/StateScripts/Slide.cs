using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

public class Slide : StateMachineBehaviour
{ 
    NinjaController ninja; 

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ninja = animator.GetComponentInParent<NinjaController>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
        if (!ninja.IsSlideArea)
        {
            animator.SetBool(TransitionParameters.Slide.ToString(), false);
            return;
        }
    } 

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    } 
} 