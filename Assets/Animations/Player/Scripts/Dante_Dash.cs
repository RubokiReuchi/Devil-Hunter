using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dante_Dash : StateMachineBehaviour
{
    Dante_StateMachine state;
    Dante_Movement dm;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        state = animator.GetComponent<Dante_StateMachine>();
        dm = animator.GetComponent<Dante_Movement>();
        animator.SetBool("Can AirDash", false);
        animator.ResetTrigger("Attack1");
        Dante_Attack.instance.canReceiveInput = true;
        Dante_Attack.instance.inputReceived = INPUT_RECEIVED.NONE;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        dm.DanteStop();
        state.dash = false;
    }
}
