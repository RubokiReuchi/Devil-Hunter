using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dante_Movement : MonoBehaviour, DataPersistenceInterfice
{
    Dante_StateMachine state;
    public GameManager game_manager;
    [HideInInspector] public CameraActions camActions;
    Dante_Stats stats;
    Dante_Skills skills;
    Dante_Menus menus;

    Dante_Hitbox hitbox;
    [NonEditable] public bool inmune;

    Rigidbody2D rb;
    Animator anim;

    [Header("Movement")]
    [HideInInspector] public Vector2 lastPositionOnGround;
    public float basicRunSpeed;
    public float basicWalkSpeed;
    [HideInInspector] public float runSpeed;
    [HideInInspector] public float walkSpeed;
    float fixed_run_speed;
    float fixed_walk_speed;

    [Header("Jump")]
    public float jumpForce;
    [NonEditable] public bool isJumping;
    [Range(0.0f, 1.0f)] public float jumpCutMultiplier;
    public float startFallGravityMultiplier;
    public float fallGravityMultiplierIncrease;
    public float maxFallGravityMultiplier;
    float fallGravityMultiplier;
    float gravityScale;
    public GameObject airJumpSprite;

    [Header("Dash")]
    [NonEditable] public bool iframe;
    public float rollVelocity;
    public float dashVelocity;
    bool canDash;
    [HideInInspector] public Vector2 dashDirection;
    public TrailRenderer dashTrail;
    public Gradient dashTrailColor;
    public Gradient pierceDashTrailColor;

    [Header("Wall Sliding and Wall Jump")]
    public float wallSlidingSpeed;
    [NonEditable][SerializeField] bool isWallSliding;
    bool isWallSlidingStuck;
    bool canMoveAfterWallSliding;
    [SerializeField] bool onWallBeforeJump;

    [Header("Hitt")]
    public ParticleSystem hitParticles;

    // Check grounded
    [Header("Check Grounded")]
    public Vector2 boxSize;
    public float maxDistance;
    public LayerMask layerMask;
    [HideInInspector] public bool isOnGround;
    [HideInInspector] public bool nullGravity;
    bool lastNullGravity;

    // Climb
    [Header("Climb")]
    [SerializeField] float slopeCheckDistance;
    CapsuleCollider2D cc;
    float slopeDownAngle;
    float slopeDownAngleOld;
    Vector2 slopeNormalPerpendicular;
    Vector2 slopeNormalPerpendicularInverse;
    [NonEditable] public bool isOnSlope;
    [SerializeField] PhysicsMaterial2D noFriction;
    [SerializeField] PhysicsMaterial2D fullFriction;

    // Check lateral
    [Header("Check Lateral")]
    public Vector2 boxSizeLateral;
    public float maxDistanceLeft;
    public float maxDistanceRight;
    public float maxDistanceUp;
    public LayerMask layerMaskLateral;
    bool wallOnRight;
    bool wallOnLeft;

    // Set Input System
    PlayerInputActions inputActions;
    public PlayerInputActions.NormalActions input;
    public float moveInput;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        input = inputActions.Normal;
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void LoadData(GameData data)
    {
        transform.position = data.position;
    }

    public void SaveData(GameData data)
    {
        data.position = game_manager.resetLevel ? game_manager.lastClockPosition : lastPositionOnGround;
    }

    // Start is called before the first frame update
    void Start()
    {
        camActions = game_manager.GetComponentInChildren<CameraActions>();

        state = GetComponent<Dante_StateMachine>();
        stats = GetComponent<Dante_Stats>();

        skills = GetComponent<Dante_Skills>();

        menus = GetComponent<Dante_Menus>();

        hitbox = GetComponent<Dante_Hitbox>();
        inmune = false;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        runSpeed = basicRunSpeed;
        walkSpeed = basicWalkSpeed;

        iframe = false;
        canDash = true;

        isJumping = false;
        fallGravityMultiplier = startFallGravityMultiplier;
        gravityScale = rb.gravityScale;

        canMoveAfterWallSliding = true;

        cc = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!state.IsAlive() || state.IsInteracting()) return;
        moveInput = input.Move.ReadValue<float>();

        // Checkers
        isOnGround = Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, maxDistance, layerMask);
        nullGravity = isOnGround || (state.dash && skills.dashLevel > 0); 
        wallOnRight = Physics2D.BoxCast(transform.position + transform.up * maxDistanceUp, boxSizeLateral, 0, transform.right, maxDistanceRight * transform.localScale.x, layerMaskLateral);
        wallOnLeft = Physics2D.BoxCast(transform.position + transform.up * maxDistanceUp, boxSizeLateral, 0, -transform.right, maxDistanceLeft * transform.localScale.x, layerMaskLateral);

        // Reset Air Skills
        if (isOnGround)
        {
            lastPositionOnGround = transform.position;

            anim.SetBool("Can LightAir", true);
            anim.SetBool("Can AirDash", true);
            anim.SetBool("Can AirJump", true);
            anim.SetBool("Moving", false);
            if (!state.IsDashing()) anim.ResetTrigger("Dash");
            anim.ResetTrigger("AttackLightAir");
            anim.ResetTrigger("AttackHeavyAir");

            isWallSliding = false;
            canMoveAfterWallSliding = true;
        }
        
        // Start to Fall
        if (!nullGravity && !isJumping && lastNullGravity != nullGravity)
        {
            anim.SetTrigger("Fall edge");
        }
        lastNullGravity = nullGravity;

        // Start Jump, Roll and Dash
        if (!state.IsDashing())
        {
            if (nullGravity)
            {
                if (!state.IsAttacking() && input.Jump.WasPressedThisFrame() && !isWallSliding) StartJump();
                if (input.Dash.WasPressedThisFrame() && canDash)
                {
                    StartDash();
                }
            }
            else
            {
                if (!state.IsAttacking() && input.Jump.WasPressedThisFrame() && !isWallSliding && anim.GetBool("Can AirJump") && skills.doubleJumpUnlocked) StartAirJump();
                if (input.Dash.WasPressedThisFrame() && canDash && anim.GetBool("Can AirDash") && skills.dashLevel > 0)
                {
                    StartDash();
                }
            }
        }

        // Slope
        SlopeCheck();
        if (isOnGround) dashDirection = new Vector2(-slopeNormalPerpendicularInverse.x, -slopeNormalPerpendicularInverse.y).normalized;
        else dashDirection = Vector2.right * transform.localScale.x;

        // On Dash
        if (iframe)
        {
            if (skills.dashLevel == 0) rb.velocity = dashDirection * rollVelocity;
            else rb.velocity = dashDirection * dashVelocity;
            state.dash = true;

            if (!nullGravity && skills.dashLevel == 0) rb.velocity = new Vector2(0, rb.velocity.y);
        }

        // Release Jump
        if (input.Jump.WasReleasedThisFrame() && rb.velocity.y > 0 && isJumping)
        {
            rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
            isJumping = false;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        // On Air
        if (!isWallSliding) isWallSlidingStuck = false;
        if (rb.velocity.y < 0)
        {
            if (fallGravityMultiplier + fallGravityMultiplierIncrease <= maxFallGravityMultiplier) fallGravityMultiplier += fallGravityMultiplierIncrease * Time.deltaTime;
            rb.gravityScale = gravityScale * fallGravityMultiplier;
        }
        else
        {
            fallGravityMultiplier = startFallGravityMultiplier;
            rb.gravityScale = isWallSlidingStuck ? 0 : gravityScale;
        }

        // Wall Sliding
        if (!wallOnLeft && !isJumping)
        {
            isWallSliding = false;
            canMoveAfterWallSliding = true;
        }

        if (wallOnRight && !nullGravity && !onWallBeforeJump && skills.wallSlidingUnlocked && moveInput == transform.localScale.x)
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            isWallSliding = true;
            canMoveAfterWallSliding = false;
            isJumping = false;
            state.SetState(DANTE_STATE.WALL_SLIDING);
            anim.SetBool("Can AirDash", true);
            anim.SetBool("Can AirJump", true);
        }

        // Look Up and Down
        CheckLookAt();

        if (!isWallSliding)
        {
            // Movement
            if (state.IsAiming() && nullGravity)
            {
                anim.SetBool("Aiming", true);
                fixed_walk_speed = walkSpeed * Time.deltaTime;
                Walk();
            }
            else
            {
                anim.SetBool("Aiming", false);
                fixed_run_speed = runSpeed * Time.deltaTime;
                Run();
            }
        }
        else
        {
            Sliding();
            if (input.Jump.WasPressedThisFrame())
            {
                StartWallJump();
            }
        }

        anim.SetBool("Wall Sliding", isWallSliding);
    }
    
    void StartJump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        anim.SetTrigger("Jump");
        state.SetState(DANTE_STATE.JUMPING);
        isJumping = true;
        StartCoroutine("Co_StartJump");
    }

    IEnumerator Co_StartJump()
    {
        onWallBeforeJump = true;
        yield return new WaitForSeconds(0.3f);
        onWallBeforeJump = false;
    }

    void StartAirJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce * 0.8f, ForceMode2D.Impulse);
        anim.SetTrigger("Jump");
        state.SetState(DANTE_STATE.JUMPING);
        isJumping = true;
        StartCoroutine("Co_StartJump");
        anim.SetBool("Can AirJump", false);
        StartCoroutine("AirJumpCercle");

    }

    IEnumerator AirJumpCercle()
    {
        SpriteRenderer sprite = airJumpSprite.GetComponent<SpriteRenderer>();
        airJumpSprite.transform.position = transform.position + Vector3.down * 0.75f;
        sprite.enabled = true;
        if (!state.demon) sprite.color = new Color(0.7f, 0, 0);
        else sprite.color = new Color(0.8f, 0.3f, 0);
        yield return new WaitForSeconds(0.5f);
        sprite.enabled = false;
    }

    void StartDash()
    {
        anim.SetTrigger("Dash");
        state.SetState(DANTE_STATE.DASHING);
        state.dash = true;
        canDash = false;
        StartCoroutine("DashCD");
        if (skills.pierceDashAvailable)
        {
            dashTrail.colorGradient = pierceDashTrailColor; 
            skills.StartCoroutine("StartPierceCooldown");
        }
        else
        {
            dashTrail.colorGradient = dashTrailColor;
        }

        isWallSliding = false;
    }

    IEnumerator DashCD()
    {
        yield return new WaitForSeconds(0.5f);
        canDash = true;
    }

    public void StartDashInmunity()
    {
        iframe = true;
        DanteStop();
    }

    public void EndDashInmunity()
    {
        iframe = false;
        DanteStop();
        if (!iframe) state.SetState(DANTE_STATE.IDLE);
    }

    public void CheckFlipInRoll()
    {
        if (state.orientation != anim.GetFloat("Walk"))
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);

        }
        else if (moveInput != 0 && transform.localScale.x != moveInput)
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
        }
    }

    void Walk()
    {
        if (state.IsAttackingStatic() || state.IsDashing()) return;
        if (moveInput > 0 && !wallOnLeft)
        {
            if (nullGravity && isOnSlope) transform.position += new Vector3(fixed_walk_speed * -slopeNormalPerpendicular.x, fixed_walk_speed * -slopeNormalPerpendicular.y);
            else transform.position += new Vector3(fixed_walk_speed, 0);
            transform.localScale = new Vector3(state.orientation, 1, 1);
            anim.SetBool("Moving", true);
            anim.SetFloat("Walk", 1.0f);
            if (!state.IsAttacking() && nullGravity) state.SetState(DANTE_STATE.WALK);
        }
        else if (moveInput < 0 && !wallOnRight)
        {
            if (nullGravity && isOnSlope) transform.position += new Vector3(fixed_walk_speed * slopeNormalPerpendicular.x, fixed_walk_speed * slopeNormalPerpendicular.y);
            else transform.position += new Vector3(-fixed_walk_speed, 0);
            transform.localScale = new Vector3(state.orientation, 1, 1);
            anim.SetBool("Moving", true);
            anim.SetFloat("Walk", -1.0f);
            if (state.CompareState(DANTE_STATE.IDLE)) state.SetState(DANTE_STATE.WALK);
        }
        else
        {
            anim.SetBool("Moving", false);
            anim.SetFloat("Walk", 0.0f);
            if (state.CompareState(DANTE_STATE.IDLE)) state.SetState(DANTE_STATE.WALK);
        }
    }

    void Run()
    {
        if (state.IsAttackingStatic() || state.IsDashing() || !canMoveAfterWallSliding) return;
        if (!skills.wallSlidingUnlocked)
        {
            if (moveInput > 0)
            {
                if (!state.IsAttacking()) transform.localScale = new Vector3(1, 1, 1);
                if (!wallOnRight)
                {
                    if (!isOnGround && rb.velocity.x < 0) rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
                    if (nullGravity && isOnSlope) transform.position += new Vector3(fixed_run_speed * -slopeNormalPerpendicular.x, fixed_run_speed * -slopeNormalPerpendicular.y);
                    else transform.position += new Vector3(fixed_run_speed, 0);

                    anim.SetBool("Moving", true);
                    if (state.CompareState(DANTE_STATE.IDLE)) state.SetState(DANTE_STATE.RUN);
                }
            }
            else if (moveInput < 0)
            {
                if (!state.IsAttacking()) transform.localScale = new Vector3(-1, 1, 1);
                if (!wallOnRight)
                {
                    if (!isOnGround && rb.velocity.x > 0) rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
                    if (nullGravity && isOnSlope) transform.position += new Vector3(fixed_run_speed * slopeNormalPerpendicular.x, fixed_run_speed * slopeNormalPerpendicular.y);
                    else transform.position += new Vector3(-fixed_run_speed, 0);

                    anim.SetBool("Moving", true);
                    if (state.CompareState(DANTE_STATE.IDLE)) state.SetState(DANTE_STATE.RUN);
                }
            }
            else
            {
                anim.SetBool("Moving", false);
                anim.SetFloat("Walk", 0.0f);
                if (state.CompareState(DANTE_STATE.RUN)) state.SetState(DANTE_STATE.IDLE);
            }
        }
        else
        {
            if (moveInput > 0)
            {
                if (!state.IsAttacking()) transform.localScale = new Vector3(1, 1, 1);
                if (!wallOnRight)
                {
                    if (!isOnGround && rb.velocity.x < 0) rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
                    if (nullGravity && isOnSlope) transform.position += new Vector3(fixed_run_speed * -slopeNormalPerpendicular.x, fixed_run_speed * -slopeNormalPerpendicular.y);
                    else transform.position += new Vector3(fixed_run_speed, 0);

                    anim.SetBool("Moving", true);
                    if (state.CompareState(DANTE_STATE.IDLE)) state.SetState(DANTE_STATE.RUN);
                }
                else
                {
                    anim.SetBool("Moving", false);
                    anim.SetFloat("Walk", 0.0f);
                    if (state.CompareState(DANTE_STATE.RUN)) state.SetState(DANTE_STATE.IDLE);
                }
            }
            else if (moveInput < 0)
            {
                if (!state.IsAttacking()) transform.localScale = new Vector3(-1, 1, 1);
                if (!wallOnRight)
                {
                    if (!isOnGround && rb.velocity.x > 0) rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
                    if (nullGravity && isOnSlope) transform.position += new Vector3(fixed_run_speed * slopeNormalPerpendicular.x, fixed_run_speed * slopeNormalPerpendicular.y);
                    else transform.position += new Vector3(-fixed_run_speed, 0);

                    anim.SetBool("Moving", true);
                    if (state.CompareState(DANTE_STATE.IDLE)) state.SetState(DANTE_STATE.RUN);
                }
                else
                {
                    anim.SetBool("Moving", false);
                    anim.SetFloat("Walk", 0.0f);
                    if (state.CompareState(DANTE_STATE.RUN)) state.SetState(DANTE_STATE.IDLE);
                }
            }
            else
            {
                anim.SetBool("Moving", false);
                anim.SetFloat("Walk", 0.0f);
                if (state.CompareState(DANTE_STATE.RUN)) state.SetState(DANTE_STATE.IDLE);
            }
        }
    }

    void Sliding()
    {
        if (transform.localScale.x == 1)
        {
            if (moveInput > 0)
            {
                StartCoroutine("Co_StopWallSliding");
            }
            else if (moveInput < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                isWallSlidingStuck = true;
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
                isWallSlidingStuck = false;
            }
        }
        else if (transform.localScale.x == -1)
        {
            if (moveInput < 0)
            {
                StartCoroutine("Co_StopWallSliding");
            }
            else if (moveInput > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                isWallSlidingStuck = true;
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
                isWallSlidingStuck = false;
            }
        }
    }

    void StartWallJump()
    {
        rb.AddForce(new Vector2(transform.localScale.x * 0.3f * jumpForce, jumpForce), ForceMode2D.Impulse);
        anim.SetTrigger("Jump");
        state.SetState(DANTE_STATE.JUMPING);
        isJumping = true;
        isWallSliding = false;
        StartCoroutine("Co_StartWallJump");
    }

    IEnumerator Co_StartWallJump()
    {
        yield return new WaitForSeconds(0.25f);
        canMoveAfterWallSliding = true;
    }

    IEnumerator Co_StopWallSliding()
    {
        yield return new WaitForSeconds(0.25f);
        isWallSliding = false;
        canMoveAfterWallSliding = true;
        state.SetState(DANTE_STATE.FALLING);
    }

    void SlopeCheck()
    {
        Vector2 checkPos = transform.position - new Vector3(0.0f, cc.size.y / 2);

        // horizontal
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right * transform.localScale.x, slopeCheckDistance, layerMask);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right * transform.localScale.x, slopeCheckDistance, layerMask);

        if (slopeHitFront) isOnSlope = true;
        else if (slopeHitBack) isOnSlope = true;
        else isOnSlope = false;

        // vertical
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, layerMask);

        if (hit)
        {
            slopeNormalPerpendicular = Vector2.Perpendicular(hit.normal).normalized;
            slopeNormalPerpendicularInverse = slopeNormalPerpendicular * transform.localScale.x;

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != slopeDownAngleOld) isOnSlope = true;

            slopeDownAngleOld = slopeDownAngle;
            
            Debug.DrawRay(hit.point, hit.normal, Color.green);
            Debug.DrawRay(hit.point, slopeNormalPerpendicular, Color.red);
            Debug.DrawRay(hit.point, slopeNormalPerpendicularInverse, Color.blue);
        }

        if (isOnSlope && nullGravity && !state.dash) rb.sharedMaterial = fullFriction;
        else rb.sharedMaterial = noFriction;
    }

    void CheckLookAt()
    {
        if (moveInput != 0)
        {
            camActions.camCentered = true;
            return;
        }
        if (input.LookDown.ReadValue<float>() == 1)
        {
            camActions.LookDown();
        }
        else if (input.LookUp.ReadValue<float>() == 1)
        {
            camActions.LookUp();
        }
        else
        {
            camActions.camCentered = true;
        }
    }

    public void DantePrepareDeath()
    {
        state.SetState(DANTE_STATE.DEATH);
    }

    public void DanteDeath()
    {
        if (state.CompareState(DANTE_STATE.DEATH))
        {
            game_manager.ResetLevel();
        }
    }

    public void DanteStop()
    {
        rb.velocity = Vector2.zero;
    }

    // Collisions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Sand Clock
        if (collision.CompareTag("SandClock"))
        {
            menus.onShop = true;
            game_manager.lastClockPosition = transform.position;
            game_manager.lastClockScene = SceneManager.GetActiveScene().name;
        }
        // Thorns
        else if (collision.CompareTag("Thorn") && !inmune)
        {
            hitbox.TakeDamage(stats.max_hp / 10.0f, transform.position, true);
            StartCoroutine("InmuneTime");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Thorns
        if (collision.CompareTag("Thorn"))
        {
            if (isWallSliding)
            {
                isWallSliding = false;
                state.SetState(DANTE_STATE.FALLING);
                anim.SetBool("Can AirDash", false);
            }
            if (!inmune)
            {
                hitbox.TakeDamage(stats.max_hp / 10.0f, transform.position, true);
                StartCoroutine("InmuneTime");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Sand Clock
        if (collision.CompareTag("SandClock"))
        {
            menus.onShop = false;
        }
    }

    IEnumerator InmuneTime()
    {
        inmune = true;
        camActions.ShakeCamera(0.25f, 2.0f);
        PlayHitParticles(100);
        yield return new WaitForSeconds(0.8f);
        inmune = false;
    }

    public void PlayHitParticles(int force)
    {
        hitParticles.Emit(force);
    }

    public void ResetAllTriggers()
    {
        anim.ResetTrigger("Jump");
        anim.ResetTrigger("Dash");
        anim.ResetTrigger("Attack1");
        anim.ResetTrigger("AttackHeavy1");
        anim.ResetTrigger("AttackLightAir");
        anim.ResetTrigger("AttackHeavyAir");
        anim.ResetTrigger("Shoot");
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position - transform.up * maxDistance, boxSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(transform.position - new Vector3(4 * boxSize.x / 10 * transform.localScale.x, 0, 0) - transform.up * maxDistance, new Vector2(boxSize.x / 3, boxSize.y));

        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(transform.position - transform.right * maxDistanceLeft * transform.localScale.x + transform.up * maxDistanceUp, boxSizeLateral);
        Gizmos.DrawCube(transform.position + transform.right * maxDistanceRight * transform.localScale.x + transform.up * maxDistanceUp, boxSizeLateral);
    }
}