using UnityEngine;

public class ProjectileEnemy : MonoBehaviour
{
    [Header("References")]
    public Transform hand;
    public Transform otherHand; // For Mini-Boss Projectile 
    public GameObject projectilePrefab;
    public Transform target;

    [Header("Enemy Settings")]
    public float aggressionRadius = 10f;
    public float attackRange = 8f;
    public float fireCooldown = 1f;
    public int damage;

    private float lastAtkTime = -999f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 distanceToTarget = (Vector2)(target.position - hand.position);
        float distance = distanceToTarget.magnitude;

        if (distance > aggressionRadius) return;

        if (distance <= attackRange && Time.time >= lastAtkTime + fireCooldown)
        {
            FireProjectile(distanceToTarget.normalized);
        }
    }

    private void FireProjectile(Vector2 direction)
    {
        lastAtkTime = Time.time;
        var projGO = Instantiate(projectilePrefab, hand.position, Quaternion.identity);
        var proj = projGO.GetComponent<Projectile>();
        if (proj) proj.Fire(direction, damage);
        if (otherHand)
        {
            var projGO2 = Instantiate(projectilePrefab, otherHand.position, Quaternion.identity);
            var proj2 = projGO2.GetComponent<Projectile>();
            if (proj2) proj2.Fire(direction, damage);
        }
    }
}
