using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    #region Profile Data
    public int gameTime;
    public long lastUpdateTime;
    public string currentScene;
    public Vector3 lastClockPosition;
    public string lastClockScene;
    #endregion

    #region Player
    // position
    public Vector3 position;

    // stats
    public float maxHp;
    public float attack;
    public float regenHp;
    public float lifeSteal;
    public float knockbackResist;
    public float currentHp;
    public float maxLimitBatteries;
    public float currentLimitValue;

    // skills
    public int dashLevel;
    public bool wallSlidingUnlocked;
    public bool doubleJumpUnlocked;
    public bool reviveUnlocked;
    public bool canRevive;
    public bool attack3Unlocked;
    public bool attack4Unlocked;
    public bool attack5Unlocked;
    public bool combo1Unlocked;
    public bool combo2Unlocked;
    public bool combo3Unlocked;
    public bool combo4Unlocked;
    public bool fallingAttackUnlocked;
    public bool chargeUnlocked;
    public bool grabUnlocked;
    public bool ultUnlocked;
    #endregion

    #region ScriptableObjects
    public int redEggs;
    public int blueEggsFrag;
    public int purpleEggsFrag;
    public int goldEggsFrag;
    #region Shops
    // Sand Clock
    public int[] sandClockExistences = new int[2];
    public int[] sandClockPrice = new int[2];
    #endregion
    #endregion

    #region Red Stones
    public SerializableDictionary<string, bool> redStonesPicked;
    #endregion

    #region Item On Floor
    public SerializableDictionary<string, bool> itemOnFloorPicked;
    #endregion

    #region Enemies
    public SerializableDictionary<string, bool> enemiesDeath;
    #endregion

    public GameData() // new game values
    {
        #region Profile Data
        gameTime = 0;
        lastUpdateTime = System.DateTime.Now.ToBinary();
        currentScene = "Tutorial Path";
        lastClockPosition = new Vector2(0.0f, -0.8f);
        lastClockScene = "Tutorial Path";
        #endregion

        #region Player
        // position
        position = new Vector2(0.0f, -0.8f);

        // stats
        maxHp = 300;
        attack = 10;
        regenHp = 0;
        lifeSteal = 2;
        knockbackResist = 30;
        currentHp = 300;
        maxLimitBatteries = 0;
        currentLimitValue = 0;

        // skills
        dashLevel = 0;
        wallSlidingUnlocked = false;
        doubleJumpUnlocked = false;
        reviveUnlocked = false;
        canRevive = false;
        attack3Unlocked = false;
        attack4Unlocked = false;
        attack5Unlocked = false;
        combo1Unlocked = false;
        combo2Unlocked = false;
        combo3Unlocked = false;
        combo4Unlocked = false;
        chargeUnlocked = false;
        fallingAttackUnlocked = false;
        grabUnlocked = false;
        ultUnlocked = false;
        #endregion

        #region ScriptableObjects
        redEggs = 0;
        blueEggsFrag = 0;
        purpleEggsFrag = 0;
        goldEggsFrag = 0;
        #region Shops
        // Sand Clock
        sandClockExistences[0] = 8;
        sandClockExistences[1] = 12;
        sandClockPrice[0] = 300;
        sandClockPrice[1] = 1000;
        #endregion
        #endregion

        #region Red Stones
        redStonesPicked = new SerializableDictionary<string, bool>();
        #endregion

        #region Item OnFloor
        itemOnFloorPicked = new SerializableDictionary<string, bool>();
        #endregion

        #region Enemies
        enemiesDeath = new SerializableDictionary<string, bool>();
        #endregion
    }

    public int GetGameTime()
    {
        return gameTime;
    }

    public string GetCurrentScene()
    {
        return currentScene;
    }

    public string GetLastClockScene()
    {
        return lastClockScene;
    }
}
