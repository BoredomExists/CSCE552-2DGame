using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 10f;
    public float lifeTime = 2f;
    public HealthSystem playerHS;

    private Rigidbody2D rb;
    private int damage;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.freezeRotation = true;
    }

    public void Fire(Vector2 direction, int dmg)
    {
        if (direction.sqrMagnitude < 0.0001f) direction = Vector2.right;
        direction.Normalize();

        rb.linearVelocity = direction * speed;
        transform.right = direction;

        damage = dmg;
        Invoke(nameof(DestroySelf), lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger) return;

        if (collision.CompareTag("Player"))
        {
            playerHS = collision.GetComponent<HealthSystem>();
            playerHS.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
