using UnityEngine;

public class JumpEnemyPatrol : MonoBehaviour
{
    [Header("References")]
    public Transform player;


    [Header("Jump Settings")]
    public float jumpForce = 10f;
    public float lateralStrength = 1f;
    public float upwardStrength = 1f;
    public float aggroRange = 10f;
    public float jumpCooldown = 5f;

    public LayerMask groundMask;

    private Rigidbody2D rb;
    private float nextJump;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        if (Vector2.Distance(player.position, transform.position) > aggroRange) return;
        bool isGrounded = Physics2D.OverlapCircle(transform.position, 1f, groundMask);
        if (!isGrounded || Time.time < nextJump) return;

        Vector2 toPlayer = (Vector2)(player.position - transform.position);
        Vector2 tangent = new Vector2(-Physics2D.gravity.normalized.y, Physics2D.gravity.normalized.x);
        Vector2 lateral = Vector2.Dot(toPlayer, tangent) * tangent;
        if (lateral.sqrMagnitude > 0.001f) lateral = lateral.normalized * lateralStrength;
        else lateral = Vector2.zero;

        Vector2 up = -Physics2D.gravity.normalized * upwardStrength;
        Vector2 jumpDirection = lateral + up;

        rb.linearVelocity = Vector2.zero;

        rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
        nextJump = Time.time + jumpCooldown;
    }
}

