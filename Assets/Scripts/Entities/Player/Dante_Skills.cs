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
