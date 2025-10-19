using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float speed = 5f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float edgeCheck = 0.3f;
    public float wallCheck = 0.1f;
    public bool facingRight = true;

    private Rigidbody2D rb;
    private RaycastHit2D hit;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        Vector2 fwd = (Vector2)transform.right * (facingRight ? 1 : -1);

        Vector2 probe = groundCheck ? (Vector2)groundCheck.position : (Vector2)transform.position;

        bool groundAhead = Physics2D.Raycast(probe, -transform.up, edgeCheck, groundLayer);
        bool wallAhead = Physics2D.Raycast(probe, fwd, wallCheck, groundLayer);

        if (!groundAhead || wallAhead)
        {
            facingRight = !facingRight;

            var s = transform.localScale;
            s.x *= -1f;
            transform.localScale = s;

            fwd = -fwd;
        }

        rb.linearVelocity = fwd.normalized * speed;
    }
}

