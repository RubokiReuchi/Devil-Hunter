using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MENUS
{
    INVENTORY,
    SHOP
}

public class Dante_Menus : MonoBehaviour
{
    Dante_Movement dm;
    Dante_StateMachine state;
    public GameObject menu;
    [HideInInspector] public bool onShop;
    bool onMenu;

    // Menus
    GameObject inventory;
    GameObject shop;

    // Start is called before the first frame update
    void Start()
    {
        dm = GetComponent<Dante_Movement>();
        state = GetComponent<Dante_StateMachine>();

        onMenu = false;
        inventory = menu.transform.Find("Inventory").gameObject;
        shop = menu.transform.Find("Shop").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // menus
        if (onMenu && dm.input.Cancel.WasPressedThisFrame())
        {
            onMenu = false;
            // continue world
            state.SetState(DANTE_STATE.IDLE);
            GetComponent<Animator>().SetTrigger("SandClockExit");
            // inventory
            inventory.SetActive(false);
            inventory.GetComponent<Inventory>().CleanSkillInfo();
            // shop
            shop.SetActive(false);
            shop.GetComponent<Shop>().DefaultShopInfo();
        }

        // open inventory
        if (!onMenu && dm.input.OpenInventory.WasPressedThisFrame())
        {
            onMenu = true;
            // pause world
            state.SetState(DANTE_STATE.INTERACT);
            GetComponent<Animator>().SetTrigger("SandClockEnter");
            StartCoroutine("Co_OpenMenu", MENUS.INVENTORY);
        }
        // open shop
        else if (!onMenu && onShop && dm.input.Aim.WasPressedThisFrame())
        {
            onMenu = true;
            // pause world
            state.SetState(DANTE_STATE.INTERACT);
            GetComponent<Animator>().SetTrigger("SandClockEnter");
            StartCoroutine("Co_OpenMenu", MENUS.SHOP);
        }
    }

    IEnumerator Co_OpenMenu(MENUS menu)
    {
        yield return new WaitForSeconds(0.2f);
        switch (menu)
        {
            case MENUS.INVENTORY:
                inventory.SetActive(true);
                break;
            case MENUS.SHOP:
                shop.SetActive(true);
                break;
            default:
                break;
        }
    }
}
