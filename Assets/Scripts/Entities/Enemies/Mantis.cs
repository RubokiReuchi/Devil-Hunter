using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mantis : Enemy
{

    public float detection_range;
    public float detection_melee;

    // melee attack
    [Header("Melee Attack")]
    [NonEditable] public float attackAtDistance = 0.7f;

    [Header("Range Attack")]
    public GameObject bullet;
    public Transform bullet_origin;

    // Check grounded
    [Header("Check Grounded")]
    public Vector2 box_size;
    public float max_distance;
    public LayerMask layer_mask;
    bool stunned;

    // Start is called before the first frame update
    void Start()
    {
        spawn = transform.position;
        anim = GetComponent<Animator>();

        orientation = 0;

        sprite = GetComponent<SpriteRenderer>();
        sprite_alpha = sprite.color.a;
        death = false;

        restrict_left = false;
        restrict_right = false;
    }

    // Update is called once per frame
    void Update()
    {
        // enmy orientation
        if (GameObject.FindGameObjectWithTag("Dante") != null)
        {
            if (GameObject.FindGameObjectWithTag("Dante").transform.position.x > transform.position.x)
            {
                orientation = 1;
            }
            else if (GameObject.FindGameObjectWithTag("Dante").transform.position.x < transform.position.x)
            {
                orientation = -1;
            }
            else
            {
                orientation = 0;
            }
        }

        // on air stunned
        anim.SetBool("Stunned", !Physics2D.BoxCast(transform.position, box_size, 0, -transform.up, max_distance, layer_mask));
        
        // control edge falling (ground enemies only)
        restrict_right = !Physics2D.BoxCast(transform.position + new Vector3(4 * box_size.x / 10, 0, 0), new Vector2(box_size.x / 5, box_size.y), 0, -transform.up, max_distance, layer_mask);
        restrict_left = !Physics2D.BoxCast(transform.position - new Vector3(4 * box_size.x / 10, 0, 0), new Vector2(box_size.x / 5, box_size.y), 0, -transform.up, max_distance, layer_mask);

        // control walking to walls
        if (transform.localScale.x == 1)
        {
            if (Physics2D.BoxCast(transform.position + transform.up * max_distance_up, box_size_lateral, 0, transform.right, max_distance_right, layer_mask_lateral)) restrict_right = true;
            if (Physics2D.BoxCast(transform.position + transform.up * max_distance_up, box_size_lateral, 0, -transform.right, max_distance_right, layer_mask_lateral)) restrict_left = true;
        }
        else
        {
            if (Physics2D.BoxCast(transform.position + transform.up * max_distance_up, box_size_lateral, 0, -transform.right, max_distance_right, layer_mask_lateral)) restrict_left = true;
            if (Physics2D.BoxCast(transform.position + transform.up * max_distance_up, box_size_lateral, 0, transform.right, max_distance_right, layer_mask_lateral)) restrict_right = true;
        }

        // death
        if (death)
        {
            if (sprite_alpha > 0) sprite_alpha -= 100 * Time.deltaTime;
            sprite.color = new Color(255, 255, 255, sprite_alpha);
        }
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
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(transform.position - transform.up * max_distance, box_size);

        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position - new Vector3(4 * box_size.x / 10, 0, 0) - transform.up * max_distance, new Vector2(box_size.x / 5, box_size.y));
        Gizmos.DrawCube(transform.position + new Vector3(4 * box_size.x / 10, 0, 0) - transform.up * max_distance, new Vector2(box_size.x / 5, box_size.y));

        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(transform.position - transform.right * max_distance_left * transform.localScale.x + transform.up * max_distance_up, box_size_lateral);
        Gizmos.DrawCube(transform.position + transform.right * max_distance_right * transform.localScale.x + transform.up * max_distance_up, box_size_lateral);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, detection_range);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detection_melee);
    }
}
