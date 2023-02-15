using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.Burst.Intrinsics.Arm;

public enum DANTE_STATE
{
    IDLE,
    RUN,
    WALK,
    JUMPING,
    WALL_SLIDING,
    FALLING,
    ATTACKING_GROUND,
    ATTACKING_AIR,
    ATTACKING_FALLING,
    SHOTING,
    DASHING,
    DEATH,
    INTERACT
}

public class Dante_StateMachine : MonoBehaviour
{
    [NonEditable][SerializeField] DANTE_STATE state;
    Dante_Stats stats;

    [Header("Demon Form")]
    [NonEditable] public bool demon;
    public float speedMultiplier;
    Animator anim;
    SpriteRenderer sprite;
    Sprite danteForm;
    public Sprite demonForm;
    public GameObject demonParticles;
    public TrailRenderer swordTrail;
    public Gradient danteTrailColor;
    public Gradient demonTrailColor;


    [NonEditable][SerializeField] bool aim;
    [NonEditable] public bool dash;
    public SpriteRenderer target;
    GameObject[] enemies;

    [NonEditable] public float orientation;

    // Start is called before the first frame update
    void Start()
    {
        state = DANTE_STATE.IDLE;
        stats = GetComponent<Dante_Stats>();

        demon = false;
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        danteForm = sprite.sprite;

        aim = false;
        dash = false;
        orientation = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Limit"))
        {
            if (!demon && stats.currentLimitValue >= 3) ActiveDemonForm();
            else if (demon) { UnactiveDemonForm(); stats.UseLimit(1.0f); }
        }
        if (demon)
        {
            stats.UseLimit(Time.deltaTime * 0.5f); // 2 second per each bar
            if (stats.currentLimitValue <= 0) UnactiveDemonForm();
        }

        aim = Input.GetButton("Aim");
        if (IsInteracting()) aim = false;

        if (aim)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length > 0) target.enabled = true;
            target.transform.position = GetClosestEnemyPos();

            if (target.transform.position.x > transform.position.x)
            {
                orientation = 1;
            }
            else if (target.transform.position.x < transform.position.x)
            {
                orientation = -1;
            }

            if (!IsDashing()) transform.localScale = new Vector3(orientation, 1, 1);
        }
        else
        {
            target.enabled = false;
        }
    }

    public void SetState(DANTE_STATE state)
    {
        this.state = state;
    }

    public bool CompareState(DANTE_STATE state)
    {
        return this.state == state;
    }

    public bool IsDashing()
    {
        if (state == DANTE_STATE.DASHING || dash) return true;
        else return false;
    }

    public bool IsAttacking()
    {
        if (state == DANTE_STATE.ATTACKING_GROUND || state == DANTE_STATE.SHOTING || state == DANTE_STATE.ATTACKING_AIR || state == DANTE_STATE.ATTACKING_FALLING) return true;
        else return false;
    }

    public bool IsAlive()
    {
        if (state == DANTE_STATE.DEATH) return false;
        else return true;
    }

    public bool IsInteracting()
    {
        if (state == DANTE_STATE.INTERACT) return true;
        else return false;
    }

    public bool IsAiming()
    {
        return aim;
    }

    // demon form
    void ActiveDemonForm()
    {
        demon = true;
        anim.SetFloat("DemonSpeed", speedMultiplier);
        sprite.sprite = demonForm;
        demonParticles.SetActive(true);
        demonParticles.GetComponent<Animator>().SetTrigger("Active");

        swordTrail.colorGradient = demonTrailColor;
    }

    void UnactiveDemonForm()
    {
        demon = false;
        anim.SetFloat("DemonSpeed", 1);
        sprite.sprite = danteForm;
        demonParticles.GetComponent<Animator>().SetTrigger("Unactive");

        swordTrail.colorGradient = danteTrailColor;
    }

    Vector3 GetClosestEnemyPos()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float distance = Mathf.Infinity;
        Vector3 pos = Vector3.zero;

        foreach (GameObject enemy in enemies)
        {
            float each_distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (each_distance < distance)
            {
                distance = each_distance;
                pos = enemy.transform.position;
            }
        }

        return pos;
    }
}
