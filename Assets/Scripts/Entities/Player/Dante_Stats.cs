using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dante_Stats : Stats
{
    public float maxLimitBatteries;
    [NonEditable] public float currentLimitValue;

    LimitBattery limitBattery;

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
