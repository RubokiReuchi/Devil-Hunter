using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapBox : MonoBehaviour
{
    public int row;
    public int col;

    bool cleared;
    bool unveil;

    [HideInInspector] public BoxCollider2D boxCollider;

    public GameObject[] importantItemsOnBox;

    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        boxCollider = GetComponent<BoxCollider2D>();
        cleared = false;
        unveil = false;
    }

    public bool CalculateClear()
    {
        if (cleared) return true;

        foreach (GameObject item in importantItemsOnBox)
        {
            if (item.activeSelf)
            {
                MapMenu.instance.SetMapBoxCleared(row, col, false);
                return false;
            }
        }

        cleared = true;
        MapMenu.instance.SetMapBoxCleared(row, col, true);
        return true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!unveil && collision.CompareTag("Dante"))
        {
            unveil = true;
            MapMenu.instance.SetMapBoxUnveil(row, col, true);
        }
    }
}
