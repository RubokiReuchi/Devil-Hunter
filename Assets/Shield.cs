using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public Sprite[] sprites;
    SpriteRenderer sprite;

    [NonEditable] public bool active;
    public float maxShield;
    [NonEditable][SerializeField] float currentShield;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        active = true;
        currentShield = maxShield;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pierce") && GameObject.FindGameObjectWithTag("Dante") != null)
        {
            if (GameObject.FindGameObjectWithTag("Dante").GetComponent<Dante_StateMachine>().demon)
            {
                TakeDamage(2);
            }
            else
            {
                TakeDamage(1);
            }
        }
    }

    void TakeDamage(float damage)
    {
        currentShield -= damage;

        if (currentShield > 3 * maxShield / 4) sprite.sprite = sprites[3];
        else if (currentShield > maxShield / 2) sprite.sprite = sprites[2];
        else if (currentShield > maxShield / 4) sprite.sprite = sprites[1];
        else sprite.sprite = sprites[0];

        if (currentShield <= 0) StartCoroutine("BreakShield");
    }

    IEnumerator BreakShield()
    {
        while (sprite.color.a > Time.deltaTime)
        {
            sprite.color = new Color(255, 255, 255, sprite.color.a - Time.deltaTime);
        }
        yield return null;
        active = false;
    }
}
