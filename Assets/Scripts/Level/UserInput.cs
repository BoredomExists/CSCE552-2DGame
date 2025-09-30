using UnityEngine;

public class UserInput : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask ground;
    public bool grounded;

    [Header("Camera Settings")]
    public Transform mainCamera;
    public Transform player;


    private Rigidbody2D rb;
    private Vector2 moveVector;

    private float zRotation = 0f;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = 0f;
        if (Input.GetKey(KeyCode.A)) moveX = -1f;
        if (Input.GetKey(KeyCode.D)) moveX = 1f;

        moveVector = new Vector2(moveX, 0f);

        if (Input.GetButtonDown("Jump") && grounded)
        {
            rb.AddForce(-Physics2D.gravity.normalized * jumpForce, ForceMode2D.Impulse);
        }
        
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            zRotation += 90f;
            ChangeGravity();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            zRotation -= 90f;
            ChangeGravity();
        }
        mainCamera.rotation = Quaternion.Euler(0f, 0f, zRotation);
        player.rotation = Quaternion.Euler(0f, 0f, zRotation);
    }

    void FixedUpdate()
    {
        Vector2 moveDir = player.right * moveVector.x * moveSpeed;

        Vector2 gravityVelocity = Project(rb.linearVelocity, Physics2D.gravity.normalized);

        rb.linearVelocity = moveDir + gravityVelocity;

        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, ground);
    }
    
    private Vector2 Project(Vector2 a, Vector2 b)
    {
        return (Vector2.Dot(a, b) * b.sqrMagnitude) * b;
    }
    
    private void ChangeGravity()
    {
        Vector2 newGravity = Quaternion.Euler(0f, 0f, zRotation) * Vector2.down * 9.81f;
        Physics2D.gravity = newGravity;
    }
}
