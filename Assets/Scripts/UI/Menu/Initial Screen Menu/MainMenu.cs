using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

enum SELECTED
{
    NONE,
    NEW_GAME,
    CONTINUE,
    LOAD,
    EXIT,
    SETTINGS
}

public class MainMenu : Menu
{
    public InitialScreenManager initialScreenManager;
    public Fade fade;
    SELECTED option;

    public Button continueButton;
    public Button loadButton;

    void Start()
    {
        option = SELECTED.NONE;
        OnEnter();
    }

    void Update()
    {
        switch (option)
        {
            case SELECTED.NEW_GAME:
                initialScreenManager.OpenMenu(ACTIVE_MENU.SAVE_SLOTS, true);
                break;
            case SELECTED.CONTINUE:
                if (!fade.black) return;
                DataPersistenceManager.instance.SaveGame();
                SceneManager.LoadScene(DataPersistenceManager.instance.GetCurrentScene());
                break;
            case SELECTED.LOAD:
                initialScreenManager.OpenMenu(ACTIVE_MENU.SAVE_SLOTS, false);
                break;
            case SELECTED.EXIT:
                Application.Quit();
                break;
            case SELECTED.SETTINGS:
                initialScreenManager.OpenMenu(ACTIVE_MENU.SETTINGS);
                break;
            default:
                break;
        }

        option = SELECTED.NONE;
    }

    public void NewGame()
    {
        if (option != SELECTED.NONE) return;
        option = SELECTED.NEW_GAME;
    }

    public void Continue()
    {
        if (option != SELECTED.NONE) return;
        fade.FadeOn();
        option = SELECTED.CONTINUE;
    }

    public void Load()
    {
        if (option != SELECTED.NONE) return;
        option = SELECTED.LOAD;
    }

    public void Exit()
    {
        if (option != SELECTED.NONE) return;
        option = SELECTED.EXIT;
    }

    public void Settings()
    {
        if (option != SELECTED.NONE) return;
        option = SELECTED.SETTINGS;
    }

    public void OnEnter()
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            continueButton.interactable = false;
            loadButton.interactable = false;
        }
    }
}
