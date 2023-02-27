using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapBox : MonoBehaviour
{
    public int row;
    public int col;

    bool cleared;

    [HideInInspector] public BoxCollider2D boxCollider;

    public GameObject[] importantItemsOnBox;

    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        boxCollider = GetComponent<BoxCollider2D>();
        cleared = false;
    }

    private void FixedUpdate()
    {
        if (cleared) return;

        foreach (GameObject item in importantItemsOnBox)
        {
            if (item.activeSelf) return;
        }

        cleared = true;
    }
}
