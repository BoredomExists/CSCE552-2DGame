using UnityEngine;

/// <summary>
/// Manager for the jumping mini-boss enemy
/// </summary>
public class JumpEnemyPatrol : MonoBehaviour
{
    [Header("References")]
    public Transform player;                            // The player to move towards


    [Header("Jump Settings")]
    public float jumpForce = 10f;                      // How strong of a jump
    public float lateralStrength = 1f;                  // How strong to jump towards the player
    public float upwardStrength = 1f;                   // How strong to jump upwards with jumpForce
    public float aggroRange = 10f;                      // How far can the enemy see the player
    public float jumpCooldown = 3f;                     // How long till the enemy can jump again

    public LayerMask groundMask;                        // Check to see if the enemy is grounded to jump again

    private Rigidbody2D rb;
    private float nextJump;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();                               // Gets the rigidbody of the enemy
        player = GameObject.FindWithTag("Player").transform;            // Gets the player transform
    }

    void FixedUpdate()
    {
        if (Vector2.Distance(player.position, transform.position) > aggroRange) return;     // If the player is out of range, do noting
        bool isGrounded = Physics2D.OverlapCircle(transform.position, 1f, groundMask);      // Check to see if enemy is grounded
        if (!isGrounded || Time.time < nextJump) return;                                    // If is grounded or "jump cooldown" is not refreshed, do nothing

        Vector2 toPlayer = (Vector2)(player.position - transform.position);                                 // Gets the distance between the player and enemy
        Vector2 tangent = new Vector2(-Physics2D.gravity.normalized.y, Physics2D.gravity.normalized.x);     // Gets the tangent vector relative to the gravity direction
        Vector2 lateral = Vector2.Dot(toPlayer, tangent) * tangent;                                         // Gets the dot product of the toPlayer vector and the tanget to get the lateral
        if (lateral.sqrMagnitude > 0.001f) lateral = lateral.normalized * lateralStrength;                  // If the lateral is meaningful, normalize it to scale by lateralStrength, otherwise set to 0
        else lateral = Vector2.zero;

        Vector2 up = -Physics2D.gravity.normalized * upwardStrength;                        // Gets the up direction of gravity and scale it by the upward strength
        Vector2 jumpDirection = lateral + up;                                               // Gets the direction the enemy jumps to

        rb.linearVelocity = Vector2.zero;                                                   // Sets the velocity to zero to prepare for jump

        rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);                        // Have the enemy jump
        nextJump = Time.time + jumpCooldown;                                                // Sets the jump cooldown
    }
}

