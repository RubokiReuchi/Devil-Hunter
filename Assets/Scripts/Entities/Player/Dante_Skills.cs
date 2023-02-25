using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dante_Skills : MonoBehaviour, DataPersistenceInterfice
{
    public static Dante_Skills instance;

    // skills
    [Header("Dash")]
    public int dashLevel;
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
    public bool attack3Unlocked;
    public bool attack4Unlocked;
    public bool attack5Unlocked;
    public bool combo1Unlocked;
    public bool combo2Unlocked;
    public bool combo3Unlocked;
    public bool combo4Unlocked;
    public bool fallingAttackUnlocked;
    public bool chargeUnlocked;
    public bool grabUnlocked;
    public bool ultUnlocked;

    Animator anim;

    public void LoadData(GameData data)
    {
        dashLevel = data.dashLevel;
        wallSlidingUnlocked = data.wallSlidingUnlocked;
        doubleJumpUnlocked = data.doubleJumpUnlocked;
        reviveUnlocked = data.reviveUnlocked;
        canRevive = data.canRevive;
        attack3Unlocked = data.attack3Unlocked;
        attack4Unlocked = data.attack4Unlocked;
        attack5Unlocked = data.attack5Unlocked;
        combo1Unlocked = data.combo1Unlocked;
        combo2Unlocked = data.combo2Unlocked;
        combo3Unlocked = data.combo3Unlocked;
        combo4Unlocked = data.combo4Unlocked;
        fallingAttackUnlocked = data.fallingAttackUnlocked;
        chargeUnlocked = data.chargeUnlocked;
        grabUnlocked = data.grabUnlocked;
        ultUnlocked = data.ultUnlocked;
    }

    public void SaveData(GameData data)
    {
        data.dashLevel = dashLevel;
        data.wallSlidingUnlocked = wallSlidingUnlocked;
        data.doubleJumpUnlocked = doubleJumpUnlocked;
        data.reviveUnlocked = reviveUnlocked;
        data.canRevive = canRevive;
        data.attack3Unlocked = attack3Unlocked;
        data.attack4Unlocked = attack4Unlocked;
        data.attack5Unlocked = attack5Unlocked;
        data.combo1Unlocked = combo1Unlocked;
        data.combo2Unlocked = combo2Unlocked;
        data.combo3Unlocked = combo3Unlocked;
        data.combo4Unlocked = combo4Unlocked;
        data.fallingAttackUnlocked = fallingAttackUnlocked;
        data.chargeUnlocked = chargeUnlocked;
        data.grabUnlocked = grabUnlocked;
        data.ultUnlocked = ultUnlocked;
    }

    private void Awake()
    {
        instance = this;
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
