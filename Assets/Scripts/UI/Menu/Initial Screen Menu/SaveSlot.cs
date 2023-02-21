using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class SaveSlot : MonoBehaviour
{
    [SerializeField] string profileId;
    bool haveData;
    bool createOrLoad; // true --> create, false --> load
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI buttonText;
    public Button loadButton;
    public GameObject deleteButton;

    public void CreateOrLoad()
    {
        SlotsMenu slots = GetComponentInParent<SlotsMenu>();
        if (slots.option != MENU_ACTION_TYPE.NONE) return;
        if (createOrLoad) // create
        {
            if (haveData)
            {
                slots.AskForConfirmation(true, profileId);
            }
            else
            {
                DataPersistenceManager.instance.ChangeSelectedProfileId(profileId);
                slots.fade.FadeOn();
                slots.option = MENU_ACTION_TYPE.CREATE;
            }
        }
        else // load
        {
            DataPersistenceManager.instance.ChangeSelectedProfileId(profileId);
            slots.fade.FadeOn();
            slots.option = MENU_ACTION_TYPE.LOAD;
        }
    }

    public void Delete()
    {
        SlotsMenu slots = GetComponentInParent<SlotsMenu>();
        slots.AskForConfirmation(false, profileId);
    }

    public void SetSlot(GameData data, bool createOrLoad)
    {
        if (data == null)
        {
            haveData = false;
            if (createOrLoad)
            {
                loadButton.interactable = true;
                buttonText.text = "Create";
                timeText.text = "Empty";
                deleteButton.SetActive(false);
            }
            else
            {
                loadButton.interactable = false;
                buttonText.text = "Load";
                timeText.text = "Empty";
                deleteButton.SetActive(false);
            }
        }
        else
        {
            haveData = true;
            if (createOrLoad)
            {
                loadButton.interactable = true;
                buttonText.text = "Override";
                timeText.text = SecondsToTime(data.GetGameTime());
                deleteButton.SetActive(false);
            }
            else
            {
                loadButton.interactable = true;
                buttonText.text = "Load";
                timeText.text = SecondsToTime(data.GetGameTime());
                deleteButton.SetActive(true);
            }
        }

        this.createOrLoad = createOrLoad;
    }

    public string GetProfileId()
    {
        return profileId;
    }

    public void DisableButton()
    {
        buttonText.transform.parent.gameObject.SetActive(false);
    }

    string SecondsToTime(int seconds)
    {
        int hours = 0;
        int minutes = 0;

        while (seconds > 59)
        {
            minutes++;
            seconds -= 60;
        }
        while (minutes > 59)
        {
            hours++;
            minutes -= 60;
        }

        string time = "";

        if (hours > 9) time += hours;
        else if (hours > 0) time += "0" + hours;
        else time += "00";
        time += ":";

        if (minutes > 9) time += minutes;
        else if (minutes > 0) time += "0" + minutes;
        else time += "00";
        time += ":";

        if (seconds > 9) time += seconds;
        else if (seconds > 0) time += "0" + seconds;
        else time += "00";

        return time;
    }
}
