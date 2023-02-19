using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dante_SaveAndLoad : MonoBehaviour
{
    Dante_Stats stats;
    Dante_Skills skills;

    void Awake()
    {
        stats = GetComponent<Dante_Stats>();
        skills = GetComponent<Dante_Skills>();
    }

    public void SaveGame()
    {
        SaveSystem.SavePlayer(stats, skills);
    }

    public void LoadGame(bool newGame)
    {
        DanteData data;
        if (!newGame) data = SaveSystem.LoadPlayer();
        else data = SaveSystem.LoadPlayerNewGame();

        // position
        transform.position = new(data.position[0], data.position[1]);

        // stats
        stats.max_hp = data.maxHp;
        stats.attack = data.attack;
        stats.regen_hp = data.regenHp;
        stats.life_steal = data.lifeSteal;
        stats.knockback_resist = data.knockbackResist;
        stats.current_hp = data.currentHp;
        stats.maxLimitBatteries = data.maxLimitBatteries;
        stats.currentLimitValue = data.currentLimitValue;

        // skills
        skills.dashLevel = data.dashLevel;
        skills.wallSlidingUnlocked = data.wallSlidingUnlocked;
        skills.doubleJumpUnlocked = data.doubleJumpUnlocked;
        skills.reviveUnlocked = data.reviveUnlocked;
        skills.waveUnlocked = data.waveUnlocked;
        skills.grabUnlocked = data.grabUnlocked;
        skills.fallingAttackUnlocked = data.fallingAttackUnlocked;
        skills.thrustUnlocked = data.thrustUnlocked;

        SceneData sceneData = SaveSystem.LoadScene();
        transform.position = new Vector2(sceneData.positionOnScene[0], sceneData.positionOnScene[1]);
    }
}
