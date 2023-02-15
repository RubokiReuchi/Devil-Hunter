using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dante_Thrust : StateMachineBehaviour
{
    Dante_StateMachine state;
    Dante_Movement dm;
    Rigidbody2D rb;

    float thrustSpeed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        state = animator.GetComponent<Dante_StateMachine>();
        dm = animator.GetComponent<Dante_Movement>();
        rb = animator.GetComponent<Rigidbody2D>();

        if (state.demon) thrustSpeed = 22.0f;
        else thrustSpeed = 15.0f;

        state.dash = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.velocity = dm.dashDirection * thrustSpeed;

        if (!dm.isOnGround) rb.velocity = new Vector2(0, rb.velocity.y);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Thrust", false);
        dm.runSpeed = dm.basicRunSpeed;
        dm.DanteStop();
        state.dash = false;
        if (dm.isOnGround) state.SetState(DANTE_STATE.IDLE);
        else state.SetState(DANTE_STATE.FALLING);
    }
}
