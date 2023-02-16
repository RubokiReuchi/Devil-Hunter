using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SecretZone : MonoBehaviour
{
    Tilemap tilemap;
    [NonEditable][SerializeField] bool visible;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        visible = false;
    }

    void Update()
    {
        if (visible && tilemap.color.a > 0)
        {
            float newA = tilemap.color.a - Time.deltaTime;
            if (newA < 0) newA = 0;
            tilemap.color = new Color(1, 1, 1, newA);
        }
        if (!visible && tilemap.color.a < 1)
        {
            float newA = tilemap.color.a + Time.deltaTime;
            if (newA > 1) newA = 1;
            tilemap.color = new Color(1, 1, 1, newA);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dante"))
        {
            visible = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Dante"))
        {
            visible = false;
        }
    }
}
