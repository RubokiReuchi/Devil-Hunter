using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // used to check if dante exists
    [NonEditable] public bool danteState;
    [HideInInspector] public bool reviving;
    
    public HealthBar healthBar;
    public LimitBattery limitBattery;

    public GameObject dante_prefab;
    public Transform dante_spawn;
    Vector3 dante_position;

    #region SaveAndLoad
    private void Awake()
    {
        StartCoroutine("Co_SaveGame");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
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

    IEnumerator Co_SaveGame()
    {
        yield return new WaitForSeconds(10.0f);
        SaveGame();
        StartCoroutine("Co_SaveGame");
    }

    void SaveGame()
    {
        Debug.Log("Game Saved");
        if (GameObject.FindGameObjectWithTag("Dante") != null)
        {
            GameObject.FindGameObjectWithTag("Dante").SendMessage("SaveGame");
        }
    }

    void LoadGame()
    {
        Debug.Log("Game Loaded");
        if (GameObject.FindGameObjectWithTag("Dante") != null)
        {
            GameObject.FindGameObjectWithTag("Dante").SendMessage("LoadGame", true);
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        dante_position = dante_spawn.position;
        reviving = false; //ReviveDante();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Dante") != null) danteState = true;
        else danteState = false;
    }



    public void SaveDeathPos(Vector3 dante_pos)
    {
        dante_position = dante_pos;
    }

    public void ReviveDante()
    {
        reviving = true;
        StartCoroutine("Co_ReviveDante");
    }

    IEnumerator Co_ReviveDante()
    {
        yield return new WaitForSeconds(1);
        reviving = false;
        GameObject dante = Instantiate(dante_prefab, dante_position, Quaternion.identity);
        dante.GetComponent<Dante_StateMachine>().target = GameObject.FindGameObjectWithTag("Target").GetComponent<SpriteRenderer>();
        dante.GetComponent<Dante_Movement>().game_manager = this;
        GetComponentInChildren<CinemachineVirtualCamera>().Follow = dante.transform;
        healthBar.danteStats = dante.GetComponent<Dante_Stats>();
        healthBar.ResetHealthBar();
        limitBattery.danteStats = dante.GetComponent<Dante_Stats>();
        limitBattery.CalculeteLimit();
    }

    public void ResetLevel()
    {

    }
}
