using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dante_Heavy2 : StateMachineBehaviour
{
    Dante_StateMachine state;
    Dante_Movement dm;
    Rigidbody2D rb;
    SpriteRenderer sprite;

    float thrustSpeed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        state = animator.GetComponent<Dante_StateMachine>();
        dm = animator.GetComponent<Dante_Movement>();
        rb = animator.GetComponent<Rigidbody2D>();
        sprite = animator.GetComponent<SpriteRenderer>();

        if (state.demon) thrustSpeed = 22.0f;
        else thrustSpeed = 15.0f;

        state.dash = true;

        Dante_Attack.instance.hit.damage = 20;
        Dante_Attack.instance.canReceiveInput = false;
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
        state.dash = false;
        if (dm.isOnGround) state.SetState(DANTE_STATE.IDLE);
        else state.SetState(DANTE_STATE.FALLING);
        dm.DanteStop();

        Dante_Attack.instance.hit.damage = 0;
        Dante_Attack.instance.canReceiveInput = true;
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
