using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    float lenght;
    float startPos;
    public GameObject cam;
    public float parallaxForce;

    // Start is called before the first frame update
    void Start()
    {
        lenght = 45;
        startPos = transform.position.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = cam.transform.position.x * (1 - parallaxForce);
        float distance = cam.transform.position.x * parallaxForce;

        transform.position = new Vector2(startPos + distance, transform.position.y);

        if (temp > startPos + lenght) startPos += lenght;
        else if (temp < startPos - lenght) startPos -= lenght;
    }
}
