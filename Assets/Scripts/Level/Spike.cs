using UnityEngine;

/// <summary>
/// Manages the collision interaction of the spike object with the player
/// </summary>
public class Spike : MonoBehaviour
{
    [Header("References")]
    public HealthSystem playerHS;               // Player Health System

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerHS = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthSystem>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        if (collision.collider.CompareTag("Player"))
        {
            playerHS.TakeDamage(5);
        }
    }
}
