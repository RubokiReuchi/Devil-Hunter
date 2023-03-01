using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MapMenu : MonoBehaviour, DataPersistenceInterfice
{
    public static MapMenu instance;

    public GameObject map;
    MapInterface mapInterface;

    Vector3 touchOffset;
    bool touching;

    public SerializableDictionary<int, bool> loadedMapTilesUnveil = new();
    public SerializableDictionary<int, bool> loadedMapTilesCleared = new();

    public void LoadData(GameData data)
    {
        loadedMapTilesUnveil = data.mapTilesUnveil;
        loadedMapTilesCleared = data.mapTilesCleared;
    }

    public void SaveData(GameData data)
    {
        data.mapTilesUnveil = loadedMapTilesUnveil;
        data.mapTilesCleared = loadedMapTilesCleared;
    }

    public void Init()
    {
        instance = this;
        mapInterface = map.GetComponent<MapInterface>();
    }

    private void OnEnable()
    {
        touching = false;
    }

    // Update is called once per frame
    void Update()
    {
        // move
        if (!touching && InputManager.instance.input.Attack2.WasPressedThisFrame())
        {
            touching = true;
            touchOffset = map.transform.position - Input.mousePosition;
        }

        if (touching)
        {
            map.transform.position = Input.mousePosition + touchOffset;
            mapInterface.RefreshPlayerMark();
        }

        if (touching && InputManager.instance.input.Attack2.WasReleasedThisFrame())
        {
            touching = false;
        }

        // zoom
        if (InputManager.instance.input.Zoom.ReadValue<float>() > 0)
        {
            ZoomMap(0.1f);
            mapInterface.RefreshPlayerMark();
        }
        else if (InputManager.instance.input.Zoom.ReadValue<float>() < 0)
        {
            ZoomMap(-0.1f);
            mapInterface.RefreshPlayerMark();
        }
    }

    public void CenterOnPlayer()
    {
        map.transform.localPosition += transform.position - mapInterface.playerMark.transform.position;
        mapInterface.RefreshPlayerMark();
    }

    void ZoomMap(float value)
    {
        RectTransform rect = map.GetComponent<RectTransform>();
        Vector2 pivotStart = rect.pivot;

        Vector2 localpoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, GetComponentInParent<Canvas>().worldCamera, out localpoint);

        Vector2 normalizedPoint = Rect.PointToNormalized(rect.rect, localpoint);

        rect.pivot = normalizedPoint;
        map.transform.localScale += Vector3.one * value;
        map.transform.position = Input.mousePosition;
    }

    public bool CheckMapBoxClean(int row, int col)
    {
        bool ret = false;

        for (int i = 0; i < mapInterface.transform.childCount; i++)
        {
            MapTile aux = mapInterface.transform.GetChild(i).GetComponent<MapTile>();
            if (aux.row == row && aux.col == col)
            {
                if (loadedMapTilesCleared.ContainsKey(i))
                {
                    loadedMapTilesCleared.TryGetValue(i, out ret);
                }
                else
                {
                    loadedMapTilesCleared.Add(i, false);
                }
                return ret;
            }
        }

        return ret;
    }

    public void SetMapBoxCleared(int row, int col, bool cleared)
    {
        for (int i = 0; i < mapInterface.transform.childCount; i++)
        {
            MapTile aux = mapInterface.transform.GetChild(i).GetComponent<MapTile>();
            if (aux.row == row && aux.col == col)
            {
                if (loadedMapTilesCleared.ContainsKey(i)) loadedMapTilesCleared.Remove(i);
                loadedMapTilesCleared.Add(i, cleared);
                return;
            }
        }
    }

    public bool CheckMapBoxUnveil(int row, int col)
    {
        bool ret = false;

        for (int i = 0; i < mapInterface.transform.childCount; i++)
        {
            MapTile aux = mapInterface.transform.GetChild(i).GetComponent<MapTile>();
            if (aux.row == row && aux.col == col)
            {
                if (loadedMapTilesUnveil.ContainsKey(i))
                {
                    loadedMapTilesUnveil.TryGetValue(i, out ret);
                }
                else
                {
                    loadedMapTilesUnveil.Add(i, false);
                }
                return ret;
            }
        }

        return ret;
    }

    public void SetMapBoxUnveil(int row, int col, bool unveil)
    {
        for (int i = 0; i < mapInterface.transform.childCount; i++)
        {
            MapTile aux = mapInterface.transform.GetChild(i).GetComponent<MapTile>();
            if (aux.row == row && aux.col == col)
            {
                if (loadedMapTilesUnveil.ContainsKey(i)) loadedMapTilesUnveil.Remove(i);
                loadedMapTilesUnveil.Add(i, unveil);
                return;
            }
        }
    }
}
