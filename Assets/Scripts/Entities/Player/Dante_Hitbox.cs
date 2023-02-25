using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dante_Hitbox : Hitbox
{
    Dante_Movement dm;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        stats = GetComponent<Stats>();
        rb = GetComponent<Rigidbody2D>();

        dm = GetComponent<Dante_Movement>();
    }

    public override void TakeDamage(float damage, Vector3 hit_point, bool ultraKnockBack = false)
    {
        if (GetComponent<Dante_Movement>().saveTime) return; // ignore it if dante is iframed
        if (anim.GetBool("Death")) return; // ignore it if entity is death

        if (!anim.GetBool("Death") && stats.knockback_resist >= 0 && ((damage * 3 - stats.knockback_resist > 0) || ultraKnockBack))
        {
            if (rb.velocity.y < 0) rb.velocity = new Vector2(rb.velocity.x, 0);
            if (!ultraKnockBack) rb.AddForce(Vector2.up * (damage * 3 - stats.knockback_resist));
            else rb.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
        }

        dm.camActions.ShakeCamera(0.25f, damage / 50.0f);
        dm.PlayHitParticles((int)damage);

        stats.current_hp -= damage;
        DisplayDamage(damage, hit_point + Vector3.up * 0.2f);
        if (stats.current_hp <= 0)
        {
            anim.SetBool("Death", true);
        }
    }
}
