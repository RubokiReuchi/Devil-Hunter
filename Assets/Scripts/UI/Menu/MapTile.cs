using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapTile : MonoBehaviour
{
    public int row;
    public int col;

    [HideInInspector] public Image image;

    public void Init()
    {
        image = GetComponent<Image>();
        gameObject.SetActive(false);
    }

    public void UpdateColor(bool clean)
    {
        if (clean) image.color = new Color(0.6f, 0.6f, 0.6f);
        else image.color = new Color(0.17f, 0.6f, 0.56f);
    }

    public void UpdateVisibility(bool unveil)
    {
        if (unveil) gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }
}
