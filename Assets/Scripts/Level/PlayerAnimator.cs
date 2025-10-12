using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("Graphics")]
    public Transform spriteGO;
    public SpriteRenderer spriteRender;

    [Header("Animation")]
    public Animator animator;
    public float walkThreshold = 0.1f;

    [Header("Attack Settings")]
    public float attackCooldown = 0.5f;

    public UserInput userInput;

    private Rigidbody2D rb;
    private int lastFacing = 1;
    private float lastAtkTime = -999f;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRender = spriteGO.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector2 vel = rb.linearVelocity;

        Vector2 grav = Physics2D.gravity.sqrMagnitude < 1e-6f ? Vector2.down : Physics2D.gravity.normalized;
        Vector2 lateral = new Vector2(-grav.y, grav.x);
        float lateralSpeed = Vector2.Dot(vel, lateral);

        bool isWalkingRight = lateralSpeed > walkThreshold;
        bool isWalkingLeft = lateralSpeed < -walkThreshold;
        bool isGrounded = userInput != null ? userInput.CheckIsGrounded() : true;

        if (isWalkingRight) lastFacing = 1;
        if (isWalkingLeft) lastFacing = -1;
        spriteRender.flipX = lastFacing < 0;


        animator.SetBool("isJumping", !isGrounded);
        if (!isGrounded)
        {
            animator.SetBool("isWalkingRight", false);
            animator.SetBool("isWalkingLeft", false);
        }
        else
        {
            animator.SetBool("isWalkingRight", isWalkingRight);
            animator.SetBool("isWalkingLeft", isWalkingLeft);
            PlayerAttack();
        }
    }

    private void PlayerAttack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time >= lastAtkTime + attackCooldown)
        {
            lastAtkTime = Time.time;

            animator.SetTrigger("isAttacking");
        }
    }
}
