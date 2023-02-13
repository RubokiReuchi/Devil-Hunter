using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextOrderInLayer : MonoBehaviour
{
    public int sort;
    MeshRenderer render;
    // Start is called before the first frame update
    void Start()
    {
        render= GetComponent<MeshRenderer>();
        render.sortingOrder = sort;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
