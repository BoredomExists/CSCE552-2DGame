using UnityEngine;

public class HitBoxCollisions : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Cover")) return;

        if (collision.collider.CompareTag("Enemy"))
        {
            HealthSystem enemyHS = collision.collider.GetComponent<HealthSystem>();
            if (enemyHS != null)
            {
                enemyHS.TakeDamage(50);
            }
        }

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
