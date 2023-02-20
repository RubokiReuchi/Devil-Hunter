using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

enum DEBUG
{
    NONE,
    INIT_DATA,
    DO_NOT_LOAD
}

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] DEBUG debug = DEBUG.NONE;

    [Header("File Storage Config")]
    [SerializeField] string fileName;
    [SerializeField] bool useEncryption;

    GameData gameData;
    List<DataPersistenceInterfice> dataPersistenceObjects;

    FileDataHandler dataHandler;

    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        SaveGame();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();

        if (gameData == null && debug == DEBUG.INIT_DATA)
        {
            NewGame();
        }

        if (gameData == null || debug == DEBUG.DO_NOT_LOAD)
        {
            return;
        }

        foreach (DataPersistenceInterfice dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        if (gameData == null)
        {
            return;
        }

        foreach (DataPersistenceInterfice dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
    }

    List<DataPersistenceInterfice> FindAllDataPersistenceObjects()
    {
        IEnumerable<DataPersistenceInterfice> dataPersObjects = FindObjectsOfType<MonoBehaviour>().OfType<DataPersistenceInterfice>();

        return new List<DataPersistenceInterfice>(dataPersObjects);
    }

    public bool HasGameData()
    {
        return gameData != null;
    }
}
