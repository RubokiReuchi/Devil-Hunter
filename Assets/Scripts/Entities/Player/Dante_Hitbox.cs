using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
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

        dm.camActions.ShakeCamera(0.25f, damage / 50.0f);
        dm.PlayHitParticles((int)damage);

        stats.current_hp -= damage;
        DisplayDamage(damage, hit_point + Vector3.up * 0.2f);
        if (stats.current_hp <= 0)
        {
            stats.current_hp = 0;
            anim.SetBool("Death", true);
        }
        else
        {
            if (!Dante_StateMachine.instance.CompareState(DANTE_STATE.DASHING)) anim.SetTrigger("Hitted");
            StartCoroutine("Co_TakeDamage");
        }
    }

    IEnumerator Co_TakeDamage()
    {
        dm.saveTime = true;
        yield return new WaitForSeconds(0.5f);
        dm.saveTime = false;
    }
}
