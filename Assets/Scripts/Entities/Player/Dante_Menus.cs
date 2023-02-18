using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dante_Menus : MonoBehaviour
{
    Dante_Movement dm;
    Dante_StateMachine state;
    public GameObject menu;
    bool onMenu;

    // Menus
    GameObject inventory;

    // Start is called before the first frame update
    void Start()
    {
        dm = GetComponent<Dante_Movement>();
        state = GetComponent<Dante_StateMachine>();

        onMenu = false;
        inventory = menu.transform.Find("Inventory").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (onMenu && dm.input.Cancel.WasPressedThisFrame())
        {
            onMenu = false;
            // continue world
            state.SetState(DANTE_STATE.IDLE);
            inventory.SetActive(false);
            inventory.GetComponent<Inventory>().CleanSkillInfo();
        }

        if (dm.input.OpenInventory.WasPressedThisFrame())
        {
            onMenu = true;
            // pause world
            state.SetState(DANTE_STATE.INTERACT);
            inventory.SetActive(true);
        }
    }
}
