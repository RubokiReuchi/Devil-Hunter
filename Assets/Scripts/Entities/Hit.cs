using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Hit : MonoBehaviour
{
    public float damage;
    public bool projectile;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((this.CompareTag("DanteSword") || this.CompareTag("DanteWave")) && collision.gameObject.CompareTag("Enemy"))
        {
            Hitbox hb = collision.GetComponent<Hitbox>();
            hb.TakeDamage(damage, collision.transform.position);
            if (projectile) return;
            Dante_Stats stats = this.GetComponentInParent<Dante_Stats>();
            Dante_StateMachine state = this.GetComponentInParent<Dante_StateMachine>();
            if (!state.demon)
            {
                stats.LifeSteal(damage);
                stats.AddLimit(0.2f);
                stats.GetStyle(10.0f);
            }
            else
            {
                stats.LifeSteal(damage * 5.0f);
                stats.GetStyle(10.0f);
            }
        }
        else if ((this.CompareTag("EnemySword") || this.CompareTag("EnemyWave")) && collision.gameObject.CompareTag("Dante"))
        {
            Hitbox hb = collision.GetComponent<Hitbox>();
            hb.TakeDamage(damage, collision.transform.position);
            if (projectile) return;
            Stats stats = this.GetComponentInParent<Stats>();
            stats.LifeSteal(damage);
        }
    }
}
