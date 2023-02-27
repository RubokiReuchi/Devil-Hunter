using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public List<MapBox> mapBoxes = new();

    public GameObject dante;
    [NonEditable] public MapBox actualMapBox;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            mapBoxes.Add(transform.GetChild(i).GetComponent<MapBox>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (MapBox box in mapBoxes)
        {
            if (box.boxCollider.bounds.Contains(dante.transform.position))
            {
                actualMapBox = box;
                return;
            }
        }
    }

    public bool CheckMapBoxClean(int row, int col)
    {
        foreach (MapBox box in mapBoxes)
        {
            if (box.row == row && box.col == col)
            {
                return box.CalculateClear();
            }
        }

        return false; // box not in this scene
    }
}
