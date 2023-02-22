using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dragonfly : Enemy
{
    public float detection_range;
    public float shoot_range;

    [Header("Range Attack")]
    public GameObject bullet;
    public Transform bullet_origin;

    // Check grounded
    [Header("Check Grounded")]
    public Vector2 boxSize;
    public float maxDistance;
    public LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        spawn = transform.position;
        anim = GetComponent<Animator>();

        orientation = 0;

        sprite = GetComponent<SpriteRenderer>();
        sprite_alpha = sprite.color.a;
        death = false;
    }

    // Update is called once per frame
    void Update()
    {
        // death
        if (death)
        {
            if (sprite_alpha > 0) sprite_alpha -= 100 * Time.deltaTime;
            sprite.color = new Color(255, 255, 255, sprite_alpha);
        }
    }

    public IEnumerator Co_Attack()
    {
        yield return new WaitForSeconds(attack_cd);
        anim.SetBool("CanAttack", true);
    }

    public void AttackRange()
    {
        anim.SetBool("CanAttack", false);
        if (bullet)
        {
            Instantiate(bullet, bullet_origin.position, Quaternion.identity);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position - transform.up * maxDistance, boxSize);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, detection_range);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, shoot_range);
    }
}
