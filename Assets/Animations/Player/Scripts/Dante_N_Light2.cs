using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dante_N_Light2 : StateMachineBehaviour
{
    Dante_StateMachine state;
    Dante_Movement dm;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        state = animator.GetComponent<Dante_StateMachine>();
        dm = animator.GetComponent<Dante_Movement>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Attack2", false);
        dm.runSpeed = dm.basicRunSpeed;
        if (state.InGround()) state.SetState(DANTE_STATE.IDLE);
    }
}
