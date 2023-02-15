using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dante_N_LightAir : StateMachineBehaviour
{
    Dante_StateMachine state;
    Rigidbody2D rb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        state = animator.GetComponent<Dante_StateMachine>();
        rb = animator.GetComponent<Rigidbody2D>();
        animator.SetBool("Can LightAir", false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.gravityScale = 0;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.gravityScale = 1;
        state.SetState(DANTE_STATE.FALLING);
    }
}
