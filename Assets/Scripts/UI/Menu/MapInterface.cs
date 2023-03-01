using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInterface : MonoBehaviour
{
    List<MapTile> mapTiles = new();
    public MapManager mapManager;
    MapBox actualMapTile;

    public GameObject playerMark;

    void OnEnable()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            mapTiles.Add(transform.GetChild(i).GetComponent<MapTile>());
            mapTiles[i].Init();

            mapTiles[i].UpdateVisibility(MapMenu.instance.CheckMapBoxUnveil(mapTiles[i].row, mapTiles[i].col));
            mapTiles[i].UpdateColor(MapMenu.instance.CheckMapBoxClean(mapTiles[i].row, mapTiles[i].col));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (actualMapTile != null)
        {
            GetActualTile().UpdateColor(actualMapTile.CalculateClear());
        }

        if (actualMapTile != mapManager.actualMapBox)
        {
            actualMapTile = mapManager.actualMapBox;
            playerMark.transform.position = GetActualTile().transform.position;
        }
    }

    MapTile GetActualTile()
    {
        foreach (MapTile tile in mapTiles)
        {
            if (tile.row == actualMapTile.row && tile.col == actualMapTile.col)
            {
                return tile;
            }
        }

        return null;
    }

    public void RefreshPlayerMark()
    {
        playerMark.transform.position = GetActualTile().transform.position;
    }
}
