using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactToHit : MonoBehaviour
{
    public GameObject parent;
    GameObject dante;
    public float reactionCooldown;
    bool canReact;

    void Start()
    {
        dante = GameObject.FindGameObjectWithTag("Dante");
        canReact = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canReact && collision.gameObject.CompareTag("Dante"))
        {
            parent.GetComponent<Rigidbody2D>().AddForce((transform.position - dante.transform.position).normalized * 5, ForceMode2D.Impulse);
            StartCoroutine("Co_React");
        }
    }

    IEnumerator Co_React()
    {
        canReact = false;
        yield return new WaitForSeconds(reactionCooldown);
        canReact = true;
    }
}
