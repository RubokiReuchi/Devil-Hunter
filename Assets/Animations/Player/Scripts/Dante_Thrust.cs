using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dante_Thrust : StateMachineBehaviour
{
    Dante_StateMachine state;
    Dante_Movement dm;
    Rigidbody2D rb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        state = animator.GetComponent<Dante_StateMachine>();
        dm = animator.GetComponent<Dante_Movement>();
        rb = animator.GetComponent<Rigidbody2D>();

        float thrustForce;
        if (state.demon) thrustForce = 40.0f;
        else thrustForce = 20.0f;
        animator.GetComponent<Rigidbody2D>().AddForce(Vector2.right * thrustForce * animator.transform.localScale.x, ForceMode2D.Impulse);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Thrust", false);
        dm.runSpeed = dm.basicRunSpeed;
        if (state.InGround()) state.SetState(DANTE_STATE.IDLE);
        else state.SetState(DANTE_STATE.FALLING);
    }
}
