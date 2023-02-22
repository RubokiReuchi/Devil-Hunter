using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ITEM
{
    BLUE_EGGS_FRAGMENT,
    PURPLE_EGGS_FRAGMENT,
    GOLD_EGGS_FRAGMENT
}

public class Dante_Stats : Stats, DataPersistenceInterfice
{
    public float maxLimitBatteries;
    [NonEditable] public float currentLimitValue;

    LimitBattery limitBattery;

    public IntValue redEggs;
    public IntValue blueEggsFrag;
    public IntValue purpleEggsFrag;
    public IntValue goldEggsFrag;

    public void LoadData(GameData data)
    {
        max_hp = data.maxHp;
        attack = data.attack;
        regen_hp = data.regenHp;
        life_steal = data.lifeSteal;
        knockback_resist = data.knockbackResist;
        current_hp = data.currentHp;
        maxLimitBatteries = data.maxLimitBatteries;
        currentLimitValue = data.currentLimitValue;

        redEggs.value = data.redEggs;
        blueEggsFrag.value = data.blueEggsFrag;
        purpleEggsFrag.value = data.purpleEggsFrag;
        goldEggsFrag.value = data.goldEggsFrag;
    }

    public void SaveData(GameData data)
    {
        data.maxHp = max_hp;
        data.attack = attack;
        data.regenHp = regen_hp;
        data.lifeSteal = life_steal;
        data.knockbackResist = knockback_resist;
        data.currentHp = current_hp;
        data.maxLimitBatteries = maxLimitBatteries;
        data.currentLimitValue = currentLimitValue;

        data.redEggs = redEggs.value;
        data.blueEggsFrag = blueEggsFrag.value;
        data.purpleEggsFrag = purpleEggsFrag.value;
        data.goldEggsFrag = goldEggsFrag.value;
    }

    void Start()
    {
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

    public void GetItem(ITEM item)
    {
        switch (item)
        {
            case ITEM.BLUE_EGGS_FRAGMENT:
                blueEggsFrag.value++;
                if (blueEggsFrag.value == 4)
                {
                    blueEggsFrag.value = 0;
                    max_hp += 50;
                }
                current_hp = max_hp;
                break;
            case ITEM.PURPLE_EGGS_FRAGMENT:
                purpleEggsFrag.value++;
                if (purpleEggsFrag.value == 4)
                {
                    purpleEggsFrag.value = 0;
                    maxLimitBatteries += 1;
                    limitBattery.CalculeteLimit();
                }
                break;
            case ITEM.GOLD_EGGS_FRAGMENT:
                goldEggsFrag.value++;
                if (goldEggsFrag.value == 4)
                {
                    GetComponent<Dante_Skills>().reviveUnlocked = true;
                }
                break;
            default:
                break;
        }
    }
}
