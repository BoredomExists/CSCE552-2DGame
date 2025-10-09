using System;
using Unity.Collections;
using Unity.VisualScripting;
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

    [Header("Camera Settings")]
    public Transform mainCamera;
    public Transform player;

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

        lastGroundSpeed = (Input.GetKey(KeyCode.LeftShift) && isGrounded) ? sprintSpeed : moveSpeed; 
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
}
