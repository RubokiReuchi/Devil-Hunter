using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dante_Falling : StateMachineBehaviour
{
    Dante_StateMachine state;
    Dante_Movement dm;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        state = animator.GetComponent<Dante_StateMachine>();
        dm = animator.GetComponent<Dante_Movement>();

        dm.iframe = false;

        state.SetState(DANTE_STATE.FALLING);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (dm.isOnGround)
        {
            animator.SetTrigger("Grounding");
            state.SetState(DANTE_STATE.IDLE);
        }

        switch (Dante_Attack.instance.inputReceived)
        {
            case INPUT_RECEIVED.NONE:
                break;
            case INPUT_RECEIVED.A_LIGHT:
                animator.SetTrigger("AttackLightAir");
                Dante_Attack.instance.inputReceived = INPUT_RECEIVED.NONE;
                Dante_StateMachine.instance.SetState(DANTE_STATE.ATTACKING_AIR);
                break;
            case INPUT_RECEIVED.A_HEAVY:
                animator.SetTrigger("AttackHeavyAir");
                Dante_Attack.instance.inputReceived = INPUT_RECEIVED.NONE;
                Dante_StateMachine.instance.SetState(DANTE_STATE.ATTACKING_FALLING);
                break;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Dante_Attack.instance.canReceiveInput = true;
    }
}
