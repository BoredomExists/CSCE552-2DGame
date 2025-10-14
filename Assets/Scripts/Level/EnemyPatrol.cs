using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f;
    public bool startFacingRight = true;

    [Header("Detection References")]
    public LayerMask groundLayer;
    public float lookAhead = 0.2f;
    public float edgeCheck = 0.3f;
    public float wallCheck = 0.1f;

    private Rigidbody2D rb;
    private BoxCollider2D col;
    private int direction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.useFullKinematicContacts = true;
        direction = startFacingRight ? 1 : -1;
    }

    void FixedUpdate()
    {
        Bounds b = col.bounds;
        Vector2 center = b.center;
        float halfW = b.extents.x;

        Vector2 frontFoot = new Vector2(center.x + direction * (halfW + lookAhead), b.min.y + 0.02f);
        bool groundAhead = Physics2D.Raycast(frontFoot, Vector2.down, edgeCheck, groundLayer).collider != null;

        bool wallAhead = Physics2D.Raycast(center, new Vector2(direction, 0f), halfW + wallCheck, groundLayer).collider != null;

        if (!groundAhead || wallAhead)
            direction *= -1;

        Vector2 move = rb.position + new Vector2(direction * speed, 0f) * Time.fixedDeltaTime;
        rb.MovePosition(move);
    }
}
