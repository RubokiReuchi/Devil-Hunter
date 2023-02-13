using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LimitBattery : MonoBehaviour
{
    public Color32 fullLimitColor;
    public Color32 defaultLimitColor;

    public LimitBar[] limitBars;
    public Dante_Stats danteStats;
    Dante_StateMachine danteState;
    public GameObject[] fireIndicators;

    // Start is called before the first frame update
    void Start()
    {
        danteState = danteStats.GetComponent<Dante_StateMachine>();
        CalculeteLimit();
    }

    public void CalculeteLimit()
    {
        // Display Limit Bars
        int i;
        for (i = 0; i < danteStats.maxLimitBatteries; i++)
        {
            limitBars[i].gameObject.SetActive(true);
            limitBars[i].Start();
        }
        for (int j = i; j < limitBars.Length; j++)
        {
            limitBars[j].gameObject.SetActive(false);
            limitBars[i].Start();
        }

        // Fill Limit Bars
        float currentLimitValue = danteStats.currentLimitValue;
        for (int j = 0; j < danteStats.maxLimitBatteries; j++)
        {
            if (currentLimitValue > 1.0f)
            {
                limitBars[j].SetLimitBarValue(1.0f);
                currentLimitValue -= 1.0f;
            }
            else
            {
                limitBars[j].SetLimitBarValue(currentLimitValue);
                currentLimitValue = 0.0f;
            }
        }

        if (!danteState.demon && danteStats.currentLimitValue >= 3)
        {
            foreach (var fire in fireIndicators)
            {
                fire.SetActive(true);
            }
        }
        else
        {
            foreach (var fire in fireIndicators)
            {
                fire.SetActive(false);
            }
        }
    }
}
