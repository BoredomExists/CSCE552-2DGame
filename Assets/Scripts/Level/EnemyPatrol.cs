using UnityEngine;

/// <summary>
/// Patrol Script for basic enemies that move back and forth (Basic Melee Enemy and Extending Laser Enemy)
/// </summary>
public class EnemyPatrol : MonoBehaviour
{
    [Header("References")]
    public EnemyAudioController enemyAudio;                                 // Audio Controller for the Enemy
    [Header("Enemy Settings")]
    public float speed = 5f;                                                // Move speed of the enemy             
    public LayerMask groundLayer;                                           // Checks objects to see if their layer is "Ground"
    public Transform groundCheck;                                           // Check to make sure the enemy is grounded
    public float edgeCheck = 0.3f;                                          // Length forward to check if there ground stops "existing"
    public float wallCheck = 0.1f;                                          // Checks if the enemy has ran into a wall
    public bool facingRight = true;                                         // Starts the enemy facing right

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();                                           // Gets the rigidbody of the enemy
        enemyAudio = GetComponent<EnemyAudioController>();                          // Gets the Audio Controller
        rb.freezeRotation = true;                                                   // Freezes the rotation so the enemy cannot be knocked over
    }

    void FixedUpdate()
    {
        Vector2 fwd = (Vector2)transform.right * (facingRight ? 1 : -1);            // Gets the forward direction based on which way the enemy is facing

        Vector2 probe = groundCheck ? (Vector2)groundCheck.position : (Vector2)transform.position;      // Checks to see if there is still a ground below them

        bool groundAhead = Physics2D.Raycast(probe, -transform.up, edgeCheck, groundLayer);             // Checks to see if there is still ground "existing"
        bool wallAhead = Physics2D.Raycast(probe, fwd, wallCheck, groundLayer); ;                       // Checks to see if there is a wall ahead

        // Swaps the enemies direction when needing to turn from either hitting a wall or when the ground is no more
        if (!groundAhead || wallAhead)
        {
            facingRight = !facingRight;

            var s = transform.localScale;
            s.x *= -1f;
            transform.localScale = s;

            fwd = -fwd;
        }

        rb.linearVelocity = fwd.normalized * speed;                                                    // Moves the enemy
        enemyAudio.PlayMoving();
    }
}

