using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 10f;
    public float lifeTime = 5f;
    public HealthSystem playerHS;
    public HealthSystem enemyHS;

    public enum ProjectileOwner { Player, Enemy };

    private Rigidbody2D rb;
    private int damage;
    public ProjectileOwner owner = ProjectileOwner.Player;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.freezeRotation = true;
    }

    public void Fire(Vector2 direction, int dmg, ProjectileOwner ownerType = ProjectileOwner.Player)
    {
        if (direction.sqrMagnitude < 0.0001f) direction = Vector2.right;
        direction.Normalize();

        rb.linearVelocity = direction * speed;
        transform.right = direction;

        owner = ownerType;
        damage = dmg;
        Invoke(nameof(DestroySelf), lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger) return;
        if (!collision.CompareTag("Enemy") || !collision.CompareTag("Player")) Destroy(gameObject);

        if (owner == ProjectileOwner.Player)
        {
            if (collision.CompareTag("Enemy"))
            {
                Debug.Log("Enemy Contact");
                enemyHS = collision.GetComponent<HealthSystem>();
                enemyHS.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        else
        {
            if (collision.CompareTag("Player"))
            {
                playerHS = collision.GetComponent<HealthSystem>();
                playerHS.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }

    public void SetProjectileOwner(ProjectileOwner newOwner)
    {
        owner = newOwner;
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
