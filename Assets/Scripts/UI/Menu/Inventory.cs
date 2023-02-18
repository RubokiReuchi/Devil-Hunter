using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class Inventory : MonoBehaviour
{
    public GameObject itemPanel;
    List<GameObject> skillsList = new();

    public TextMeshProUGUI skillNameText;
    public TextMeshProUGUI skillDescritionText;

    // Start is called before the first frame update
    void Start()
    {
        CleanSkillInfo();
        for (int i = 0; i < itemPanel.transform.childCount; i++)
        {
            skillsList.Add(itemPanel.transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSkillInfo(int skill)
    {
        SkillInfoContainer aux = skillsList[skill].GetComponent<SkillInfoContainer>();
        skillNameText.text = aux.info.skillName;
        skillDescritionText.text = aux.info.skillDescription;
    }

    public void CleanSkillInfo()
    {
        skillNameText.text = "";
        skillDescritionText.text = "";
    }
}
