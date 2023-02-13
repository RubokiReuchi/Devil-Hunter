using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dante_Falling : StateMachineBehaviour
{
    Dante_StateMachine state;
    Transform transform;
    Dante_Movement dm;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        state = animator.GetComponent<Dante_StateMachine>();
        dm = animator.GetComponent<Dante_Movement>();

        if (state.IsRolling()) dm.iframe = !dm.iframe;

        state.SetState(DANTE_STATE.FALLING);
        transform = animator.transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Physics2D.BoxCast(transform.position, dm.boxSize, 0, -transform.up, dm.maxDistance, dm.layerMask))
        {
            animator.SetTrigger("Grounding");
            state.SetState(DANTE_STATE.IDLE);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
