using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class Worm : Enemy
{
    bool turn;

    // Check grounded
    [Header("Check Grounded")]
    public Vector2 box_size;
    public float max_distance;
    public LayerMask layer_mask;

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
        if (anim.GetBool("Destroy")) return;

        // on air stunned
        if (!Physics2D.BoxCast(transform.position, box_size, 0, -transform.up, max_distance, layer_mask)) return;

        if (turn) { speed = -speed; turn = false; }

        // enemy orientation
        transform.position += new Vector3(speed * Time.deltaTime, 0);
        if (speed > 0) transform.localScale = new Vector2(1, 1);
        else transform.localScale = new Vector2(-1, 1);

        // control edge falling (ground enemies only)
        turn = !Physics2D.BoxCast(transform.position + new Vector3(4 * box_size.x / 10, 0, 0), new Vector2(box_size.x / 5, box_size.y), 0, -transform.up, max_distance, layer_mask);
        turn = !Physics2D.BoxCast(transform.position - new Vector3(4 * box_size.x / 10, 0, 0), new Vector2(box_size.x / 5, box_size.y), 0, -transform.up, max_distance, layer_mask);

        // control walking to walls
        if (transform.localScale.x == 1)
        {
            if (Physics2D.BoxCast(transform.position + transform.up * max_distance_up, box_size_lateral, 0, transform.right, max_distance_right, layer_mask_lateral)) turn = true;
        }
        else
        {
            if (Physics2D.BoxCast(transform.position + transform.up * max_distance_up, box_size_lateral, 0, -transform.right, max_distance_right, layer_mask_lateral)) turn = true;
        }

        // death
        if (death)
        {
            if (sprite_alpha > 0) sprite_alpha -= 100 * Time.deltaTime;
            sprite.color = new Color(255, 255, 255, sprite_alpha);
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
    }
}
