using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    protected Animator anim;
    protected Stats stats;
    protected Rigidbody2D rb;
    public GameObject floating_text;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        stats = GetComponent<Stats>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void TakeDamage(float damage, Vector3 hit_point, bool ultraKnockBack = false)
    {
        if (anim.GetBool("Death") || anim.GetBool("Destroy")) return; // ignore it if entity is death
        if (GetComponentInChildren<Shield>() != null && GetComponentInChildren<Shield>().active) return;

        if (!anim.GetBool("Hitted") && !anim.GetBool("Death") && stats.knockback_resist >= 0 && (damage * 3 - stats.knockback_resist > 0))
        {
            anim.SetBool("Hitted", true);
            rb.AddForce(Vector2.up * (damage * 3 - stats.knockback_resist));
        }

        if (stats.max_hp == 0) // display before set stats
        {
            DisplayDamage(damage, hit_point + Vector3.up * 0.2f);
            return;
        }

        stats.current_hp -= damage;
        DisplayDamage(damage, hit_point + Vector3.up * 0.2f);
        if (stats.current_hp <= 0)
        {
            anim.SetBool("Death", true);
        }
    }

    protected void DisplayDamage(float damage, Vector3 hit_point)
    {
        if (floating_text)
        {
            Vector2 circle = Random.insideUnitCircle * 0.75f;
            GameObject text = Instantiate(floating_text, hit_point + new Vector3(circle.x, circle.y, 0), Quaternion.identity);

            float rounded = Mathf.Round(damage * 100.0f) / 100.0f;
            TextMesh aux = text.GetComponentInChildren<TextMesh>();
            aux.text = rounded.ToString();

            if (damage < 25.0f) aux.color = Color.white;
            else if (damage < 50.0f) aux.color = Color.yellow;
            else if (damage < 75.0f) aux.color = new Color(241, 73, 0, 255);
            else if (damage < 100.0f) aux.color = Color.red;
            else aux.color = new Color(80, 0, 241, 255);
        }
    }
}
