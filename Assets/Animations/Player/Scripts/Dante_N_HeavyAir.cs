using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dante_N_HeavyAir : StateMachineBehaviour
{
    Dante_StateMachine state;
    Transform transform;
    Dante_Movement dm;
    Rigidbody2D rb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Dante_Attack.instance.hit.damage = 35;
        state = animator.GetComponent<Dante_StateMachine>();
        transform = animator.transform;
        dm = animator.GetComponent<Dante_Movement>();
        rb = animator.GetComponent<Rigidbody2D>();
        rb.velocity = Vector3.zero;
        rb.AddForce(new Vector2(0, -200));
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
        Dante_Attack.instance.hit.damage = 0;
    }
}