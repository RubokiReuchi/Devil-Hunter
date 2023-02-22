using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dante_Skills : MonoBehaviour, DataPersistenceInterfice
{
    // skills
    [Header("Dash")]
    /*[NonEditable]*/ public int dashLevel;
    public bool pierceDashAvailable;
    public ParticleSystem pierceReadyParticles;

    [Header("Wall Sliding")]
    public bool wallSlidingUnlocked;

    [Header("Double Jump")]
    public bool doubleJumpUnlocked;

    [Header("Revive")]
    public bool reviveUnlocked;
    public bool canRevive;

    [Header("Attacks")]
    public bool waveUnlocked;
    public bool grabUnlocked;
    public bool fallingAttackUnlocked;
    public bool thrustUnlocked;

    Animator anim;

    public void LoadData(GameData data)
    {
        dashLevel = data.dashLevel;
        wallSlidingUnlocked = data.wallSlidingUnlocked;
        doubleJumpUnlocked = data.doubleJumpUnlocked;
        reviveUnlocked = data.reviveUnlocked;
        canRevive = data.canRevive;
        waveUnlocked = data.waveUnlocked;
        grabUnlocked = data.grabUnlocked;
        fallingAttackUnlocked = data.fallingAttackUnlocked;
        thrustUnlocked = data.thrustUnlocked;
    }

    public void SaveData(GameData data)
    {
        data.dashLevel = dashLevel;
        data.wallSlidingUnlocked = wallSlidingUnlocked;
        data.doubleJumpUnlocked = doubleJumpUnlocked;
        data.reviveUnlocked = reviveUnlocked;
        data.canRevive = canRevive;
        data.waveUnlocked = waveUnlocked;
        data.grabUnlocked = grabUnlocked;
        data.fallingAttackUnlocked = fallingAttackUnlocked;
        data.thrustUnlocked = thrustUnlocked;
    }

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
