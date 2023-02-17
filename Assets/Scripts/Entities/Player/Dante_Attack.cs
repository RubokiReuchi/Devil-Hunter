using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Dante_Attack : MonoBehaviour
{
    Dante_StateMachine state;
    Dante_Skills skills;
    Dante_Movement dm;
    Animator anim;

    public GameObject danteWavePrefab;
    public GameObject demonWavePrefab;
    public Transform waveSpawn;

    [NonEditable][SerializeField] bool canShoot;
    [NonEditable][SerializeField] bool canThrust;

    // Start is called before the first frame update
    void Start()
    {
        state = GetComponent<Dante_StateMachine>();

        skills = GetComponent<Dante_Skills>();

        dm = GetComponent<Dante_Movement>();
        anim = GetComponent<Animator>();

        canShoot = true;
        canThrust = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (state.IsDashing() || !state.IsAlive() || state.IsInteracting()) return;

        if (!state.IsAiming())
        {
            if (dm.nullGravity)
            {
                if (dm.input.Attack1.WasPressedThisFrame())
                {
                    anim.SetTrigger("Attack1");
                    dm.runSpeed = 0.5f;
                    state.SetState(DANTE_STATE.ATTACKING_GROUND);
                }
                else if (dm.input.Attack2.WasPressedThisFrame() && skills.thrustUnlocked && canThrust)
                {
                    anim.SetBool("Thrust", true);
                    dm.runSpeed = 0.0f;
                    state.SetState(DANTE_STATE.ATTACKING_GROUND);
                    canThrust = false;
                }
            }
            else
            {
                if (dm.input.Attack1.WasPressedThisFrame() && anim.GetBool("Can LightAir"))
                {
                    anim.SetTrigger("AttackLightAir");
                    state.SetState(DANTE_STATE.ATTACKING_AIR);
                }
                else if (dm.input.Attack2.WasPressedThisFrame() && skills.fallingAttackUnlocked)
                {
                    anim.SetTrigger("AttackHeavyAir");
                    dm.runSpeed = 0.0f;
                    state.SetState(DANTE_STATE.ATTACKING_FALLING);
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

    public IEnumerator Thrust()
    {
        dm.DanteStop();
        float delay = state.demon ? 0.5f : 1.0f;
        yield return new WaitForSeconds(delay);
        canThrust = true;
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
