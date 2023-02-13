using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMenu : MonoBehaviour
{
    GameManager gameManager;
    public GameObject deathMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.reviving) deathMenuUI.SetActive(!gameManager.danteState);
    }

    public void UseGoldEgg()
    {
        // what ever
        gameManager.ReviveDante();
        deathMenuUI.SetActive(false);
    }

    public void UseRedEggs()
    {
        // what ever
        gameManager.ReviveDante();
        deathMenuUI.SetActive(false);
    }

    public void LastCheckpoint()
    {
        // what ever
        gameManager.ReviveDante();
        deathMenuUI.SetActive(false);
    }
}
