using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ACTIVE_MENU
{
    MAIN,
    SETTINGS,
    REBIND_KEYBOARD,
    SAVE_SLOTS,
}

public class InitialScreenManager : MonoBehaviour
{
    [NonEditable] ACTIVE_MENU activeMenu;
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject rebindKeyboardMenu;
    public GameObject saveSlotsMenu;
    public GameObject confirmationMenu;

    // Start is called before the first frame update
    void Start()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        rebindKeyboardMenu.SetActive(false);
        saveSlotsMenu.SetActive(false);
        confirmationMenu.SetActive(false);
        activeMenu = ACTIVE_MENU.MAIN;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && !confirmationMenu.activeSelf)
        {
            if (rebindKeyboardMenu.activeInHierarchy) OpenMenu(ACTIVE_MENU.SETTINGS);
            else OpenMenu(ACTIVE_MENU.MAIN);
        }
    }

    public void OpenMenu(ACTIVE_MENU menuToOpen, bool createOrLoad = false)
    {
        CloseAllMenus();
        activeMenu = menuToOpen;
        switch (menuToOpen)
        {
            case ACTIVE_MENU.MAIN:
                mainMenu.SetActive(true);
                mainMenu.GetComponent<MainMenu>().OnEnter();
                break;
            case ACTIVE_MENU.SETTINGS:
                settingsMenu.SetActive(true);
                break;
            case ACTIVE_MENU.REBIND_KEYBOARD:
                rebindKeyboardMenu.SetActive(true);
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
        rebindKeyboardMenu.SetActive(false);
        saveSlotsMenu.SetActive(false);
    }
}
