using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dante_Movement : MonoBehaviour
{
    Dante_StateMachine state;
    public GameManager game_manager;
    Dante_Skills skills;

    Rigidbody2D rb;
    Animator anim;

    [Header("Movement")]
    public float basicRunSpeed;
    public float basicWalkSpeed;
    [HideInInspector] public float runSpeed;
    [HideInInspector] public float walkSpeed;
    float fixed_run_speed;
    float fixed_walk_speed;

    [Header("Jump")]
    public float jumpForce;
    bool isJumping;
    [Range(0.0f, 1.0f)] public float jumpCutMultiplier;
    public float startFallGravityMultiplier;
    public float fallGravityMultiplierIncrease;
    public float maxFallGravityMultiplier;
    float fallGravityMultiplier;
    float gravityScale;

    [Header("Dash")]
    [NonEditable] public bool iframe;
    public float rollVelocity;
    public float dashVelocity;
    [HideInInspector] public Vector2 dashDirection;
    public TrailRenderer dashTrail;
    public Gradient dashTrailColor;
    public Gradient pierceDashTrailColor;

    [Header("Wall Sliding and Wall Jump")]
    public float wallSlidingSpeed;
    [NonEditable][SerializeField] bool isWallSliding;
    bool isWallSlidingStuck;
    bool canMoveAfterWallSliding;

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
    [NonEditable] public bool isOnSlope;
    [SerializeField] PhysicsMaterial2D noFriction;
    [SerializeField] PhysicsMaterial2D fullFriction;
    float slopeDir;

    // Check lateral
    [Header("Check Lateral")]
    public Vector2 boxSizeLateral;
    public float maxDistanceLeft;
    public float maxDistanceRight;
    public float maxDistanceUp;
    public LayerMask layerMaskLateral;
    bool wallOnRight;
    bool wallOnLeft;

    // Start is called before the first frame update
    void Start()
    {
        state = GetComponent<Dante_StateMachine>();

        skills = GetComponent<Dante_Skills>();

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        runSpeed = basicRunSpeed;
        walkSpeed = basicWalkSpeed;

        iframe = false;

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

        // Checkers
        isOnGround = Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, maxDistance, layerMask);
        nullGravity = isOnGround || (state.dash && skills.dashLevel > 0); 
        wallOnRight = Physics2D.BoxCast(transform.position + transform.up * maxDistanceUp, boxSizeLateral, 0, transform.right, maxDistanceRight * transform.localScale.x, layerMaskLateral);
        wallOnLeft = Physics2D.BoxCast(transform.position + transform.up * maxDistanceUp, boxSizeLateral, 0, -transform.right, maxDistanceLeft * transform.localScale.x, layerMaskLateral);

        // Reset Air Skills
        if (isOnGround)
        {
            anim.SetBool("Can LightAir", true);
            anim.SetBool("Can AirDash", true);
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
                if (Input.GetButtonDown("Jump") && !isWallSliding) StartJump();
                if (Input.GetButtonDown("Dash"))
                {
                    StartDash();
                }
            }
            else
            {
                if (Input.GetButtonDown("Dash") && anim.GetBool("Can AirDash") && skills.dashLevel > 0)
                {
                    StartDash();
                }
            }
        }

        // Slope
        SlopeCheck();
        if (isOnSlope) dashDirection = new Vector2(transform.localScale.x * -slopeNormalPerpendicular.x, slopeDir * -slopeNormalPerpendicular.y).normalized;
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
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0 && isJumping)
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
        if (!wallOnLeft && !isJumping) isWallSliding = false;

        if (wallOnRight && !nullGravity && Input.GetAxisRaw("Horizontal") == transform.localScale.x)
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            isWallSliding = true;
            canMoveAfterWallSliding = false;
            isJumping = false;
            state.SetState(DANTE_STATE.WALL_SLIDING);
            anim.SetBool("Can AirDash", true);
        }

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
            if (Input.GetButtonDown("Jump"))
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
    }

    void StartDash()
    {
        anim.SetTrigger("Dash");
        state.SetState(DANTE_STATE.DASHING);
        state.dash = true;
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
        if (state.IsAiming())
        {
            if (state.orientation != anim.GetFloat("Walk"))
            {
                transform.localScale = new Vector3(-state.orientation, 1, 1);
            }
        }
    }

    void Walk()
    {
        if (state.IsDashing()) return;
        if (Input.GetAxisRaw("Horizontal") > 0 && !wallOnRight)
        {
            if (nullGravity && isOnSlope) transform.position += new Vector3(fixed_walk_speed * -slopeNormalPerpendicular.x, fixed_walk_speed * -slopeNormalPerpendicular.y);
            else transform.position += new Vector3(fixed_walk_speed, 0);
            transform.localScale = new Vector3(state.orientation, 1, 1);
            anim.SetBool("Moving", true);
            anim.SetFloat("Walk", 1.0f);
            if (!state.IsAttacking() && nullGravity) state.SetState(DANTE_STATE.WALK);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0 && !wallOnLeft)
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
        if (state.IsDashing() || !canMoveAfterWallSliding) return;
        if (Input.GetAxisRaw("Horizontal") > 0 && (!wallOnRight || !isOnGround))
        {
            if (!isOnGround && rb.velocity.x < 0) rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
            if (nullGravity && isOnSlope) transform.position += new Vector3(fixed_run_speed * -slopeNormalPerpendicular.x, fixed_run_speed * -slopeNormalPerpendicular.y);
            else transform.position += new Vector3(fixed_run_speed, 0);
            transform.localScale = new Vector3(1, 1, 1);
            anim.SetBool("Moving", true);
            if (state.CompareState(DANTE_STATE.IDLE)) state.SetState(DANTE_STATE.RUN);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0 && (!wallOnLeft || !isOnGround))
        {
            if (!isOnGround && rb.velocity.x > 0) rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
            if (nullGravity && isOnSlope) transform.position += new Vector3(fixed_run_speed * slopeNormalPerpendicular.x, fixed_run_speed * slopeNormalPerpendicular.y);
            else transform.position += new Vector3(-fixed_run_speed, 0);
            transform.localScale = new Vector3(-1, 1, 1);
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

    void Sliding()
    {
        if (transform.localScale.x == 1)
        {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                StartCoroutine("Co_StopWallSliding");
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
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
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                StartCoroutine("Co_StopWallSliding");
            }
            else if (Input.GetAxisRaw("Horizontal") > 0)
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

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != slopeDownAngleOld) isOnSlope = true;

            slopeDownAngleOld = slopeDownAngle;

            Vector2 checkPos2 = checkPos + Vector2.up * 0.05f;
            RaycastHit2D hit2 = Physics2D.Raycast(checkPos2, Vector2.right * transform.localScale.x, slopeCheckDistance, layerMask);

            if (hit2) slopeDir = 1;
            else slopeDir = -1;

            Debug.DrawLine(checkPos2, checkPos2 + Vector2.right * transform.localScale.x * slopeCheckDistance, Color.blue);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
            Debug.DrawRay(hit.point, slopeNormalPerpendicular, Color.red);
        }
        else
        {
            slopeDir = 0;
        }

        if (isOnSlope && nullGravity && !state.dash) rb.sharedMaterial = fullFriction;
        else rb.sharedMaterial = noFriction;
    }

    public void DantePrepareDeath()
    {
        GetComponent<Dante_Attack>().enabled = false;
        this.enabled = false;
    }

    public void DanteDeath()
    {
        game_manager.SaveDeathPos(transform.position);
        Destroy(gameObject);
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
            collision.GetComponent<SandClock>().onClock = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Sand Clock
        if (collision.CompareTag("SandClock"))
        {
            collision.GetComponent<SandClock>().onClock = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position - transform.up * maxDistance, boxSize);

        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(transform.position - transform.right * maxDistanceLeft * transform.localScale.x + transform.up * maxDistanceUp, boxSizeLateral);
        Gizmos.DrawCube(transform.position + transform.right * maxDistanceRight * transform.localScale.x + transform.up * maxDistanceUp, boxSizeLateral);
    }
}