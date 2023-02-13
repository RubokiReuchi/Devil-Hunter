using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dante_Static : StateMachineBehaviour
{
    Dante_Movement dm;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        dm = animator.GetComponent<Dante_Movement>();
        dm.walkSpeed = 0;
        dm.runSpeed = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        dm.walkSpeed = dm.basicWalkSpeed;
        dm.runSpeed = dm.basicRunSpeed;
    }
}
