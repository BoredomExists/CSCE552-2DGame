using UnityEngine;

/// <summary>
/// Manager for the projectile objects
/// </summary>
public class Projectile : MonoBehaviour
{
    [Header("References")]
    public PlayerAudioController playerAudio;       // Audio Controller for the Player
    public EnemyAudioController enemyAudio;         // Audio Controller for the Enemy

    [Header("Projectile Settings")]
    public float speed = 10f;                       // Speed the projectile moves at
    public float lifeTime = 5f;                     // How long until the projectile gets destroyed if no collisions
    public HealthSystem playerHS;                   // Player Health System
    public HealthSystem enemyHS;                    // Enemy Health System

    public UserInput userInput;                     // Player's User INput

    public enum ProjectileOwner { Player, Enemy };  // Enum to determine owner of the projectile (Who shot it)

    private Rigidbody2D rb;
    private int damage;
    public ProjectileOwner owner = ProjectileOwner.Player;      // Default to player
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();                                           // Gets rigidbody
        userInput = GameObject.FindWithTag("Player").GetComponent<UserInput>();     // Gets User Input Script
        playerAudio = GameObject.FindWithTag("Player").GetComponent<PlayerAudioController>();   // Gets the Player Audio Controller Script
        enemyAudio = GameObject.FindFirstObjectByType<EnemyAudioController>();      // Gets the Enemy Audio Controller Script from the EnemyScriptGetting object
        rb.gravityScale = 0f;                                                       // Sets projetile gravity scale to 0 to fly straight
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;            // Sets detecting mode to continuous to always be checking for collision
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;                    // Sets the interpolation for smoother movements
        rb.freezeRotation = true;                                                   //  Freezes rotatation so projectile does not curve
    }

    // Function to fire the projectile in a specific direction, with a specific damage, with a specific owner to determine collision effects
    public void Fire(Vector2 direction, int dmg, ProjectileOwner ownerType = ProjectileOwner.Player)
    {
        if (direction.sqrMagnitude < 0.0001f) direction = Vector2.right;
        direction.Normalize();

        rb.linearVelocity = direction * speed;
        transform.right = direction;

        owner = ownerType;
        if (ownerType == ProjectileOwner.Enemy)
        {
            enemyAudio.PlayEnemyProjectile();
        }
        damage = dmg;
        Invoke(nameof(DestroySelf), lifeTime);
    }

    // Check collisions of projectile
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger) return;

        // Check if player's projectile collide with anything but an enemy, if so, destroy it
        if (!collision.CompareTag("Player") && owner == ProjectileOwner.Player) Destroy(gameObject);

        // Check Player projectile collisions
        if (owner == ProjectileOwner.Player)
        {
            if (collision.CompareTag("Enemy"))
            {
                enemyHS = collision.GetComponent<HealthSystem>();
                enemyAudio.PlayEnemyHit();
                enemyHS.TakeDamage(userInput.GetDamage());
                Destroy(gameObject);
            }

            if (collision.CompareTag("Barrel"))
            {
                HealthSystem playerHS = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthSystem>();
                playerHS.Heal(25);
                Destroy(collision.gameObject);
            }
        }
        else
        {
            // Check if enemy projectile hits the player
            if (collision.CompareTag("Player"))
            {
                playerHS = collision.GetComponent<HealthSystem>();
                playerAudio.PlayPlayerHit();
                playerHS.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }

    // Sets the projectile owner for when player reflects a projectile
    public void SetProjectileOwner(ProjectileOwner newOwner)
    {
        owner = newOwner;
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
