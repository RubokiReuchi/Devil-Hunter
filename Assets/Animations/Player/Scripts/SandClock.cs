using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandClock : MonoBehaviour
{
    [NonEditable] public bool onClock;
    bool onMenu;

    // Start is called before the first frame update
    void Start()
    {
        // check if player in hitbox = onClock
        onMenu = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (onClock)
        {
            if (!onMenu && GameObject.FindGameObjectWithTag("Dante").GetComponent<Dante_Movement>().input.Aim.WasPressedThisFrame())
            {
                GameObject dante = GameObject.FindGameObjectWithTag("Dante");
                dante.GetComponent<Dante_StateMachine>().SetState(DANTE_STATE.INTERACT);
                dante.GetComponent<Animator>().SetTrigger("SandClockEnter");
                onMenu = true;
            }
            if (onMenu && GameObject.FindGameObjectWithTag("Dante").GetComponent<Dante_Movement>().input.Cancel.WasPressedThisFrame())
            {
                GameObject dante = GameObject.FindGameObjectWithTag("Dante");
                dante.GetComponent<Dante_StateMachine>().SetState(DANTE_STATE.IDLE);
                dante.GetComponent<Animator>().SetTrigger("SandClockExit");
                onMenu = false;
            }
        }
    }
}
