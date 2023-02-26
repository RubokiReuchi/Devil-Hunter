using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    ULT,
    DASHING,
    DEATH,
    INTERACT
}

public class Dante_StateMachine : MonoBehaviour
{
    public static Dante_StateMachine instance;

    [NonEditable][SerializeField] DANTE_STATE state;
    Dante_Stats stats;
    Dante_Movement dm;

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

    [Header("Revive")]
    public float emitForceRadius;
    public float emitForcePower;
    LayerMask layerMask;
    public ParticleSystem reviveLightning;
    bool canRevive = false;

    [NonEditable][SerializeField] bool aim;
    [NonEditable] public bool dash;
    public SpriteRenderer target;
    [NonEditable] public GameObject aimObjective;
    public Camera cam;

    [NonEditable] public float orientation;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        state = DANTE_STATE.IDLE;
        stats = GetComponent<Dante_Stats>();
        dm = GetComponent<Dante_Movement>();

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
        if (dm.input.Limit.WasPressedThisFrame())
        {
            if (IsAlive()) // shift demon
            {
                if (!demon && stats.currentLimitValue >= 3) ActiveDemonForm();
                else if (demon) { UnactiveDemonForm(); stats.UseLimit(1.0f); }
            }
            else if (canRevive && GetComponent<Dante_Skills>().reviveUnlocked && GetComponent<Dante_Skills>().canRevive) // revive
            {
                Revive();
            }
        }
        if (demon && !CompareState(DANTE_STATE.ULT))
        {
            stats.UseLimit(Time.deltaTime * 0.5f); // 2 second per each bar
            if (stats.currentLimitValue <= 0) UnactiveDemonForm();
        }

        if (dm.input.Aim.ReadValue<float>() == 1) aim = true;
        else aim = false;
        if (IsInteracting() || !dm.isOnGround) aim = false;

        if (aim && !GetClosestEnemyPos()) aim = false;

        if (aim)
        {
            target.enabled = true;

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
            aimObjective = null;
            orientation = 0;
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
        if (state == DANTE_STATE.ATTACKING_GROUND || state == DANTE_STATE.ATTACKING_AIR || state == DANTE_STATE.ATTACKING_FALLING || state == DANTE_STATE.ULT) return true;
        else return false;
    }

    public bool IsAttackingStatic()
    {
        if (state == DANTE_STATE.ATTACKING_GROUND || state == DANTE_STATE.ATTACKING_FALLING || state == DANTE_STATE.ULT) return true;
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

    public void AllowRevive()
    {
        canRevive = true;
    }

    void Revive()
    {
        GetComponent<Dante_Skills>().canRevive = false;
        anim.SetTrigger("Revive");
        anim.SetBool("Death", false);
        SetState(DANTE_STATE.IDLE);
        stats.Heal(stats.max_hp);
        stats.currentLimitValue = stats.maxLimitBatteries;
        EmitForce();
        StartCoroutine("Co_revive");
        canRevive = false;
    }

    IEnumerator Co_revive()
    {
        dm.saveTime = true;
        yield return new WaitForSeconds(0.5f);
        dm.saveTime = false;
    }

    void EmitForce()
    {
        layerMask |= (1 << 7); // add enemies
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, emitForceRadius, layerMask);

        foreach (Collider2D collider in colliders)
        {
            Vector2 direction = (collider.transform.position - transform.position).normalized;
            collider.GetComponent<Rigidbody2D>().AddForce(direction * emitForcePower, ForceMode2D.Impulse);
        }
    }

    bool GetClosestEnemyPos()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0) return false;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        List<GameObject> enemiesInCam = new List<GameObject>();

        foreach (GameObject enemy in enemies)
        {
            if (IsOnCameraBoundaries(enemy)) enemiesInCam.Add(enemy);
        }

        float distance = Mathf.Infinity;
        Vector3 pos = Vector3.zero;

        foreach (GameObject enemy in enemiesInCam)
        {
            float each_distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (each_distance < distance)
            {
                distance = each_distance;
                pos = enemy.transform.position;
                aimObjective = enemy;
            }
        }

        if (distance != Mathf.Infinity)
        {
            target.transform.position = pos + Vector3.up * 0.25f;
            return true;
        }
        else
        {
            return false;
        }
    }

    bool IsOnCameraBoundaries(GameObject enemy)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
        Vector3 point = enemy.transform.position;

        foreach (Plane plane in planes)
        {
            if (plane.GetDistanceToPoint(point) < 0)
            {
                return false;
            }
        }
        return true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, emitForceRadius);
    }
}
