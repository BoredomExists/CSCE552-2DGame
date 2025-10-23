using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Manager for the player's gravity launch and repulsor wave abilities
/// </summary>
public class PlayerAbilities : MonoBehaviour
{
    public UserInput userInput;                         // Gets the user script for getting the player's damage
    [Header("Gravity Launch")]
    public float jumpForce = 20f;                       // How much power to launch the player
    public float GLCooldown = 0.5f;                     // Cooldown before Gravity Launch can be used again
    private bool jumpReady;                             // Check to see if the player can launch again
    private float lastLaunch = -999f;                   // Timer to compare to cooldown to know when cooldown is up

    [Header("Repulsor Wave")]
    public CircleCollider2D aoe;                        // Circle that represents the repulsor wave ability
    public LayerMask projectileLayer;                   // Layer to check for incoming projectiles
    public float repulsorDuration = 0.25f;              // How long the ability lasts
    public float repulsorCooldown = 1f;                 // How long until Repuslor Wave can be used again
    private float lastRepulse = -999f;                  // Timer to compare cooldown to know when cooldown is up

    [Header("RepulsorWave FX")]
    public LineRenderer repulseVisual;                  // Line to visually show the repulsor wave ability in game
    public int ringSegments = 48;                       // How many segments are in the ring
    public float ringWidth = 0.05f;                     // How wide is the line's width of the ring
    public Color ringActiveColor = new Color(0f, 1f, 1f, 0.9f); // The color of the ring

    [Header("UI Settings")]
    public TMP_Text glCooldownText;             // Gets the text of UI Element representing the cooldown
    public TMP_Text rwCooldownText;

    private Rigidbody2D rb;
    private readonly Collider2D[] repulseHits = new Collider2D[32];         // Setup of colliders for the repulsor wave ability to check for incoming projectiles
    private ContactFilter2D repulseFilter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();                           // Gets the rigidbody
        userInput = GetComponent<UserInput>();                      // Gets the User Input Script
        aoe = GetComponentInChildren<CircleCollider2D>();           // Gets the repulsor wave collider
        aoe.isTrigger = true;                                       // Sets it to be a trigger collider
        aoe.enabled = false;                                        // Disables it on start

        // Check for projectiles colliding with the AOE
        repulseFilter = new ContactFilter2D
        {
            useLayerMask = true,
            layerMask = projectileLayer,
            useTriggers = true
        };

        // Creates base values for the repulsor wave ring
        if (!repulseVisual && aoe)
        {
            var ringGO = new GameObject("RepulseRing");
            ringGO.transform.SetParent(aoe.transform, false);
            repulseVisual = ringGO.AddComponent<LineRenderer>();
            repulseVisual.useWorldSpace = true;
            repulseVisual.loop = true;
            repulseVisual.positionCount = ringSegments;
            repulseVisual.startWidth = repulseVisual.endWidth = ringWidth;
            repulseVisual.material = new Material(Shader.Find("Sprites/Default"));
            repulseVisual.sortingLayerName = "Default";
            repulseVisual.sortingOrder = 10;
            repulseVisual.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCooldownUI(); // Updates the UI cooldown text when an ability is used

        // Input commands for abilities (Q = Gravity Launch, E = Repulsor Wave)
        if (Input.GetKeyDown(KeyCode.Q) && Time.time >= lastLaunch + GLCooldown)
        {
            jumpReady = true;
        }
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= lastRepulse + repulsorCooldown)
        {
            StartCoroutine(RepulseWave());
        }
    }

    // Check to make sure the line renderer is still applied to the AOE circle collider
    void LateUpdate()
    {
        if (!aoe || !repulseVisual) return;

        repulseVisual.enabled = aoe.enabled;
        if (!repulseVisual.enabled) return;

        RebuildRingFromCollider();

        float a = 0.6f + 0.4f * Mathf.Sin(Time.time * 8f);
        var c = ringActiveColor; c.a = a;
        repulseVisual.startColor = repulseVisual.endColor = c;
    }

    // Activates the Gravity Launch ability
    void FixedUpdate()
    {
        GravityLaunch();
    }

    // Sets up the gravity launch ability
    private void GravityLaunch()
    {
        if (jumpReady && userInput.CheckIsGrounded())
        {
            lastLaunch = Time.time;     // Reset the last time use for cooldown
            rb.AddForce(-Physics2D.gravity.normalized * jumpForce, ForceMode2D.Impulse);
        }
        jumpReady = false;
    }

    // Activates the Repulsor Wave ability
    IEnumerator RepulseWave()
    {
        lastRepulse = Time.time;            // Reset the last time use for cooldown
        aoe.enabled = true;                 // Enables the ring

        float end = Time.time + repulsorDuration;   // Sets the duration of the ability
        while (Time.time < end)
        {
            // Creates the colliders within the ring and checks for a projectile
            int count = aoe.Overlap(repulseFilter, repulseHits);
            for (int i = 0; i < count; i++)
            {
                var hit = repulseHits[i];
                if (!hit) continue;
                var prb = hit.attachedRigidbody;
                if (!prb) continue;

                Vector2 norm = (prb.position - (Vector2)transform.position).normalized;

                // Gets the projectile script from the collided projectile, makes sure the collided projectile is not from the player
                var proj = prb.GetComponent<Projectile>();
                if (proj.owner == Projectile.ProjectileOwner.Player) continue;

                // If the projectile collided with the ring, set its owner to the player for enemies to take damage
                if (Vector2.Dot(prb.linearVelocity, norm) < 0f)
                {
                    prb.linearVelocity = Vector2.Reflect(prb.linearVelocity, norm);
                    proj.SetProjectileOwner(Projectile.ProjectileOwner.Player);
                }
            }
            yield return null;
        }
        aoe.enabled = false;
    }

    // Creates the visual ring for the Repulsor Wave ability
    void RebuildRingFromCollider()
    {
        // Sets the line renderer position and scale to the AOE Circle collider position and scale
        Vector3 center = aoe.transform.TransformPoint(aoe.offset);
        float scaleX = Mathf.Abs(aoe.transform.lossyScale.x);
        float scaleY = Mathf.Abs(aoe.transform.lossyScale.y);
        float radius = aoe.radius * Mathf.Max(scaleX, scaleY);
        if (repulseVisual.positionCount != ringSegments)
            repulseVisual.positionCount = ringSegments;

        for (int i = 0; i < ringSegments; i++)
        {
            float t = (i / (float)ringSegments) * Mathf.PI * 2f;
            Vector3 p = center + new Vector3(Mathf.Cos(t) * radius, Mathf.Sin(t) * radius, 0f);
            repulseVisual.SetPosition(i, p);
        }

    }

    // Reflects any incoming projectiles causing damage to collided enemies
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!aoe || !aoe.enabled) return;
        if ((projectileLayer.value & (1 << collision.gameObject.layer) & projectileLayer) == 0) return;

        var proj = collision.GetComponent<Projectile>();
        if (proj == null || proj.owner == Projectile.ProjectileOwner.Player) return; // Ignore player projectiles

        var rb = collision.attachedRigidbody;
        if (rb != null)
        {
            Vector2 projectileTarget = (rb.position - (Vector2)transform.position).normalized;
            rb.linearVelocity = Vector2.Reflect(rb.linearVelocity, projectileTarget);
        }
    }

    // Updates the UI text of the ability cooldowns
    private void UpdateCooldownUI()
    {
        float glElapsed = Time.time - lastLaunch;               // Gets the time of last use
        float glRemaining = Mathf.Max(0, GLCooldown - glElapsed); // Gets how long until ability can be used again
        bool glReady = glElapsed >= GLCooldown;

        if (glReady) glCooldownText.text = "Ready";
        else glCooldownText.text = (glRemaining > 1f) ? Mathf.CeilToInt(glRemaining).ToString() : glRemaining.ToString("F1");   // Sets text if cooldown is ready

        // Same Concept as gravity launch
        float rwElapsed = Time.time - lastRepulse;
        float rwRemaining = Mathf.Max(0f, repulsorCooldown - rwElapsed);
        bool rwReady = rwElapsed >= repulsorCooldown;

        if (rwReady) rwCooldownText.text = "Ready";
        else rwCooldownText.text = (rwRemaining > 1f) ? Mathf.CeilToInt(rwRemaining).ToString() : rwRemaining.ToString("F1");

    }
}