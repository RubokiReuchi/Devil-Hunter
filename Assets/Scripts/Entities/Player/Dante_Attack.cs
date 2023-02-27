using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Provider;

public enum INPUT_RECEIVED
{
    NONE,
    G_LIGHT,
    G_HEAVY,
    A_LIGHT,
    A_HEAVY,
    GRAB,
    ULT
}

public class Dante_Attack : MonoBehaviour
{
    public static Dante_Attack instance;
    [NonEditable] public bool canReceiveInput;
    [NonEditable] public INPUT_RECEIVED inputReceived = INPUT_RECEIVED.NONE;

    Dante_StateMachine state;
    Dante_Stats stats;
    Dante_Skills skills;
    Dante_Movement dm;
    Rigidbody2D rb;
    Animator anim;
    public Hit hit;

    [NonEditable] public float chargeForce = 0;
    public ParticleSystem chargePs;

    public UltLightnings ult;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        state = GetComponent<Dante_StateMachine>();
        stats = GetComponent<Dante_Stats>();
        skills = GetComponent<Dante_Skills>();

        dm = GetComponent<Dante_Movement>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        canReceiveInput = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (state.IsDashing() || !state.IsAlive() || state.IsInteracting() || !canReceiveInput) return;

        if (!state.IsAiming())
        {
            if (dm.nullGravity)
            {
                if (InputManager.instance.input.Attack1.WasPressedThisFrame())
                {
                    inputReceived = INPUT_RECEIVED.G_LIGHT;
                }
                else if (InputManager.instance.input.Attack2.WasPressedThisFrame() && skills.chargeUnlocked)
                {
                    inputReceived = INPUT_RECEIVED.G_HEAVY;
                    canReceiveInput = false;
                }
            }
            else
            {
                if (InputManager.instance.input.Attack1.WasPressedThisFrame() && anim.GetBool("Can LightAir"))
                {
                    inputReceived = INPUT_RECEIVED.A_LIGHT;
                    canReceiveInput = false;
                }
                else if (InputManager.instance.input.Attack2.WasPressedThisFrame() && skills.fallingAttackUnlocked)
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
                if (InputManager.instance.input.Attack1.WasPressedThisFrame() && skills.grabUnlocked)
                {
                    inputReceived = INPUT_RECEIVED.GRAB;
                    canReceiveInput = false;
                }
                else if (InputManager.instance.input.Attack2.WasPressedThisFrame() && skills.ultUnlocked && stats.styleLevel == 5)
                {
                    inputReceived = INPUT_RECEIVED.ULT;
                    canReceiveInput = false;
                    ult.SetUltObjective(state.aimObjective);
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
        float delay = 0.3f;
        if (state.demon) delay /= 1.5f;
        yield return new WaitForSeconds(delay);
        Dante_Attack.instance.canReceiveInput = true;
    }

    public void SetJump(bool value)
    {
        dm.isJumping = value;
    }

    public void CastUlt()
    {
        ult.StartCoroutine("PlayUlt");
    }
}
