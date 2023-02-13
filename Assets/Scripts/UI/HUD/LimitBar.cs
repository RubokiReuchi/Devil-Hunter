using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LimitBar : MonoBehaviour
{
    [HideInInspector] public Slider slider;
    LimitBattery battery;
    public Image image;

    public void Start()
    {
        slider = GetComponent<Slider>();
        battery = GetComponentInParent<LimitBattery>();
    }

    void Update()
    {
        
    }

    public void SetLimitBarValue(float value)
    {
        slider.value = value;
        if (slider.value == 1)
        {
            image.color = battery.fullLimitColor;
        }
        else
        {
            image.color = battery.defaultLimitColor;
        }
    }
}
