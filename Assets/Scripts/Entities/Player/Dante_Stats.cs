using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dante_Stats : Stats, DataPersistenceInterfice
{
    public float maxLimitBatteries;
    [NonEditable] public float currentLimitValue;

    LimitBattery limitBattery;

    public void LoadData(GameData data)
    {
        transform.position = data.position;

        max_hp = data.maxHp;
        attack = data.attack;
        regen_hp = data.regenHp;
        life_steal = data.lifeSteal;
        knockback_resist = data.knockbackResist;
        current_hp = data.currentHp;
        maxLimitBatteries = data.maxLimitBatteries;
        currentLimitValue = data.currentLimitValue;
    }

    public void SaveData(ref GameData data)
    {
        data.position = transform.position;

        data.maxHp = max_hp;
        data.attack = attack;
        data.regenHp = regen_hp;
        data.lifeSteal = life_steal;
        data.knockbackResist = knockback_resist;
        data.currentHp = current_hp;
        data.maxLimitBatteries = maxLimitBatteries;
        data.currentLimitValue = currentLimitValue;
    }

    void Start()
    {
        current_hp = max_hp;
        update = true;

        limitBattery = GameObject.FindGameObjectWithTag("Limit Battery").GetComponent<LimitBattery>();
    }

    // Update is called once per frame
    void Update()
    {
        if (update)
        {
            StartCoroutine("EachSecond");
        }
    }

    public void AddLimit(float amount)
    {
        currentLimitValue += amount;

        if (currentLimitValue > maxLimitBatteries)
        {
            currentLimitValue = maxLimitBatteries;
        }

        limitBattery.CalculeteLimit();
    }

    public void UseLimit(float cost)
    {
        currentLimitValue -= cost;

        if (currentLimitValue < 0)
        {
            currentLimitValue = 0;
        }

        limitBattery.CalculeteLimit();
    }
}
