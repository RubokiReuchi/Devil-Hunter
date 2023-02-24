using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.XR;

public enum INPUT_RECEIVED
{
    NONE,
    G_LIGHT,
    G_HEAVY,
    A_LIGHT,
    A_HEAVY
}

public class Dante_Attack : MonoBehaviour
{
    public static Dante_Attack instance;
    [NonEditable] public bool canReceiveInput;
    [NonEditable] public INPUT_RECEIVED inputReceived = INPUT_RECEIVED.NONE;

    Dante_StateMachine state;
    Dante_Skills skills;
    Dante_Movement dm;
    Rigidbody2D rb;
    Animator anim;
    public Hit hit;

    public GameObject danteWavePrefab;
    public GameObject demonWavePrefab;
    public Transform waveSpawn;

    [NonEditable][SerializeField] bool canShoot;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        state = GetComponent<Dante_StateMachine>();

        skills = GetComponent<Dante_Skills>();

        dm = GetComponent<Dante_Movement>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        canReceiveInput = true;

        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (state.IsDashing() || !state.IsAlive() || state.IsInteracting() || !canReceiveInput) return;

        if (!state.IsAiming())
        {
            if (dm.nullGravity)
            {
                if (dm.input.Attack1.WasPressedThisFrame())
                {
                    inputReceived = INPUT_RECEIVED.G_LIGHT;
                    //canReceiveInput = false;
                }
                else if (dm.input.Attack2.WasPressedThisFrame())
                {
                    inputReceived = INPUT_RECEIVED.G_HEAVY;
                    canReceiveInput = false;
                }
            }
            else
            {
                if (dm.input.Attack1.WasPressedThisFrame() && anim.GetBool("Can LightAir"))
                {
                    inputReceived = INPUT_RECEIVED.A_LIGHT;
                    canReceiveInput = false;
                }
                else if (dm.input.Attack2.WasPressedThisFrame() && skills.fallingAttackUnlocked)
                {
                    inputReceived = INPUT_RECEIVED.A_HEAVY;
                    canReceiveInput = false;
                }
            }
        }
        else
        {
            if (dm.nullGravity)
            {
                if (dm.input.Attack1.WasPressedThisFrame() && skills.grabUnlocked)
                {
                    anim.SetTrigger("AttackHeavy1");
                    state.SetState(DANTE_STATE.ATTACKING_AIR);
                }
                else if (dm.input.Attack2.WasPressedThisFrame() && !state.IsAttacking() && canShoot && skills.waveUnlocked
                    && (state.CompareState(DANTE_STATE.IDLE) || state.CompareState(DANTE_STATE.WALK)))
                {
                    state.SetState(DANTE_STATE.SHOTING);
                    anim.SetTrigger("Shoot");
                }
            }
        }
    }

    public void FifthComboJump()
    {
        if (dm.isJumping) return;
        rb.AddForce(Vector2.up * 200);
        dm.isJumping = true;
    }

    public void FirstComboJump()
    {
        if (dm.isJumping) return;
        rb.AddForce(new Vector2(transform.localScale.x * 150, 300));
        dm.isJumping = true;
    }

    public void WaitUntilNextAttack()
    {
        StartCoroutine("Co_WaitUntilNextAttack");
    }

    IEnumerator Co_WaitUntilNextAttack()
    {
        yield return new WaitForSeconds(0.3f);
        Dante_Attack.instance.canReceiveInput = true;
    }

    public void SetJump(bool value)
    {
        dm.isJumping = value;
    }

    IEnumerator Shoot()
    {
        GameObject wavePrefab;
        if (state.demon) wavePrefab = demonWavePrefab;
        else wavePrefab = danteWavePrefab;
        GameObject.Instantiate(wavePrefab, waveSpawn.position, transform.rotation);
        canShoot = false;
        yield return new WaitForSeconds(0.5f);
        if (state.CompareState(DANTE_STATE.SHOTING)) state.SetState(DANTE_STATE.IDLE);
        canShoot = true;
    }
}
