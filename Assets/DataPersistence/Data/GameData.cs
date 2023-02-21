using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    #region Profile Data
    public int gameTime;
    public long lastUpdateTime;
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
    public bool waveUnlocked;
    public bool grabUnlocked;
    public bool fallingAttackUnlocked;
    public bool thrustUnlocked;
    #endregion

    #region ScriptableObjects
    public int redEggs;
    public int blueEggsFrag;
    public int purpleEggsFrag;
    public int goldEggsFrag;
    #endregion

    #region Red Stones
    public SerializableDictionary<string, bool> redStonesPicked;
    #endregion

    #region Enemies
    public SerializableDictionary<string, bool> enemiesDeath;
    #endregion

    public GameData() // new game values
    {
        #region Profile Data
        gameTime = 0;
        lastUpdateTime = System.DateTime.Now.ToBinary();
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
        waveUnlocked = false;
        grabUnlocked = false;
        fallingAttackUnlocked = false;
        thrustUnlocked = false;
        #endregion

        #region ScriptableObjects
        redEggs = 0;
        blueEggsFrag = 0;
        purpleEggsFrag = 0;
        goldEggsFrag = 0;
        #endregion

        #region Red Stones
        redStonesPicked = new SerializableDictionary<string, bool>();
        #endregion

        #region Enemies
        enemiesDeath = new SerializableDictionary<string, bool>();
        #endregion
    }

    public int GetGameTime()
    {
        return gameTime;
    }
}
