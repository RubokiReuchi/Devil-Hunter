using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedStone : MonoBehaviour
{
    Animator anim;
    public GameObject dropRedEggs;
    public int eggsAmount;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DanteSword"))
        {
            anim.SetTrigger("Destroy");
            StartCoroutine("Co_Destroy");
        }
    }

    IEnumerator Co_Destroy()
    {
        GameObject drop = Instantiate(dropRedEggs, transform.position, Quaternion.identity);
        drop.GetComponent<DropRedEggs>().particleAmount = eggsAmount;
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }
}
