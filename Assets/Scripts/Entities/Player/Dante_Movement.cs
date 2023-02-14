using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dante_Movement : MonoBehaviour
{
    Dante_StateMachine state;
    public GameManager game_manager;

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

    [Header("Roll")]
    [NonEditable] public bool iframe;
    [HideInInspector] public bool dashing;
    public float dashVelocity;
    [HideInInspector] public Vector2 dashDirection;

    // Check grounded
    [Header("Check Grounded")]
    public Vector2 boxSize;
    public float maxDistance;
    public LayerMask layerMask;
    [HideInInspector] public bool isOnGround;

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

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        runSpeed = basicRunSpeed;
        walkSpeed = basicWalkSpeed;

        iframe = false;
        dashing = false;

        isJumping = false;
        fallGravityMultiplier = startFallGravityMultiplier;
        gravityScale = rb.gravityScale;

        cc = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!state.IsAlive() || state.IsInteracting()) return;

        // Checkers
        isOnGround = Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, maxDistance, layerMask);
        wallOnRight = Physics2D.BoxCast(transform.position + transform.up * maxDistanceUp, boxSizeLateral, 0, transform.right, maxDistanceRight * transform.localScale.x, layerMaskLateral);
        wallOnLeft = Physics2D.BoxCast(transform.position + transform.up * maxDistanceUp, boxSizeLateral, 0, -transform.right, maxDistanceLeft * transform.localScale.x, layerMaskLateral);

        // Start to Fall
        if (state.InGround() && !isOnGround)
        {
            anim.SetTrigger("Fall edge");
        }

        // Start Jump, Roll and Dash
        if (state.InGround() && !state.IsRolling())
        {
            if (Input.GetButtonDown("Jump")) StartJump();
            if (Input.GetButtonDown("Roll")) StartRoll();
        }

        // Slope
        SlopeCheck();
        if (isOnSlope) dashDirection = new Vector2(transform.localScale.x * -slopeNormalPerpendicular.x, slopeDir * -slopeNormalPerpendicular.y).normalized;

        // On Roll
        if (iframe)
        {
            rb.velocity = dashDirection * dashVelocity;
            dashing = true;

            if (!isOnGround) rb.velocity = new Vector2(0, rb.velocity.y);
        }

        // Release Jump
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0 && isJumping)
        {
            rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
            isJumping = false;
        }

        // On Air
        if (rb.velocity.y < 0)
        {
            if (fallGravityMultiplier + fallGravityMultiplierIncrease <= maxFallGravityMultiplier) fallGravityMultiplier += fallGravityMultiplierIncrease * Time.deltaTime;
            rb.gravityScale = gravityScale * fallGravityMultiplier;
        }
        else
        {
            fallGravityMultiplier = startFallGravityMultiplier;
            rb.gravityScale = gravityScale;
        }

        // Movement
        if (state.IsAiming() && state.InGround())
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
    
    void StartJump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        anim.SetTrigger("Jump");
        state.SetState(DANTE_STATE.JUMPING);
        isJumping = true;
    }

    void StartRoll()
    {
        anim.SetTrigger("Roll");
        state.SetState(DANTE_STATE.ROLLING);
    }

    public void IFrameToggle()
    {
        iframe = !iframe;
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
        if (state.IsRolling()) return;
        if (Input.GetAxisRaw("Horizontal") > 0 && !wallOnRight)
        {
            if (isOnGround && isOnSlope) transform.position += new Vector3(fixed_walk_speed * -slopeNormalPerpendicular.x, fixed_walk_speed * -slopeNormalPerpendicular.y);
            else transform.position += new Vector3(fixed_walk_speed, 0);
            transform.localScale = new Vector3(state.orientation, 1, 1);
            anim.SetBool("Moving", true);
            anim.SetFloat("Walk", 1.0f);
            if (!state.IsAttacking() && state.InGround()) state.SetState(DANTE_STATE.WALK);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0 && !wallOnLeft)
        {
            if (isOnGround && isOnSlope) transform.position += new Vector3(fixed_walk_speed * slopeNormalPerpendicular.x, fixed_walk_speed * slopeNormalPerpendicular.y);
            else transform.position += new Vector3(-fixed_walk_speed, 0);
            transform.localScale = new Vector3(state.orientation, 1, 1);
            anim.SetBool("Moving", true);
            anim.SetFloat("Walk", -1.0f);
            if (!state.IsAttacking() && state.InGround()) state.SetState(DANTE_STATE.WALK);
        }
        else
        {
            anim.SetBool("Moving", false);
            anim.SetFloat("Walk", 0.0f);
            if (state.CompareState(DANTE_STATE.WALK)) state.SetState(DANTE_STATE.IDLE);
        }
    }

    void Run()
    {
        if (state.IsRolling()) return;
        if (Input.GetAxisRaw("Horizontal") > 0 && !wallOnRight)
        {
            if (isOnGround && isOnSlope) transform.position += new Vector3(fixed_run_speed * -slopeNormalPerpendicular.x, fixed_run_speed * -slopeNormalPerpendicular.y);
            else transform.position += new Vector3(fixed_run_speed, 0);
            transform.localScale = new Vector3(1, 1, 1);
            anim.SetBool("Moving", true);
            if (!state.IsAttacking() && state.InGround()) state.SetState(DANTE_STATE.RUN);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0 && !wallOnLeft)
        {
            if (isOnGround && isOnSlope) transform.position += new Vector3(fixed_run_speed * slopeNormalPerpendicular.x, fixed_run_speed * slopeNormalPerpendicular.y);
            else transform.position += new Vector3(-fixed_run_speed, 0);
            transform.localScale = new Vector3(-1, 1, 1);
            anim.SetBool("Moving", true);
            if (!state.IsAttacking() && state.InGround()) state.SetState(DANTE_STATE.RUN);
        }
        else
        {
            anim.SetBool("Moving", false);
            anim.SetFloat("Walk", 0.0f);
            if (state.CompareState(DANTE_STATE.RUN)) state.SetState(DANTE_STATE.IDLE);
        }
    }

    void SlopeCheck()
    {
        Vector2 checkPos = transform.position - new Vector3(0.0f, cc.size.y / 2);

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

        if (isOnSlope && isOnGround && !dashing) rb.sharedMaterial = fullFriction;
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
        dashing = false;
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