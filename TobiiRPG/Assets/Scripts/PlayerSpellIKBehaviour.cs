using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellIKBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (UIManager.Target == null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
            animator.SetLookAtWeight(0);
            return;
        }

        animator.SetLookAtPosition(UIManager.Target.position);
        animator.SetLookAtWeight(animator.GetFloat("LookAt"), .5f, .75f);
        animator.SetIKPosition(AvatarIKGoal.RightHand, UIManager.Target.position);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, animator.GetFloat("LookAt") * 0.5f);
    }
}
