using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Dragonfly_Fall : StateMachineBehaviour
{
    Dragonfly enemy;
    Rigidbody2D rb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<Dragonfly>();
        rb = animator.GetComponent<Rigidbody2D>();

        rb.gravityScale = 0.75f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Physics2D.BoxCast(animator.transform.position, enemy.boxSize, 0, -animator.transform.up, enemy.maxDistance, enemy.layerMask))
        {
            animator.SetTrigger("OnGround");
            animator.SetBool("Death", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
