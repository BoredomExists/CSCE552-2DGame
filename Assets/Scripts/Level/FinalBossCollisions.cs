using UnityEngine;

/// <summary>
/// Manager for the Final Boss Collisions
/// </summary>
public class FinalBossCollisions : MonoBehaviour
{
    [Header("References")]
    public HealthSystem ps;                                                 // Health System of the Player

    void Start()
    {
        ps = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthSystem>();       // Gets the HealthSystem of the Player
    }

    // Checks if Player physically collides with hitboxes causing damage
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Player")) return;

        if (collision.CompareTag("Player"))
        {
            ps.TakeDamage(30);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.collider.CompareTag("Player")) return;

        if (collision.collider.CompareTag("Player"))
        {
            ps.TakeDamage(30);
        }
    }
}
