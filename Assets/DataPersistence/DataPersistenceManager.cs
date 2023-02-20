using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] string fileName;
    [SerializeField] bool useEncryption;

    GameData gameData;
    List<DataPersistenceInterfice> dataPersistenceObjects;

    FileDataHandler dataHandler;

    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null) Debug.LogError("Found more than one Data Persistance Manager in the scene.");
        instance = this;
    }

    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
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

        if (gameData == null)
        {
            NewGame();
        }
        foreach (DataPersistenceInterfice dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
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
}
