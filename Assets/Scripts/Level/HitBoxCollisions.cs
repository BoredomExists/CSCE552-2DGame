using System.Collections;
using UnityEngine;

/// <summary>
/// Hit Box Collision Checker for melee attacks
/// Player - Sword Attack
/// Enemy - Bump into the player
/// </summary>
public class HitBoxCollisions : MonoBehaviour
{
    [Header("References")]
    public PlayerAudioController playerAudio;                       // Gets the Audio Controller from the player
    public EnemyAudioController enemyAudio;                         // Gets the Audio Controller from an enemy
    public UserInput userInput;                                     // Gets the user input to get the damage the user does

    void Start()
    {
        userInput = GameObject.FindWithTag("Player").GetComponent<UserInput>();
        playerAudio = GameObject.FindWithTag("Player").GetComponent<PlayerAudioController>();
        enemyAudio = GameObject.FindFirstObjectByType<EnemyAudioController>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Cover")) return;                         // Makes sure nothing else triggers this event

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
                playerAudio.PlayPlayerHit();
                playerHS.TakeDamage(10);
                StartCoroutine(TempDisableHitBox(gameObject));
            }
        }

        if (collision.collider.CompareTag("Barrel"))
        {
            HealthSystem playerHS = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthSystem>();
            playerHS.Heal(30);
            Destroy(collision.gameObject);
        }
    }

    IEnumerator TempDisableHitBox(GameObject gameObject)
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(2f);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
}
