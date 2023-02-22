using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, DataPersistenceInterfice
{
    [HideInInspector] public int timePlayed;
    
    public HealthBar healthBar;
    public LimitBattery limitBattery;

    public GameObject dante_prefab;

    private void Awake()
    {
        StartCoroutine("SaveEackTeenSeconds");
    }

    public void LoadData(GameData data)
    {
        timePlayed = data.gameTime;
    }

    public void SaveData(GameData data)
    {
        data.gameTime = timePlayed;
        data.currentScene = SceneManager.GetActiveScene().name;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("CountTimePlayed");
    }

    // Update is called once per frame
    void Update()
    {
        
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
