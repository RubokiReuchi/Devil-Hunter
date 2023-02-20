using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MENU_ACTION_TYPE
{
    NONE,
    CREATE,
    LOAD,
}

public class SlotsMenu : Menu
{
    public SaveSlot[] saveSlots;
    public Fade fade;
    [NonEditable] public MENU_ACTION_TYPE option;

    void Update()
    {
        if (fade.black)
        {
            switch (option)
            {
                case MENU_ACTION_TYPE.CREATE:
                    DataPersistenceManager.instance.NewGame();
                    SceneManager.LoadScene("Tutorial Path");
                    break;
                case MENU_ACTION_TYPE.LOAD:
                    SceneManager.LoadScene("Tutorial Path");
                    break;
                default:
                    break;
            }

            option = MENU_ACTION_TYPE.NONE;
        }
    }

    public void SetSlots(bool createOrLoad)
    {
        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.instance.GetAllProfilesGameData();

        foreach (SaveSlot slot in saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(slot.GetProfileId(), out profileData);

            slot.SetSlot(profileData, createOrLoad);
        }
    }

    public void DisableButtons()
    {
        foreach (SaveSlot slot in saveSlots)
        {
            slot.DisableButton();
        }
    }
}
