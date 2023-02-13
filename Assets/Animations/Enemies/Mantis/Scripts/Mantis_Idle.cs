using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mantis_Idle : StateMachineBehaviour
{
    Enemy enemy;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<Enemy>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (GameObject.FindGameObjectWithTag("Dante") != null)
        {
            if (Vector3.Distance(animator.transform.position, GameObject.FindGameObjectWithTag("Dante").transform.position) < enemy.detection_melee)
            {
                animator.SetFloat("InRange", 2);
                if (enemy.orientation != 0) animator.transform.localScale = new Vector3(enemy.orientation, 1, 1);
            }
            else if (Vector3.Distance(animator.transform.position, GameObject.FindGameObjectWithTag("Dante").transform.position) < enemy.detection_range)
            {
                animator.SetFloat("InRange", 1);
                if (enemy.orientation != 0) animator.transform.localScale = new Vector3(enemy.orientation, 1, 1);
            }
            else
            {
                animator.SetFloat("InRange", 0);
                if (Vector3.Distance(animator.transform.position, enemy.spawn) > 0.5f) animator.SetBool("InSpawn", false);
            }
        }
        else if (Vector3.Distance(animator.transform.position, enemy.spawn) > 0.5f)
        {
            animator.SetFloat("InRange", 0);
            animator.SetBool("InSpawn", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
