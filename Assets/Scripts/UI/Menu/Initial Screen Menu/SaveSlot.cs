using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class SaveSlot : MonoBehaviour
{
    [SerializeField] string profileId;
    bool createOrLoad; // true --> create, false --> load
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI buttonText;
    public Button button;

    public void CreateOrLoad()
    {
        SlotsMenu slots = GetComponentInParent<SlotsMenu>();
        if (slots.option != MENU_ACTION_TYPE.NONE) return;
        slots.fade.FadeOn();
        DataPersistenceManager.instance.ChangeSelectedProfileId(profileId);
        if (createOrLoad) // create
        {
            slots.option = MENU_ACTION_TYPE.CREATE;
        }
        else // load
        {
            slots.option = MENU_ACTION_TYPE.LOAD;
        }
    }

    public void SetSlot(GameData data, bool createOrLoad)
    {
        if (data == null)
        {
            if (createOrLoad)
            {
                button.interactable = true;
                buttonText.text = "Create";
                timeText.text = "Empty";
            }
            else
            {
                button.interactable = false;
                buttonText.text = "Load";
                timeText.text = "Empty";
            }
        }
        else
        {
            if (createOrLoad)
            {
                button.interactable = true;
                buttonText.text = "Override";
                timeText.text = SecondsToTime(data.GetGameTime());
            }
            else
            {
                button.interactable = true;
                buttonText.text = "Load";
                timeText.text = SecondsToTime(data.GetGameTime());
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
