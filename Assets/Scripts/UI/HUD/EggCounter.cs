using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EggCounter : MonoBehaviour
{
    public TextMeshProUGUI counter;
    public IntValue eggsCount;
    
    void Update()
    {
        counter.text = eggsCount.value.ToString();
    }
}
