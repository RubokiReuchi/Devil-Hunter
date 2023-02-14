using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Dante_Attack : MonoBehaviour
{
    Dante_StateMachine state;
    Dante_Stats stats;
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
        stats = GetComponent<Dante_Stats>();
        dm = GetComponent<Dante_Movement>();
        anim = GetComponent<Animator>();

        canShoot = true;
        canThrust = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (state.IsRolling() || !state.IsAlive()) return;

        if (!state.IsAiming())
        {
            if (state.InGround())
            {
                if (Input.GetButtonDown("Attack1"))
                {
                    anim.SetTrigger("Attack1");
                    dm.runSpeed = 0.5f;
                    state.SetState(DANTE_STATE.ATTACKING_GROUND);
                }
                else if (Input.GetButtonDown("Attack2") && canThrust)
                {
                    anim.SetBool("Thrust", true);
                    dm.runSpeed = 0.0f;
                    state.SetState(DANTE_STATE.ATTACKING_GROUND);
                    canThrust = false;
                }
            }
            else
            {
                if (Input.GetButtonDown("Attack1"))
                {
                    anim.SetTrigger("AttackLightAir");
                    state.SetState(DANTE_STATE.ATTACKING_AIR);
                }
                else if (Input.GetButtonDown("Attack2"))
                {
                    anim.SetTrigger("AttackHeavyAir");
                    dm.runSpeed = 0.0f;
                    state.SetState(DANTE_STATE.ATTACKING_FALLING);
                }
            }
        }
        else
        {
            if (state.InGround())
            {
                if (Input.GetButtonDown("Attack1"))
                {
                    anim.SetTrigger("AttackHeavy1");
                    state.SetState(DANTE_STATE.ATTACKING_AIR);
                }
                else if (Input.GetButtonDown("Attack2") && !state.IsAttacking() && canShoot
                    && (state.CompareState(DANTE_STATE.IDLE) || state.CompareState(DANTE_STATE.WALK)))
                {
                    state.SetState(DANTE_STATE.SHOTING);
                    anim.SetTrigger("Shoot");
                }
            }
            else
            {
                if (Input.GetButtonDown("Attack1"))
                {
                    
                }
                else if (Input.GetButtonDown("Attack2"))
                {
                    
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
