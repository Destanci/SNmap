using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprint : StateMachineBehaviour
{ 
    NinjaController ninja;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ninja = animator.GetComponentInParent<NinjaController>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ninja.vertical == 0 && ninja.horizontal == 0)
        {
            animator.SetBool(TransitionParameters.Sprint.ToString(), false);
            return;
        }

        if (ninja.IsSlideArea)
        {
            animator.SetBool(TransitionParameters.Slide.ToString(), true);
            return;
        }

    } 
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}