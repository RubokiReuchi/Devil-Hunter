using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, DataPersistenceInterfice
{
    // used to check if dante exists
    [HideInInspector] public int timePlayed;
    [NonEditable] public bool danteState;
    [HideInInspector] public bool reviving;
    
    public HealthBar healthBar;
    public LimitBattery limitBattery;

    public GameObject dante_prefab;
    public Transform dante_spawn;
    Vector3 dante_position;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        dante_position = dante_spawn.position;
        reviving = false; //ReviveDante();
        StartCoroutine("CountTimePlayed");
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

    IEnumerator SaveEackTeenSeconds()
    {
        yield return new WaitForSecondsRealtime(10.0f);
        DataPersistenceManager.instance.SaveGame();
        StartCoroutine("SaveEackTeenSeconds");
    }

    IEnumerator CountTimePlayed()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        timePlayed++;
        StartCoroutine("CountTimePlayed");
    }
}
