using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dante_Grounging : StateMachineBehaviour
{
    Dante_StateMachine state;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        state = animator.GetComponent<Dante_StateMachine>();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Grounding");
        animator.ResetTrigger("AttackLightAir");
    }
}
