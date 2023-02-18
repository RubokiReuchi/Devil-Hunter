using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    SELECTED option = SELECTED.NONE;

    void Update()
    {
        if (fade.black)
        {
            switch (option)
            {
                case SELECTED.NEW_GAME:
                    SceneManager.LoadScene("Tutorial Path");
                    break;
                case SELECTED.CONTINUE:
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
            option = SELECTED.NONE;
        }
    }

    public void NewGame()
    {
        fade.FadeOn();
        option = SELECTED.NEW_GAME;
    }

    public void Continue()
    {
        fade.FadeOn();
        option = SELECTED.CONTINUE;
    }

    public void Load()
    {
        fade.FadeOn();
        option = SELECTED.LOAD;
    }

    public void Exit()
    {
        fade.FadeOn();
        option = SELECTED.EXIT;
    }

    public void Settings()
    {
        fade.FadeOn();
        option = SELECTED.SETTINGS;
    }
}
