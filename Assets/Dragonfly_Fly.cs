using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dragonfly_Fly : StateMachineBehaviour
{
    Dragonfly enemy;
    LayerMask mask;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<Dragonfly>();
        mask |= (1 << 3); // add ground
        mask |= (1 << 6); // add player
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (GameObject.FindGameObjectWithTag("Dante") != null)
        {
            Vector3 dantePos = GameObject.FindGameObjectWithTag("Dante").transform.position;

            if (Vector3.Distance(animator.transform.position, dantePos) < enemy.detection_range)
            {
                animator.SetFloat("InRange", 1);
                RaycastHit2D hit = Physics2D.Raycast(animator.transform.position, dantePos - animator.transform.position, Mathf.Infinity, mask);
                if (hit && hit.collider.CompareTag("Dante"))
                {
                    animator.SetTrigger("Shoot");
                }
                else
                {
                    // nav to player
                }
            }
            else
            {
                animator.SetFloat("InRange", 0);
                if (Vector3.Distance(animator.transform.position, enemy.spawn) > 0.5f)
                {
                    animator.SetBool("InSpawn", false);
                    //nav to spawn
                }
                else
                {
                    animator.SetBool("InSpawn", true);
                    // move in circles
                }
            }
        }
        else
        {
            animator.SetFloat("InRange", 0);
            if (Vector3.Distance(animator.transform.position, enemy.spawn) > 0.5f)
            {
                animator.SetBool("InSpawn", false);
                //nav to spawn
            }
            else
            {
                animator.SetBool("InSpawn", true);
                // move in circles
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
