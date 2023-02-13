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
    public float rollForce;
    [NonEditable] public bool iframe;

    // Check grounded
    [Header("Check Grounded")]
    public Vector2 boxSize;
    public Vector2 smallBoxSize;
    public float maxDistance;
    public LayerMask layerMask;

    // Climb
    [Header("Climb")]
    [SerializeField] float slopeCheckDistance;
    CapsuleCollider2D cc;
    float slopeDownAngle;
    float slopeDownAngleOld;
    Vector2 slopeNormalPerpendicular;
    [NonEditable][SerializeField] bool isOnSlope;
    [SerializeField] PhysicsMaterial2D noFriction;
    [SerializeField] PhysicsMaterial2D fullFriction;
    bool colliderInGround;

    // Check lateral
    [Header("Check Lateral")]
    public Vector2 boxSizeLateral;
    public float maxDistanceLeft;
    public float maxDistanceRight;
    public float maxDistanceUp;
    public LayerMask layerMaskLateral;

    // Start is called before the first frame update
    void Start()
    {
        state = GetComponent<Dante_StateMachine>();

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        runSpeed = basicRunSpeed;
        walkSpeed = basicWalkSpeed;

        iframe = false;

        isJumping = false;
        fallGravityMultiplier = startFallGravityMultiplier;
        gravityScale = rb.gravityScale;

        cc = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!state.IsAlive()) return;

        bool inGround = Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, maxDistance, layerMask);
        colliderInGround = Physics2D.BoxCast(transform.position, smallBoxSize, 0, -transform.up, maxDistance, layerMask);

        if (state.InGround() && !inGround)
        {
            anim.SetTrigger("Fall edge");
        }

        float fixed_run_speed = runSpeed * Time.deltaTime;
        float fixed_walk_speed = walkSpeed * Time.deltaTime;

        if (state.InGround() && !state.IsRolling())
        {
            if (Input.GetButtonDown("Jump"))
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                anim.SetTrigger("Jump");
                state.SetState(DANTE_STATE.JUMPING);
                isJumping = true;
            }
            if (Input.GetButtonDown("Roll"))
            {
                anim.SetTrigger("Roll");
                state.SetState(DANTE_STATE.ROLLING);
            }
        }

        // Fix little velocity Y variation when rolling
        if (state.IsRolling()) rb.velocity = new Vector2(rb.velocity.x, 0);

        // Release jump
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0 && isJumping)
        {
            rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
            isJumping = false;
        }
        
        if (rb.velocity.y < 0 && !colliderInGround)
        {
            if (fallGravityMultiplier + fallGravityMultiplierIncrease <= maxFallGravityMultiplier) fallGravityMultiplier += fallGravityMultiplierIncrease * Time.deltaTime;
            rb.gravityScale = gravityScale * fallGravityMultiplier;
        }
        else
        {
            fallGravityMultiplier = startFallGravityMultiplier;
            rb.gravityScale = colliderInGround ? 0 : gravityScale;
        }

        SlopeCheck();

        if (state.IsAiming() && state.InGround())
        {
            anim.SetBool("Aiming", true);
            if (Input.GetAxisRaw("Horizontal") > 0 && !state.IsRolling() && !Physics2D.BoxCast(transform.position + transform.up * maxDistanceUp, boxSizeLateral, 0, transform.right, maxDistanceRight * transform.localScale.x, layerMaskLateral))
            {
                if (inGround && isOnSlope) transform.position += new Vector3(fixed_walk_speed * -slopeNormalPerpendicular.x, fixed_walk_speed * -slopeNormalPerpendicular.y);
                else transform.position += new Vector3(fixed_walk_speed, 0);
                transform.localScale = new Vector3(state.orientation, 1, 1);
                anim.SetBool("Moving", true);
                anim.SetFloat("Walk", 1.0f);
                if (!state.IsAttacking() && state.InGround()) state.SetState(DANTE_STATE.WALK);
            }
            else if (Input.GetAxisRaw("Horizontal") < 0 && !state.IsRolling() && !Physics2D.BoxCast(transform.position + transform.up * maxDistanceUp, boxSizeLateral, 0, -transform.right, maxDistanceLeft * transform.localScale.x, layerMaskLateral))
            {
                if (inGround && isOnSlope) transform.position += new Vector3(fixed_walk_speed * slopeNormalPerpendicular.x, fixed_walk_speed * slopeNormalPerpendicular.y);
                else transform.position += new Vector3(-fixed_walk_speed, 0);
                transform.localScale = new Vector3(state.orientation, 1, 1);
                anim.SetBool("Moving", true);
                anim.SetFloat("Walk", -1.0f);
                if (!state.IsAttacking() && state.InGround()) state.SetState(DANTE_STATE.WALK);
            }
            else if (!state.IsRolling())
            {
                anim.SetBool("Moving", false);
                anim.SetFloat("Walk", 0.0f);
                if (state.CompareState(DANTE_STATE.WALK)) state.SetState(DANTE_STATE.IDLE);
            }
        }
        else
        {
            anim.SetBool("Aiming", false);
            if (Input.GetAxisRaw("Horizontal") > 0 && !state.IsRolling() && !Physics2D.BoxCast(transform.position + transform.up * maxDistanceUp, boxSizeLateral, 0, transform.right * transform.localScale.x, maxDistanceRight * transform.localScale.x, layerMaskLateral))
            {
                if (inGround && isOnSlope) transform.position += new Vector3(fixed_run_speed * -slopeNormalPerpendicular.x, fixed_run_speed * -slopeNormalPerpendicular.y);
                else transform.position += new Vector3(fixed_run_speed, 0);
                transform.localScale = new Vector3(1, 1, 1);
                anim.SetBool("Moving", true);
                if (!state.IsAttacking() && state.InGround()) state.SetState(DANTE_STATE.RUN);
            }
            else if (Input.GetAxisRaw("Horizontal") < 0 && !state.IsRolling() && !Physics2D.BoxCast(transform.position + transform.up * maxDistanceUp, boxSizeLateral, 0, -transform.right * transform.localScale.x, maxDistanceLeft * transform.localScale.x, layerMaskLateral))
            {
                if (inGround && isOnSlope) transform.position += new Vector3(fixed_run_speed * slopeNormalPerpendicular.x, fixed_run_speed * slopeNormalPerpendicular.y);
                else transform.position += new Vector3(-fixed_run_speed, 0);
                transform.localScale = new Vector3(-1, 1, 1);
                anim.SetBool("Moving", true);
                if (!state.IsAttacking() && state.InGround()) state.SetState(DANTE_STATE.RUN);
            }
            else
            {
                anim.SetBool("Moving", false);
                if (state.CompareState(DANTE_STATE.RUN)) state.SetState(DANTE_STATE.IDLE);
            }
        }
    }

    public void IFrameToggle()
    {
        iframe = !iframe;
        if (iframe)
        {
            rb.velocity = Vector2.zero;
            if (!state.IsAiming()) rb.AddForce(Vector2.right * rollForce * transform.localScale.x, ForceMode2D.Impulse);
            else if (anim.GetFloat("Walk") != 0) rb.AddForce(Vector2.right * rollForce * anim.GetFloat("Walk"), ForceMode2D.Impulse);
            else rb.AddForce(Vector2.left * rollForce * state.orientation, ForceMode2D.Impulse);
        }
        else
        {
            rb.velocity = Vector2.zero;
            state.SetState(DANTE_STATE.IDLE);
        }
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
        }

        if (isOnSlope && colliderInGround) rb.sharedMaterial = fullFriction;
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(transform.position - transform.up * maxDistance, boxSize);
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position - transform.up * maxDistance, smallBoxSize);

        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(transform.position - transform.right * maxDistanceLeft * transform.localScale.x + transform.up * maxDistanceUp, boxSizeLateral);
        Gizmos.DrawCube(transform.position + transform.right * maxDistanceRight * transform.localScale.x + transform.up * maxDistanceUp, boxSizeLateral);
    }
}