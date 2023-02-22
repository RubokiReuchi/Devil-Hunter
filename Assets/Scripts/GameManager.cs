using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, DataPersistenceInterfice
{
    [HideInInspector] public int timePlayed;
    [HideInInspector] public Vector3 lastClockPosition;
    [HideInInspector] public string lastClockScene;
    public Fade fade;
    [HideInInspector] public bool resetLevel;

    private void Awake()
    {
        StartCoroutine("SaveEackTeenSeconds");
    }

    public void LoadData(GameData data)
    {
        timePlayed = data.gameTime;
        lastClockPosition = data.lastClockPosition;
        lastClockScene = data.lastClockScene;
    }

    public void SaveData(GameData data)
    {
        data.gameTime = timePlayed;
        data.currentScene = SceneManager.GetActiveScene().name;
        data.lastClockPosition = lastClockPosition;
        data.lastClockScene = lastClockScene;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("CountTimePlayed");
        resetLevel = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (resetLevel && fade.black)
        {
            DataPersistenceManager.instance.SaveGame();
            SceneManager.LoadScene(lastClockScene);
        }
    }
    public void ResetLevel()
    {
        resetLevel = true;
        fade.FadeOn();
    }

    IEnumerator SaveEackTeenSeconds()
    {
        DataPersistenceManager.instance.SaveGame();
        yield return new WaitForSecondsRealtime(10.0f);
        StartCoroutine("SaveEackTeenSeconds");
    }

    IEnumerator CountTimePlayed()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        timePlayed++;
        StartCoroutine("CountTimePlayed");
    }
}
