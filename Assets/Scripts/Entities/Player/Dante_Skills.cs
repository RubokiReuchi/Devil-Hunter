using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dante_Skills : MonoBehaviour
{
    // skills
    [Header("Dash")]
    /*[NonEditable]*/ public int dashLevel;
    public bool pierceDashAvailable;
    public ParticleSystem pierceReadyParticles;

    [Header("Wall Sliding")]
    public bool wallSlidingUnlocked;

    [Header("Double Jump")]
    public bool dobleJumpUnlocked;

    [Header("Revive")]
    public bool reviveUnlocked;

    [Header("Attacks")]
    public bool waveUnlocked;
    public bool grabUnlocked;
    public bool fallingAttackUnlocked;
    public bool thrustUnlocked;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        anim.SetInteger("Dash Level", dashLevel);
    }

    void Update()
    {
        // test
        anim.SetInteger("Dash Level", dashLevel);
        anim.SetBool("PierceDashAvailable", pierceDashAvailable);
    }

    public void DashLevelUp()
    {
        dashLevel++;
        if (dashLevel == 2) pierceDashAvailable = true;
    }

    public IEnumerator StartPierceCooldown()
    {
        pierceDashAvailable = false;
        yield return new WaitForSeconds(5);
        pierceDashAvailable = true;
        pierceReadyParticles.Play();
    }
}
