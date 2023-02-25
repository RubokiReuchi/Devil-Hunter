using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixOrintationInsideEntity : MonoBehaviour
{
    public Transform parent;
    float originalScaleX;

    void Start()
    {
        originalScaleX = transform.localScale.x;
    }

    void Update()
    {
        if (parent.transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-originalScaleX, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(originalScaleX, transform.localScale.y, transform.localScale.z);
        }
    }
}
