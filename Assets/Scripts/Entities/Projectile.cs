using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Vector3 direction;
    public float speed;
    public bool flip_or_aim; // flip --> true
    public string[] tags;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Dante") == null) Destroy(gameObject);
        if (flip_or_aim)
        {
            if (GameObject.FindGameObjectWithTag("Dante").transform.position.x > this.transform.position.x)
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
            direction = (GameObject.FindGameObjectWithTag("Dante").transform.position - transform.position).normalized;
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
            if (collision.gameObject.CompareTag("Dante") && GameObject.FindGameObjectWithTag("Dante").GetComponent<Dante_Movement>().iframe) continue;
            else if (collision.gameObject.CompareTag(tag)) n++;
        }

        if (n > 0) Destroy(gameObject);
    }
}
