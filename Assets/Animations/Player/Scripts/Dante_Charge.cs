using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dante_Charge : StateMachineBehaviour
{
    Dante_Movement dm;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        dm = animator.GetComponent<Dante_Movement>();
        Dante_Attack.instance.canReceiveInput = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!Dante_StateMachine.instance.demon) Dante_Attack.instance.chargeForce += Time.deltaTime;
        else Dante_Attack.instance.chargeForce += Time.deltaTime * 1.5f;

        if (dm.input.Attack2.WasReleasedThisFrame())
        {
            if (Dante_Attack.instance.chargeForce < 0.1f) Dante_Attack.instance.chargeForce = 0;
            animator.SetTrigger("Discharge");
        }
        else if (Dante_Attack.instance.chargeForce > 3)
        {
            Dante_Attack.instance.chargeForce = 3;
            animator.SetTrigger("Discharge");
        }
    }

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
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
