using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    // Plays all the animations for the player
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

        Vector2 grav = Physics2D.gravity.sqrMagnitude < 1e-6f ? Vector2.down : Physics2D.gravity.normalized;          // Gets either the local gravity or the regular gravity
        Vector2 lateral = new Vector2(-grav.y, grav.x);                                                               // Gets the right of the gravity (gravity direction when rotated 90 degress)
        float lateralSpeed = Vector2.Dot(rb.linearVelocity, lateral);                                                 // Gets the velocity along an axis (left or right)

        bool isWalkingRight = lateralSpeed > walkThreshold;                                     // Checks if the player was last walking right
        bool isWalkingLeft = lateralSpeed < -walkThreshold;                                     // Checks if the player was last walking left
        bool isGrounded = userInput != null ? userInput.CheckIsGrounded() : true;               // Checks if the user is grounded or in the air

        if (isWalkingRight) lastFacing = 1;                                                     // Sets the index to leave the player facing in the right direction
        if (isWalkingLeft) lastFacing = -1;                                                     // Sets the index to leave the player facing in the left direction
        spriteRender.flipX = lastFacing < 0;


        animator.SetBool("isJumping", !isGrounded);                                             // Starts the jumping animation when the player is in the air
        if (!isGrounded)
        {
            animator.SetBool("isWalkingRight", false);
            animator.SetBool("isWalkingLeft", false);
        }
        else
        {
            animator.SetBool("isWalkingRight", isWalkingRight);                                 // Sets the animation for the player walking right
            animator.SetBool("isWalkingLeft", isWalkingLeft);                                   // Sets the animation for the player walking left
            PlayerAttack();                                                                     // Has the player attack in the direction they are facing
        }
    }

    private void PlayerAttack()
    {
        // Starts the player attacking animation
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time >= lastAtkTime + attackCooldown)
        {
            lastAtkTime = Time.time;

            animator.SetTrigger("isAttacking");
        }
    }
}
