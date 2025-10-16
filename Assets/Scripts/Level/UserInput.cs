using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class UserInput : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 12f;
    public float jumpForce = 10f;

    [Header("Ground Settings")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask ground;
    public bool isGrounded;

    [Header("Air Settings")]
    public float airSpeed = 20f;
    public float fastFallSpeed = 20f;

    [Header("Camera Settings")]
    public Transform mainCamera;
    public Transform player;
    public float rotationSpeed = 10f;

    [Header("Player UI Settings")]
    public Image gravityIcon;
    public Sprite gravityUp;
    public Sprite gravityDown;
    public Sprite gravityLeft;
    public Sprite gravityRight;


    private Rigidbody2D rb;
    private Vector2 moveVector;
    private float lastGroundSpeed;

    private float zRotation = 0f;

    private CinemachineCamera ccam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastGroundSpeed = moveSpeed;

        ccam = FindFirstObjectByType<CinemachineCamera>();
        if (ccam != null)
            mainCamera = ccam.transform;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = CheckIsGrounded();
        float moveX = 0f;
        if (Input.GetKey(KeyCode.A)) moveX = -1f;                                                                   // Check if the player is moving left
        if (Input.GetKey(KeyCode.D)) moveX = 1f;                                                                    // Check if the player is moving right        

        moveVector = new Vector2(moveX, 0f);                                                                        // Set the vector in which direction the player is moving

        // Jump Function
        if (Input.GetButtonDown("Jump") && isGrounded)
            rb.AddForce(-Physics2D.gravity.normalized * jumpForce, ForceMode2D.Impulse);

        // Fast Fall Function
        if (Input.GetKeyDown(KeyCode.S) && !isGrounded)
            rb.linearVelocity += Physics2D.gravity.normalized * fastFallSpeed;

        lastGroundSpeed = (Input.GetKey(KeyCode.LeftShift) && isGrounded) ? sprintSpeed : moveSpeed;               // Determines if the player is sprinting or not

        ChangeRotation();

        Quaternion rotationToTurnTo = Quaternion.Euler(0f, 0f, zRotation);

        // Changes the camera and player rotation
        if (mainCamera != null)
            if (!RoomCameraTrigger.roomEntered)
                mainCamera.rotation = Quaternion.Lerp(mainCamera.rotation, rotationToTurnTo, rotationSpeed * Time.deltaTime);
            else
                mainCamera.rotation = Quaternion.identity;
        if (player != null)
                player.rotation = Quaternion.Lerp(player.rotation, rotationToTurnTo, rotationSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        isGrounded = CheckIsGrounded();

        Vector2 lateralAxis = (Quaternion.Euler(0f, 0f, zRotation) * Vector2.right).normalized;         // Gets the current lateral axis representing the player moving left or right in the rotation frame
        float currentLateral = Vector2.Dot(rb.linearVelocity, lateralAxis);                             // Projects the current velocity onto the lateral to get the signed lateral speed

        float groundTarget = moveVector.x * lastGroundSpeed;                                            // Desired lateral speeds for on the ground
        float airTarget = moveVector.x * lastGroundSpeed;                                               // or in the air
        float finalLateral;

        if (isGrounded)
            finalLateral = groundTarget;
        else
            finalLateral = Mathf.MoveTowards(currentLateral, airTarget, airSpeed * Time.fixedDeltaTime);

        Vector2 lateralVelocity = lateralAxis * finalLateral;                                          // Reconstruct the new velocity
        Vector2 gravityVelocity = Vector2Project(rb.linearVelocity, Physics2D.gravity.normalized);     // Keep only the component of current velocity along gravity

        rb.linearVelocity = lateralVelocity + gravityVelocity;
    }

    private Vector2 Vector2Project(Vector2 a, Vector2 b)
    {
        if (b.sqrMagnitude < 1e-6f) return Vector2.zero;
        return (Vector2.Dot(a, b) / b.sqrMagnitude) * b;
    }

    // Check if the player is grounded
    public bool CheckIsGrounded()
    {
        if (groundCheck == null) return false;

        Vector2 gravityDirection = Physics2D.gravity.normalized;
        float castDistance = groundCheckRadius + 0.05f;

        RaycastHit2D hit = Physics2D.CircleCast(groundCheck.position, groundCheckRadius, gravityDirection, castDistance, ground);
        if (hit.collider != null)
        {
            float dot = Vector2.Dot(hit.normal, -gravityDirection);
            if (dot > 0.7f) return true;
        }
        return false;
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
            zRotation += 180f;
            ChangeGravity();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            zRotation -= 180f;
            ChangeGravity();
        }
        ChangeGravityIcon();
    }

    // Creates the new gravity when the rotation is changed
    private void ChangeGravity()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        Vector2 oldGravity = Physics2D.gravity.sqrMagnitude < 1e-6f ? Vector2.down : Physics2D.gravity.normalized;
        Vector2 newGravity = Quaternion.Euler(0f, 0f, zRotation) * Vector2.down * 9.81f;
        Physics2D.gravity = newGravity;

        float deltaAngle = Vector2.SignedAngle(oldGravity, newGravity.normalized);
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

    public void ChangeGravityIcon()
    {
        float z = Mathf.DeltaAngle(0f, player.eulerAngles.z);
        int snapped = Mathf.RoundToInt(z / 90f) * 90;
        if (snapped == -180) snapped = 180;
        switch (snapped)
        {
            case 0:
                gravityIcon.sprite = gravityDown;
                break;

            case 90:
                gravityIcon.sprite = gravityRight;
                break;

            case 180:
                gravityIcon.sprite = gravityUp;
                break;

            case -90:
                gravityIcon.sprite = gravityLeft;
                break;
        }
    }
}
