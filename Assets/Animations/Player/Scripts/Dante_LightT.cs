using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Dante_LightT : StateMachineBehaviour
{
    Dante_Movement dm;
    public int attackNum;
    bool canContinue;
    bool canCombo;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        dm = animator.GetComponent<Dante_Movement>();

        switch (attackNum)
        {
            case 2: canContinue = Dante_Skills.instance.attack3Unlocked; break;
            case 3: canContinue = Dante_Skills.instance.attack4Unlocked; break;
            case 4: canContinue = Dante_Skills.instance.attack5Unlocked; break;
            default: canContinue = true; break;
        }
        switch (attackNum)
        {
            case 1: canCombo = Dante_Skills.instance.combo1Unlocked; break;
            case 2: canCombo = Dante_Skills.instance.combo2Unlocked; break;
            case 3: canCombo = Dante_Skills.instance.combo3Unlocked; break;
            case 4: canCombo = Dante_Skills.instance.combo4Unlocked; break;
            default: canCombo = true; break;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (InputManager.instance.input.Move.ReadValue<float>() > 0) animator.transform.localScale = new Vector3(1, 1, 1);
        else if (InputManager.instance.input.Move.ReadValue<float>() < 0) animator.transform.localScale = new Vector3(-1, 1, 1);

        switch (Dante_Attack.instance.inputReceived)
        {
            case INPUT_RECEIVED.NONE:
                break;
            case INPUT_RECEIVED.G_LIGHT:
                if (canContinue)
                {
                    animator.SetTrigger("Attack1");
                    Dante_StateMachine.instance.SetState(DANTE_STATE.ATTACKING_GROUND);
                }
                Dante_Attack.instance.canReceiveInput = true;
                Dante_Attack.instance.inputReceived = INPUT_RECEIVED.NONE;
                break;
            case INPUT_RECEIVED.G_HEAVY:
                if (canCombo)
                {
                    animator.SetTrigger("Attack2");
                    Dante_StateMachine.instance.SetState(DANTE_STATE.ATTACKING_GROUND);
                }
                Dante_Attack.instance.canReceiveInput = true;
                Dante_Attack.instance.inputReceived = INPUT_RECEIVED.NONE;
                break;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
