using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Platform : MonoBehaviour
{
    BoxCollider2D colliderBox;
    float danteCapsuleSizeY;
    GameObject dante;
    Dante_Movement dm;
    bool waiting;

    // Start is called before the first frame update
    void Start()
    {
        colliderBox = GetComponent<BoxCollider2D>();
        dante = GameObject.FindGameObjectWithTag("Dante");
        danteCapsuleSizeY = GameObject.FindGameObjectWithTag("Dante").GetComponent<CapsuleCollider2D>().size.y / 2;
        dm = GameObject.FindGameObjectWithTag("Dante").GetComponent<Dante_Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dante.transform.position.y - danteCapsuleSizeY < transform.position.y + colliderBox.offset.y)
        {
            colliderBox.enabled = false;
        }
        else if (!waiting)
        {
            colliderBox.enabled = true;
            if (dm.input.GetDown.ReadValue<float>() == 1)
            {
                StartCoroutine("WaitToReactive");
                colliderBox.enabled = false;
            }
        }
    }

    IEnumerator WaitToReactive()
    {
        waiting = true;
        yield return new WaitForSeconds(0.25f);
        waiting= false;
    }
}
