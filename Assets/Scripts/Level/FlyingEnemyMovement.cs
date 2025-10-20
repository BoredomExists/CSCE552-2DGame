using UnityEngine;

public class FlyingEnemyMovement : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float speed = 5f;
    public float rotateSpeed = 2f;
    public LayerMask groundLayer;
    public bool movingDown = true;

    private bool isRotating = false;
    private Quaternion rotateTo;

    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 down = -transform.up;
        Vector2 moveY = down * (movingDown ? 1 : -1);

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

        bool wallAhead = Physics2D.Raycast(transform.position, down, 5f, groundLayer);
        if (wallAhead)
        {
            rb.linearVelocity = Vector2.zero;
            rotateTo = transform.rotation * Quaternion.Euler(0f, 0f, 90f);
            isRotating = true;

            return;
        }
        rb.linearVelocity = moveY.normalized * speed;
    }
}
