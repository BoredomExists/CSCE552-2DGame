using UnityEngine;

/// <summary>
/// Manages the Animations of the player
/// </summary>
public class PlayerAnimator : MonoBehaviour
{
    [Header("References")]
    public PlayerAudioController playerAudio;           // Gets the Player Audio Controller Script 
    // Plays all the animations for the player
    [Header("Graphics")]
    public Transform spriteGO;                      // Player Sprite Visual Game Object
    public SpriteRenderer spriteRender;             // Sprite Renderer of player sprite visual game object
    public Sprite projSprite;                       // Sprite to change into when player switches to gauntlet

    [Header("Animation")]
    public Animator animator;                       // Animator of the sprite
    public float walkThreshold = 0.1f;              // Threshold to determine if player is moving

    [Header("Attack Settings")]
    public float attackCooldown = 0.5f;            // How quickly the player can attack
    public Transform projStart;                    // Starting Position where the player's projectile will be created at

    public UserInput userInput;                     // User Input Script of the player

    private Rigidbody2D rb;
    private int lastFacing = 1;                     // Determine which way the player will face
    private float lastAtkTime = -999f;              // Max Float value to help with attackCooldown time

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerAudio = GetComponent<PlayerAudioController>();    // Gets the audio controller
        spriteRender = spriteGO.GetComponent<SpriteRenderer>();

        if (userInput != null && projStart == null)
            projStart = userInput.projPOS;
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
        if (isWalkingLeft) lastFacing = -1;                                                   // Sets the index to leave the player facing in the left direction

        CapsuleCollider2D col = gameObject.GetComponent<CapsuleCollider2D>();
        col.offset = lastFacing < 0 ? new Vector2(-0.03f, -0.05f) : new Vector2(0.03f, -0.05f);
        projStart.localPosition = lastFacing < 0 ? new Vector2(-.14f, .1f) : new Vector2(.14f, .1f);

        //animator.SetBool("isJumping", !isGrounded);                                            // Starts the jumping animation when the player is in the air
        animator.SetInteger("lastFacing", lastFacing);
        if (isGrounded)
        {
            animator.SetBool("isWalkingRight", isWalkingRight);                                 // Sets the animation for the player walking right
            animator.SetBool("isWalkingLeft", isWalkingLeft);                                   // Sets the animation for the player walking left
        }
        else
        {
            animator.SetBool("isWalkingRight", false);
            animator.SetBool("isWalkingLeft", false);
        }
        PlayerAttack();
    }


    // Function to either attack with the sword or fire a projectile
    private void PlayerAttack()
    {
        if (userInput.GetSwordUser()) animator.SetBool("isGauntlet", false);                // Checks to see if player is in sword mode or gauntlet mode
        else animator.SetBool("isGauntlet", true);
        // Starts the player attacking animation
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time >= lastAtkTime + attackCooldown)
        {
            lastAtkTime = Time.time;        // Resets the player attack cooldown

            // If in sword mode, play sword animations
            if (userInput.GetSwordUser())
            {
                playerAudio.PlaySwordAttack();
                if (lastFacing > 0)
                    animator.SetTrigger("isAttacking");
                else
                    animator.SetTrigger("isAttackingLeft");
            }
            else
            {
                // Otherwise fire the gauntlet
                userInput.ShootGauntlet();
            }
        }
    }

    // Get the direction the player was last facing
    public int GetLastFacing()
    {
        return lastFacing;
    }
}
