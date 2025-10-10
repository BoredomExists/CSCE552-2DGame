using System;
using Unity.Collections;
using UnityEngine;

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

    private Rigidbody2D rb;
    private Vector2 moveVector;
    private float lastGroundSpeed;

    private float zRotation = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastGroundSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = CheckIsGrounded();
        float moveX = 0f;
        if (Input.GetKey(KeyCode.A)) moveX = -1f;
        if (Input.GetKey(KeyCode.D)) moveX = 1f;

        moveVector = new Vector2(moveX, 0f);

        if (Input.GetButtonDown("Jump") && isGrounded)
            rb.AddForce(-Physics2D.gravity.normalized * jumpForce, ForceMode2D.Impulse);

        if (Input.GetKeyDown(KeyCode.S) && !isGrounded)
            rb.linearVelocity += Physics2D.gravity.normalized * fastFallSpeed;

        lastGroundSpeed = (Input.GetKey(KeyCode.LeftShift) && isGrounded) ? sprintSpeed : moveSpeed;

        ChangeRotation();

        Quaternion rotationToTurnTo = Quaternion.Euler(0f, 0f, zRotation);

        if (mainCamera != null)
            mainCamera.rotation = Quaternion.Lerp(mainCamera.rotation, rotationToTurnTo, rotationSpeed * Time.deltaTime);
        if (player != null)
            player.rotation = Quaternion.Lerp(player.rotation, rotationToTurnTo, rotationSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        isGrounded = CheckIsGrounded();

        Vector2 lateralAxis = (Quaternion.Euler(0f, 0f, zRotation) * Vector2.right).normalized;
        float currentLateral = Vector2.Dot(rb.linearVelocity, lateralAxis);

        float groundTarget = moveVector.x * lastGroundSpeed;
        float airTarget = moveVector.x * lastGroundSpeed;
        float finalLateral;

        if (isGrounded)
            finalLateral = groundTarget;
        else
            finalLateral = Mathf.MoveTowards(currentLateral, airTarget, airSpeed * Time.fixedDeltaTime);

        Vector2 lateralVelocity = lateralAxis * finalLateral;
        Vector2 gravityVelocity = Vector2Project(rb.linearVelocity, Physics2D.gravity.normalized);

        rb.linearVelocity = lateralVelocity + gravityVelocity;
    }

    private Vector2 Vector2Project(Vector2 a, Vector2 b)
    {
        if (b.sqrMagnitude < 1e-6f) return Vector2.zero;
        return (Vector2.Dot(a, b) / b.sqrMagnitude) * b;
    }

    private bool CheckIsGrounded()
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
}
