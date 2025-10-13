using UnityEngine;

public class EnemyGravity : MonoBehaviour
{
    // Applies the gravity based on the enemies downward direction
    [Header("Gravity Settings")]
    public float gravityStrength = 9.81f;

    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        Vector2 localDown = -transform.up;                                      // Gets the way their lower half is facing"

        rb.AddForce(localDown * gravityStrength * rb.mass, ForceMode2D.Force);  // Applies the gravitational force
    }
}
