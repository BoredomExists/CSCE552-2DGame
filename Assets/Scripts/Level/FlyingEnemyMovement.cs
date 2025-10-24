using UnityEngine;

/// <summary>
/// Manages the flying mini-boss enemy movement
/// </summary>
public class FlyingEnemyMovement : MonoBehaviour
{
    [Header("References")]
    public EnemyAudioController enemyAudio;                                 // Audio Controller for the Enemy
    [Header("Enemy Settings")]
    public float speed = 5f;                                            // Move speed for enemy
    public float rotateSpeed = 2f;                                      // Rotate speed when enemy hit a wall
    public LayerMask groundLayer;                                       // Check to see if a wall was hit (All borders of the level are layer=Ground)
    public bool movingDown = true;                                      // Direction in which the enemy will move

    private bool isRotating = false;                                    // Check to see if the enemy is rotation
    private Quaternion rotateTo;                                        // Check which Quaternion values to rotate towards

    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();                               // Gets rigidbody component
        GameObject.FindFirstObjectByType<EnemyAudioController>();       // Gets the Audio Controller from EnemyScriptGetting Object
    }

    void FixedUpdate()
    {
        Vector2 down = -transform.up;                                  // Gets the down direction of the enemy
        Vector2 moveY = down * (movingDown ? 1 : -1);                  // Velocity to move the enemy towards

        // If the enemy is rotating, stop movement and rotate 90 degrees and move again
        if (isRotating)
        {
            rb.linearVelocity = Vector2.zero;
            transform.rotation = Quaternion.Lerp(transform.rotation, rotateTo, rotateSpeed * Time.fixedDeltaTime);

            if (Quaternion.Angle(transform.rotation, rotateTo) < 0.5f)
            {
                transform.rotation = rotateTo;
                isRotating = false;
            }
            return;
        }

        // Check to see if a wall is ahead and sets values for the isRotation statement to rotate the enemy
        bool wallAhead = Physics2D.Raycast(transform.position, down, 5f, groundLayer);
        if (wallAhead)
        {
            rb.linearVelocity = Vector2.zero;
            rotateTo = transform.rotation * Quaternion.Euler(0f, 0f, 90f);
            isRotating = true;

            return;
        }
        rb.linearVelocity = moveY.normalized * speed;                       // Moves the enemy
        // enemyAudio.PlayMoving(); Not sure which audio to put here Idle or moving
        // enemyAudio.PlayEnemyIdle();
    }
}
