using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MENUS
{
    PAUSE,
    INVENTORY,
    ITEM_STORAGE,
    SAND_CLOCK,
    MAP
}

public class Dante_Menus : MonoBehaviour
{
    Dante_Movement dm;
    Dante_StateMachine state;
    public GameObject menu;
    [HideInInspector] public bool onShop;
    bool onMenu;

    // Menus
    GameObject pauseMenu;
    GameObject inventory;
    GameObject itemStorage;
    GameObject shop;
    GameObject map;

    // Start is called before the first frame update
    void Start()
    {
        dm = GetComponent<Dante_Movement>();
        state = GetComponent<Dante_StateMachine>();

        onMenu = false;
        pauseMenu = menu.transform.Find("Pause").gameObject;
        inventory = menu.transform.Find("Inventory").gameObject;
        itemStorage = menu.transform.Find("Item Storage").gameObject;
        shop = menu.transform.Find("Shop").gameObject;
        map = menu.transform.Find("Map").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!state.IsAlive() && dm.isOnGround) return;

        // close menus
        if (onMenu && InputManager.instance.input.Cancel.WasPressedThisFrame())
        {
            onMenu = false;
            ResumeGame();
            state.SetState(DANTE_STATE.IDLE);
            GetComponent<Animator>().SetTrigger("SandClockExit");
            // pause menu
            pauseMenu.SetActive(false);
            // inventory
            inventory.SetActive(false);
            inventory.GetComponent<Inventory>().CleanSkillInfo();
            // item storage
            itemStorage.SetActive(false);
            // shop
            shop.SetActive(false);
            shop.GetComponent<Shop>().DefaultShopInfo();
            // map
            map.SetActive(false);
        }
        // open pause menu
        else if (!onMenu && InputManager.instance.input.Cancel.WasPressedThisFrame())
        {
            onMenu = true;
            state.SetState(DANTE_STATE.INTERACT);
            GetComponent<Animator>().SetTrigger("SandClockEnter");
            StartCoroutine("Co_OpenMenu", MENUS.PAUSE);
        }
        // open inventory
        if (!onMenu && InputManager.instance.input.OpenInventory.WasPressedThisFrame())
        {
            onMenu = true;
            state.SetState(DANTE_STATE.INTERACT);
            GetComponent<Animator>().SetTrigger("SandClockEnter");
            StartCoroutine("Co_OpenMenu", MENUS.INVENTORY);
        }
        // open item storage
        if (!onMenu && InputManager.instance.input.OpenItemStorage.WasPressedThisFrame())
        {
            onMenu = true;
            state.SetState(DANTE_STATE.INTERACT);
            GetComponent<Animator>().SetTrigger("SandClockEnter");
            StartCoroutine("Co_OpenMenu", MENUS.ITEM_STORAGE);
        }
        // open sand clock
        else if (!onMenu && onShop && InputManager.instance.input.Aim.WasPressedThisFrame())
        {
            onMenu = true;
            GetComponent<Dante_Stats>().Heal(GetComponent<Dante_Stats>().max_hp);
            state.SetState(DANTE_STATE.INTERACT);
            GetComponent<Animator>().SetTrigger("SandClockEnter");
            StartCoroutine("Co_OpenMenu", MENUS.SAND_CLOCK);
        }
        // open map
        if (!onMenu && InputManager.instance.input.OpenMap.WasPressedThisFrame())
        {
            onMenu = true;
            state.SetState(DANTE_STATE.INTERACT);
            GetComponent<Animator>().SetTrigger("SandClockEnter");
            StartCoroutine("Co_OpenMenu", MENUS.MAP);
        }
    }

    IEnumerator Co_OpenMenu(MENUS menu)
    {
        yield return new WaitForSeconds(0.2f);
        switch (menu)
        {
            case MENUS.PAUSE:
                pauseMenu.SetActive(true);
                break;
            case MENUS.INVENTORY:
                inventory.SetActive(true);
                break;
            case MENUS.ITEM_STORAGE:
                itemStorage.SetActive(true);
                itemStorage.GetComponent<ItemStorageMenu>().SetData();
                break;
            case MENUS.SAND_CLOCK:
                shop.SetActive(true);
                break;
            case MENUS.MAP:
                map.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
    }
}
