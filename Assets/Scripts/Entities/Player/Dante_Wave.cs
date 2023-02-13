using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dante_Wave : MonoBehaviour
{
    Vector3 direction;
    public float speed;
    public bool flip_or_aim; // flip --> true
    public string[] tags;
    GameObject dante;

    // Start is called before the first frame update
    void Start()
    {
        dante = GameObject.FindGameObjectWithTag("Dante");
        if (flip_or_aim)
        {
            if (dante.transform.localScale.x == 1)
            {
                direction = Vector3.right;
                this.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                direction = Vector3.left;
                this.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int n = 0;
        foreach (string tag in tags)
        {
            if (collision.gameObject.CompareTag(tag)) n++;
        }

        if (n > 0) Destroy(gameObject);
    }
}
