using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mantis_Walk : StateMachineBehaviour
{
    Mantis enemy;
    Rigidbody2D rb;
    LayerMask mask;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<Mantis>();
        rb = animator.GetComponent<Rigidbody2D>();
        mask |= (1 << 3); // add ground
        mask |= (1 << 6); // add player
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetFloat("InRange") == 0)
        {
            float direction = (animator.transform.position.x - enemy.spawn.x) > 0 ? -1 : 1;
            animator.transform.position += new Vector3(direction * enemy.speed * Time.deltaTime, 0, 0);
            animator.transform.localScale = new Vector3(direction, 1, 1);

            if (Vector3.Distance(animator.transform.position, enemy.spawn) < 0.5f)
            {
                animator.SetBool("InSpawn", true);
            }

            if (GameObject.FindGameObjectWithTag("Dante") == null) return;
            Vector3 dantePos = GameObject.FindGameObjectWithTag("Dante").transform.position;
            RaycastHit2D hit = Physics2D.Raycast(animator.transform.position, dantePos - animator.transform.position, Mathf.Infinity, mask);
            if (Vector3.Distance(animator.transform.position, dantePos) < enemy.detection_range && hit && hit.collider.CompareTag("Dante"))
            {
                animator.SetFloat("InRange", 1);
                if (enemy.orientation != 0) animator.transform.localScale = new Vector3(enemy.orientation, 1, 1);
            }
        }
        else
        {
            animator.transform.localScale = new Vector3(enemy.orientation, 1, 1);

            if (GameObject.FindGameObjectWithTag("Dante") == null) return;

            Vector3 dantePos = GameObject.FindGameObjectWithTag("Dante").transform.position;
            RaycastHit2D hit = Physics2D.Raycast(animator.transform.position, dantePos - animator.transform.position, Mathf.Infinity, mask);
            if (hit && hit.collider.CompareTag("Dante"))
            {
                if (Vector3.Distance(animator.transform.position, dantePos) > enemy.detection_melee)
                {
                    animator.SetFloat("InRange", 1);
                }

                if (Vector3.Distance(animator.transform.position, dantePos) >= enemy.attackAtDistance)
                {
                    if ((enemy.orientation == -1 && enemy.restrict_left) || (enemy.orientation == 1 && enemy.restrict_right)) { }
                    else animator.transform.position += new Vector3(enemy.orientation * enemy.speed * Time.deltaTime, 0, 0);
                }

                if (Vector3.Distance(animator.transform.position, dantePos) < enemy.attackAtDistance)
                {
                    animator.SetTrigger("AttackMelee");
                }
            }
            else
            {
                animator.SetFloat("InRange", 1);
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
