using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ACTIVE_MENU
{
    MAIN,
    SETTINGS,
    SAVE_SLOTS
}

public class InitialScreenManager : MonoBehaviour
{
    [NonEditable] ACTIVE_MENU activeMenu;
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject saveSlotsMenu;

    // Start is called before the first frame update
    void Start()
    {
        mainMenu.SetActive(true);
        activeMenu = ACTIVE_MENU.MAIN;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel")) OpenMenu(ACTIVE_MENU.MAIN);
    }

    public void OpenMenu(ACTIVE_MENU menuToOpen, bool createOrLoad = false)
    {
        CloseAllMenus();
        activeMenu = menuToOpen;
        switch (menuToOpen)
        {
            case ACTIVE_MENU.MAIN:
                mainMenu.SetActive(true);
                break;
            case ACTIVE_MENU.SETTINGS:
                settingsMenu.SetActive(true);
                break;
            case ACTIVE_MENU.SAVE_SLOTS:
                saveSlotsMenu.SetActive(true);
                saveSlotsMenu.GetComponent<SlotsMenu>().SetSlots(createOrLoad);
                break;
            default:
                break;
        }
    }

    void CloseAllMenus()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        saveSlotsMenu.SetActive(false);
    }
}
