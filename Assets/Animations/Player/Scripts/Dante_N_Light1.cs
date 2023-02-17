using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dante_N_Light1 : StateMachineBehaviour
{
    Dante_StateMachine state;
    bool cont = false;
    bool cont_heavy = false;
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
        if (dm.input.Attack1.WasPressedThisFrame())
        {
            if (!state.IsAiming())
            {
                animator.SetBool("Attack2", true);
                cont = true;
            }
            else
            {
                animator.SetBool("AttackHeavy1", true);
                cont_heavy = true;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack1");
        if (!cont)
        {
            dm.runSpeed = dm.basicRunSpeed;
            if (dm.isOnGround) state.SetState(DANTE_STATE.IDLE);
        }
        if (!cont_heavy)
        {
            dm.runSpeed = dm.basicRunSpeed;
        }
        cont = false;
        cont_heavy = false;
    }
}
