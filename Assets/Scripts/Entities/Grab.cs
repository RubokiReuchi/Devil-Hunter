using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.CompareTag("DanteSword") && collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.GetComponent<Stats>().knockback_resist >= 0) // negative knockback means inmune
            {
                Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
                rb.AddForce(new Vector3(0, 400, 0));
                this.GetComponentInParent<Rigidbody2D>().AddForce(new Vector3(0, 400, 0));
            }
        }
    }
}
