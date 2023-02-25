using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dante_Discharge : StateMachineBehaviour
{
    Hit hit;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hit = Dante_Attack.instance.chargePs.GetComponentInParent<Hit>();
        hit.damage = 20 + 30 * Dante_Attack.instance.chargeForce;
        Dante_Attack.instance.chargeForce = 0;
        animator.SetBool("Charging", false);
        Dante_Attack.instance.chargePs.Play();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hit.damage = 0;
        Dante_Attack.instance.WaitUntilNextAttack();
    }

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
