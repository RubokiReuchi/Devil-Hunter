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

public class MainMenu : MonoBehaviour
{
    public Fade fade;
    SELECTED option;

    public Button continueButton;

    void Start()
    {
        option = SELECTED.NONE;
        if (!DataPersistenceManager.instance.HasGameData()) continueButton.interactable = false;
    }

    void Update()
    {
        if (fade.black)
        {
            switch (option)
            {
                case SELECTED.NEW_GAME:
                    DataPersistenceManager.instance.NewGame();
                    SceneManager.LoadScene("Tutorial Path");
                    break;
                case SELECTED.CONTINUE:
                    SceneManager.LoadScene("Tutorial Path");
                    break;
                case SELECTED.LOAD:
                    break;
                case SELECTED.EXIT:
                    Application.Quit();
                    break;
                case SELECTED.SETTINGS:
                    break;
                default:
                    break;
            }
        }
    }

    public void NewGame()
    {
        if (option != SELECTED.NONE) return;
        fade.FadeOn();
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
        fade.FadeOn();
        option = SELECTED.LOAD;
    }

    public void Exit()
    {
        if (option != SELECTED.NONE) return;
        fade.FadeOn();
        option = SELECTED.EXIT;
    }

    public void Settings()
    {
        if (option != SELECTED.NONE) return;
        fade.FadeOn();
        option = SELECTED.SETTINGS;
    }
}
