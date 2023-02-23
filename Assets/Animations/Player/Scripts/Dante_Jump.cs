using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dante_Jump : StateMachineBehaviour
{
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        switch (Dante_Attack.instance.inputReceived)
        {
            case INPUT_RECEIVED.NONE:
                return;
            case INPUT_RECEIVED.A_LIGHT:
                animator.SetTrigger("AttackLightAir");
                Dante_Attack.instance.inputReceived = INPUT_RECEIVED.NONE;
                Dante_StateMachine.instance.SetState(DANTE_STATE.ATTACKING_AIR);
                break;
            case INPUT_RECEIVED.A_HEAVY:
                break;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Jump");
        Dante_Attack.instance.canReceiveInput = true;
    }
}
