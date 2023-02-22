using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public Fade fade;
    bool returnToMenu;
    public Dante_Movement dm;

    // Start is called before the first frame update
    void Start()
    {
        returnToMenu = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (returnToMenu && fade.black)
        {
            DataPersistenceManager.instance.SaveGame();
            SceneManager.LoadScene("Main menu");
        }
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        dm.ResetAllTriggers();
        gameObject.SetActive(false);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1.0f;
        fade.FadeOn();
        returnToMenu = true;
    }
}
