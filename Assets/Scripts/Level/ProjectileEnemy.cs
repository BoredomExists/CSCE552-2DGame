using UnityEngine;

/// <summary>
/// Manager for any enemy that fire projectile
/// </summary>
public class ProjectileEnemy : MonoBehaviour
{
    [Header("References")]
    public EnemyAudioController enemyAudio;                                 // Audio Controller for the Enemy
    public Transform hand;      // Projectile Starting that a projectile enemy must have
    public Transform otherHand; // For Mini-Boss Projectile
    public Transform flyingBossHand; // For Three armed flying enemy
    public GameObject projectilePrefab; // Prefab of the projectile
    public Transform target;            // The player to fire projectiles at

    [Header("Enemy Settings")]
    public float aggressionRadius = 10f;    // How far they can see the player
    public float attackRange = 8f;          // How close the player must be to fire projectiles
    public float fireCooldown = 1f;         // How long until next fire
    public int damage;

    private float lastAtkTime = -999f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;            // Gets the player
        enemyAudio = GetComponent<EnemyAudioController>();              // Gets the audio controller for the enemy
    }

    // Update is called once per frame
    void Update()
    {
        // Gets the distance between the projectile starting point and player
        Vector2 distanceToTarget = (Vector2)(target.position - hand.position);
        float distance = distanceToTarget.magnitude;

        if (distance > aggressionRadius) return;        // If player is outside aggression radius, do nothing

        // If player is within range and attacking range, fire projectile
        if (distance <= attackRange && Time.time >= lastAtkTime + fireCooldown)
        {
            FireProjectile(distanceToTarget.normalized);
        }
    }

    // Function to fire projectil
    private void FireProjectile(Vector2 direction)
    {
        lastAtkTime = Time.time;
        enemyAudio.PlayEnemyProjectile();
        var projGO = Instantiate(projectilePrefab, hand.position, Quaternion.identity);         // Creates the new projectile
        var proj = projGO.GetComponent<Projectile>();                                           // Gets the projectile script from the projectile Game Object

        // Fires projectile(s) based on amount of projectile starting points
        if (proj) proj.Fire(direction, damage, Projectile.ProjectileOwner.Enemy);
        if (otherHand)
        {
            var projGO2 = Instantiate(projectilePrefab, otherHand.position, Quaternion.identity);
            var proj2 = projGO2.GetComponent<Projectile>();
            if (proj2) proj2.Fire(direction, damage, Projectile.ProjectileOwner.Enemy);
        }
        if (flyingBossHand)
        {
            var projGO3 = Instantiate(projectilePrefab, flyingBossHand.position, Quaternion.identity);
            var proj3 = projGO3.GetComponent<Projectile>();
            if (proj3) proj3.Fire(direction, damage, Projectile.ProjectileOwner.Enemy);
        }
    }
}
