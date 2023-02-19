using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DanteData
{
    // position
    public float[] position;

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

    public DanteData(Dante_Stats stats, Dante_Skills skills)
    {
        // position
        position = new float[2];
        position[0] = stats.transform.position.x;
        position[1] = stats.transform.position.y;

        // stats
        maxHp = stats.max_hp;
        attack = stats.attack;
        regenHp = stats.regen_hp;
        lifeSteal = stats.life_steal;
        knockbackResist = stats.knockback_resist;
        currentHp = stats.current_hp;
        maxLimitBatteries = stats.maxLimitBatteries;
        currentLimitValue = stats.currentLimitValue;

        // skills
        dashLevel = skills.dashLevel;
        wallSlidingUnlocked = skills.wallSlidingUnlocked;
        doubleJumpUnlocked = skills.doubleJumpUnlocked;
        reviveUnlocked = skills.reviveUnlocked;
        waveUnlocked = skills.waveUnlocked;
        grabUnlocked = skills.grabUnlocked;
        fallingAttackUnlocked = skills.fallingAttackUnlocked;
        thrustUnlocked = skills.thrustUnlocked;
    }
}
