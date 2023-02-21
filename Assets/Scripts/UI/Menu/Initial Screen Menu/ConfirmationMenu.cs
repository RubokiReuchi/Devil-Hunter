using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling;

public class ConfirmationMenu : Menu
{
    public TextMeshProUGUI questionText;
    bool overrideOrDelete;
    string profileId;

    public void SetConfirmation(bool overrideOrDelete, string profileId)
    {
        this.overrideOrDelete = overrideOrDelete;
        this.profileId = profileId;
        if (overrideOrDelete) questionText.text = "Starting a New Game with this slot will override the current saved data. Are you Sure?";
        else questionText.text = "You can't recover a deleted data. Are you Sure?";
    }

    public void Confirm()
    {
        SlotsMenu slots = GetComponentInParent<SlotsMenu>();
        if (overrideOrDelete)
        {
            DataPersistenceManager.instance.ChangeSelectedProfileId(profileId);
            slots.fade.FadeOn();
            slots.option = MENU_ACTION_TYPE.CREATE;
        }
        else
        {
            DataPersistenceManager.instance.DeleteProfileData(profileId);
            slots.SetSlots(false);
        }
        gameObject.SetActive(false);
    }

    public void Cancel()
    {
        gameObject.SetActive(false);
    }
}
