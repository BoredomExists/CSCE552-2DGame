using UnityEngine;

/// <summary>
/// Sets the gravity of the enemy based on the rotation they are set at
/// </summary>
public class EnemyGravity : MonoBehaviour
{
    // Applies the gravity based on the enemies downward direction
    [Header("Gravity Settings")]
    public float gravityStrength = 9.81f;                               // Gravity Strength to apply when done changing the direction of the enemy gravity based on rotation

    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();                                      // Gets the Rigidbody of the Enemy
        rb.gravityScale = 0f;                                                   // Sets the gravity scale to 0
        rb.freezeRotation = true;                                               // Freezes rotation so the enemy is not on their "side" if rotated by accident
    }

    void FixedUpdate()
    {
        Vector2 localDown = -transform.up;                                      // Gets the way their lower half is facing"

        rb.AddForce(localDown * gravityStrength * rb.mass, ForceMode2D.Force);  // Applies the gravitational force
    }
}
