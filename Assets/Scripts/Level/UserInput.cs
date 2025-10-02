using System.ComponentModel.Design;
using UnityEngine;

public class UnserInput : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;            // Walking Speed
    public float sprintSpeed = 12f;         // Sprinting Speed
    public float jumpForce = 10f;           // Jumping Force

    [Header("Ground Check")]
    public Transform groundCheck;           // Position of ground check game object
    public float groundCheckRadius = 0.2f;  // Radius of ground check
    public LayerMask ground;                // Checks what is considered ground
    public bool isGrounded;                 // Check if the player is on the ground

    [Header("Air Movement Settings")]
    public float airSpeed = 20f;            // How much speed to apply to the player in the air for direcitonal movement
    public float fastFallSpeed = 20f;       // Fast Fall Speed

    [Header("Camera Settings")]
    public Transform mainCamera;            // Game Object for the Main Camera
    public Transform player;                // Game Object for the Player
    public float rotationSpeed = 10f;       // The rotation speed on how fast to rotate the camera/player

    private Rigidbody2D rb;

    private Vector2 moveVector;

    private float zRotation = 0f;
    private float lastGroundSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();       // Initialize RigidBody Component
        lastGroundSpeed = moveSpeed;            // Set the ground speed to reset move speed
    }

    void Update()
    {
        isGrounded = CheckIsGrounded();         // Check if player is on the ground
        float moveX = 0f;
        if (Input.GetKey(KeyCode.A)) moveX = -1f;   // Move Left
        if (Input.GetKey(KeyCode.D)) moveX = 1f;    // Move Right
        moveVector = new Vector2(moveX, 0f);        // Create move vector to know which way to move the player

        // Implements Jumping and Fast Falling
        if (Input.GetButtonDown("Jump") && isGrounded)
            rb.AddForce(-Physics2D.gravity.normalized * jumpForce, ForceMode2D.Impulse);

        if (Input.GetKeyDown(KeyCode.S) && !isGrounded)
            rb.linearVelocity += Physics2D.gravity.normalized * fastFallSpeed;
                
        lastGroundSpeed = (Input.GetKey(KeyCode.LeftShift) && isGrounded) ? sprintSpeed : moveSpeed;        // Sets the lastGroundSpeed to help with jumping momentum

        ChangeRotation();       // Changes player and camera rotation

        Quaternion targetRotation = Quaternion.Euler(0f, 0f, zRotation);        // Sets the rotation value to rotate to

        // Rotates Player and Camera
        if (mainCamera != null)
            mainCamera.rotation = Quaternion.Lerp(mainCamera.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        if (player != null)
            player.rotation = Quaternion.Lerp(player.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        isGrounded = CheckIsGrounded();     // Check if player is on the ground

        Vector2 lateralAxis = (Quaternion.Euler(0f, 0f, zRotation) * Vector2.right).normalized;     // Determine movement axis based on the current zRotation

        float currentLateral = Vector2.Dot(rb.linearVelocity, lateralAxis);     //  Gets current velocity projected along the lateral axis

        // Desired targets for ground vs air movement
        float groundTarget = moveVector.x * lastGroundSpeed;
        float airTarget = moveVector.x * lastGroundSpeed;

        float finalLateral;

        // Snap to ground target immediately when grounded
        if (isGrounded)
            finalLateral = groundTarget;
        else
            finalLateral = Mathf.MoveTowards(currentLateral, airTarget, airSpeed * Time.fixedDeltaTime);        // Smoothing the acceleration towards the air target when airborne

        Vector2 lateralVelocity = lateralAxis * finalLateral;       // Builds velocity along the lateral axis

        Vector2 gravityVelocity = Project(rb.linearVelocity, Physics2D.gravity.normalized);     // Preserve velocity component along ravity direction

        rb.linearVelocity = lateralVelocity + gravityVelocity;      // Moves the players
    }

    // Projects Vector a onto Vector b in a 2D format
    private Vector2 Project(Vector2 a, Vector2 b)
    {
        if (b.sqrMagnitude < 1e-6f) return Vector2.zero;
        return (Vector2.Dot(a, b) / b.sqrMagnitude) * b;
    }

    // Handles input for rotating camera and player
    private void ChangeRotation()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            zRotation += 90f;
            ChangeGravity();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            zRotation -= 90f;
            ChangeGravity();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            zRotation += 180;
            ChangeGravity();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            zRotation -= 180;
            ChangeGravity();
        }
    }

    // Creates the new gravity when the rotation is changed
    private void ChangeGravity()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        // Stores the old gravity before rotation
        Vector2 oldGravity = Physics2D.gravity.sqrMagnitude < 1e-6f ? Vector2.down : Physics2D.gravity.normalized;
       
        // Calculates the new gravity from the rotation
        Vector2 newGravity = Quaternion.Euler(0f, 0f, zRotation) * Vector2.down * 9.81f;
        Physics2D.gravity = newGravity;
        
        // Angle difference between the old and new gravity vectors
        float deltaAngle = Vector2.SignedAngle(oldGravity, newGravity.normalized);

        // Rotate current velocity by deltaAngle so momentum is preserved
        rb.linearVelocity = RotateVector(rb.linearVelocity, deltaAngle);
    }

    // Rotates a vector by X degrees in 2D
    private Vector2 RotateVector(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float c = Mathf.Cos(rad);
        float s = Mathf.Sin(rad);
        return new Vector2(v.x * c - v.y * s, v.x * s + v.y * c);
    }
    
    // Check when the player is grounded or not
    private bool CheckIsGrounded()
    {
        if (groundCheck == null) return false;

        Vector2 gravityDirection = Physics2D.gravity.normalized;    // Gets the direction of down
        float castDistance = groundCheckRadius + 0.05f;             // Radius + a buffer

        RaycastHit2D hit = Physics2D.CircleCast(groundCheck.position, groundCheckRadius, gravityDirection, castDistance, ground);
        if (hit.collider != null)
        {
            float dot = Vector2.Dot(hit.normal, -gravityDirection); // Check the normal surface vs the gravity
            if (dot > 0.7f) return true;                            // Checks the ground being mostly flat relative to gravity
        }
        return false;
    }
}