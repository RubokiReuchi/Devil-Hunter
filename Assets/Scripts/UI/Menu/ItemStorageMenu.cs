using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemStorageMenu : MonoBehaviour
{
    Dante_Stats stats;

    [Header("Blue")]
    public TextMeshProUGUI blueFragmentText;
    public IntValue blueFragmentsCount;
    public TextMeshProUGUI blueEggText;

    [Header("Purple")]
    public TextMeshProUGUI purpleFragmentText;
    public IntValue purpleFragmentsCount;
    public TextMeshProUGUI purpleEggText;

    [Header("Gold")]
    public TextMeshProUGUI goldFragmentText;
    public IntValue goldFragmentsCount;
    public Image goldEggImage;
    public Sprite goldFragmentSprite;
    public Sprite goldEggSprite;

    [Header("Red")]
    public TextMeshProUGUI redEggsText;
    public IntValue redEggsCount;

    
    public void SetData()
    {
        if (GameObject.FindGameObjectWithTag("Dante") == null)
        {
            gameObject.SetActive(false);
            return;
        }

        stats = GameObject.FindGameObjectWithTag("Dante").GetComponent<Dante_Stats>();

        // Blue
        blueFragmentText.text = blueFragmentsCount.value.ToString() + " / 4";
        blueEggText.text = ((stats.max_hp - 300) / 50.0f).ToString();

        // Purple
        purpleFragmentText.text = purpleFragmentsCount.value.ToString() + " / 4";
        purpleEggText.text = (stats.maxLimitBatteries - 3).ToString();

        // Gold
        if (goldFragmentsCount.value == 4)
        {
            goldEggImage.sprite = goldEggSprite;
            goldEggImage.GetComponent<RectTransform>().sizeDelta = new Vector2(202, 250);
            goldFragmentText.text = "Obtained";
        }
        else
        {
            goldEggImage.sprite = goldFragmentSprite;
            goldEggImage.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 250);
            goldFragmentText.text = goldFragmentsCount.value.ToString() + " / 4";
        }

        // Red
        redEggsText.text = redEggsCount.value.ToString();
    }
}
