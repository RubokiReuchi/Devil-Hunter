using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
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

    #region Red Stones
    public Dictionary<string, bool> redStonesPicked;
    #endregion

    public GameData() // new game values
    {
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
    }
}
