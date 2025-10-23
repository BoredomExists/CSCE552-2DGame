using UnityEngine;

/// <summary>
/// Hit Box Collision Checker for melee attacks
/// Player - Sword Attack
/// Enemy - Bump into the player
/// </summary>
public class HitBoxCollisions : MonoBehaviour
{
    [Header("References")]
    public UserInput userInput;                                     // Gets the user input to get the damage the user does

    void Start()
    {
        userInput = GameObject.FindWithTag("Player").GetComponent<UserInput>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Cover")) return;                         // Makes sure nothing else triggers this even

        // If the player collides with the enemy, the enemy takes damage
        if (collision.collider.CompareTag("Enemy"))
        {
            HealthSystem enemyHS = collision.collider.GetComponent<HealthSystem>();
            if (enemyHS != null)
            {
                enemyHS.TakeDamage(userInput.GetDamage());
            }
        }

        // If the enemy collides with the player, the player takes damage
        if (collision.collider.CompareTag("Player"))
        {
            HealthSystem playerHS = collision.collider.GetComponent<HealthSystem>();

            if (playerHS != null)
            {
                playerHS.TakeDamage(20);
            }
        }
    }
}
