using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Slider slider;
    RectTransform rectangle;
    public GameObject variationGO;
    Image variationImage;
    RectTransform variationRectangle;
    [NonEditable][SerializeField] float variationValue;
    bool delay;
    public Color32 variationBarColor;

    public Stats danteStats;
    float danteFixedCurrentHp;

    void Start()
    {
        slider = GetComponent<Slider>();
        rectangle = GetComponent<RectTransform>();

        slider.maxValue = danteStats.max_hp;
        slider.value = danteStats.max_hp;

        variationImage = variationGO.GetComponent<Image>();
        variationRectangle = variationGO.GetComponent<RectTransform>();
        variationValue = 0;
        delay = false;

        danteFixedCurrentHp = 0;
    }

    void Update()
    {
        danteFixedCurrentHp = danteStats.current_hp;
        if (danteFixedCurrentHp < 0) danteFixedCurrentHp = 0;

        if (slider.value > danteFixedCurrentHp)
        {
            variationValue += slider.value - danteFixedCurrentHp;
            StartCoroutine("Variation");
        }
        if (slider.value < danteFixedCurrentHp)
        {
            float value;
            if (danteFixedCurrentHp == danteStats.max_hp) value = Time.deltaTime * 500.0f;
            else value = Time.deltaTime * 50.0f;

            slider.value += value;
        }
        else
        {
            slider.value = danteFixedCurrentHp;
        }

        slider.maxValue = danteStats.max_hp;
        rectangle.sizeDelta = new Vector2(slider.maxValue, rectangle.sizeDelta.y);

        if (delay)
        {
            variationValue -= Time.deltaTime * 50.0f;
            variationRectangle.sizeDelta = new Vector2(variationValue, variationRectangle.sizeDelta.y);

            if (variationValue <= 0)
            {
                variationValue = 0;
            }
        }
        else
        {
            delay = false;
        }
    }

    IEnumerator Variation()
    {
        slider.value = danteFixedCurrentHp;
        variationImage.color = variationBarColor;
        variationRectangle.anchoredPosition = new Vector2(slider.value, variationRectangle.pivot.y);
        variationRectangle.sizeDelta = new Vector2(variationValue, variationRectangle.sizeDelta.y);
        yield return new WaitForSeconds(0.5f);
        delay = true;
    }
}
