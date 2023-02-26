using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FollowY : MonoBehaviour
{
    float lenght;
    float startPos;
    public GameObject cam;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(cam.transform.position.x, transform.position.y);
        lenght = 45;
        startPos = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float temp = cam.transform.position.y;

        transform.position = new Vector2(transform.position.x, startPos);

        if (temp > startPos + lenght) startPos += lenght;
        else if (temp < startPos - lenght) startPos -= lenght;
    }
}
