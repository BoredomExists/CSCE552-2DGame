using System.Collections;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Manager for the animations and attacks of the Final Boss
/// </summary>
public class FinalBoss : MonoBehaviour
{
    [Header("References")]
    public FinalBossAudioController bossAudio;                              // Audio Controller for final boss
    public Animator animator;                                               // Animator Component
    public HealthSystem hs;                                                 // Health System of the Final Boss
    public BoxCollider2D leftArm;                                           // Collider hit box for the left arm
    public BoxCollider2D rightArm;                                          // Collider hit box for the right arm
    public CircleCollider2D head;                                           // Collider circle for the head

    public GameObject bossHealthBar;                                        // UI Health Bar Game Object to enable when starting boss fight
    public Slider bossSlider;                                               // Slider for Health Bar
    public Transform fire1;                                                 // Firing points for when final boss is firing projectiles (6 of them)
    public Transform fire2;
    public Transform fire3;
    public Transform fire4;
    public Transform fire5;
    public Transform fire6;
    public CircleCollider2D shockwave1;                                    // Trigger Circle colliders to represent the range of the slam down attack (2 for each arm)
    public CircleCollider2D shockwave2;

    [Header("Timing")]
    public float delayBetweenMoves = 5f;                                    // How long before the boss can do another move
    public float postAttackBuffer = .5f;                                    // Delay for the animation of a move to finish

    [Header("Projectile")]
    public GameObject projPrefab;                                           // Prefab of the enemy projectile
    public int projDMG = 20;                                                // The damage that the projectile will do on hit

    [Header("Slam Ability FX")]
    public LineRenderer slam1Line;                                          // Creates the line for the left arm slam
    public LineRenderer slam2Line;                                          // Creates the line for the right arm
    public int ringSegments = 48;                                           // Sets how many segments the ring will have
    public float ringWidth = 0.05f;                                        // Sets how width of the line of the ring
    public Color ringColor = new Color(1f, 0.5f, 0.2f, 0.9f);               // Color of the ring

    private Coroutine move;                                                 // Coroutine for the enemy to do one move at a time
    private Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        LevelManager.Instance.SetFinalBoss(transform.parent.gameObject);
        bossAudio = GetComponentInParent<FinalBossAudioController>();                       // Gets the Audio Controller from Parent
        animator = GetComponent<Animator>();                                                // Gets the Animator of the Final Boss
        hs = GetComponentInParent<HealthSystem>();                                          // Gets the HealthSystem of the Final Boss
        bossHealthBar.SetActive(true);                                                      // Sets the boss health bar to be active
        UpdateBossBar(); // Sets the value of the health bar
        player = GameObject.FindWithTag("Player").transform;                                // Gets the player
        rightArm = GetComponentsInParent<BoxCollider2D>()[0];                               // Gets the right arm hit box
        leftArm = GetComponentsInParent<BoxCollider2D>()[1];                                // Gets the left arm hit box
        head = GetComponentsInParent<CircleCollider2D>()[0];                                // Gets the head hit box
        shockwave1 = GetComponentsInParent<CircleCollider2D>()[1];                          // Gets the collider representing the left arm slam attack
        shockwave2 = GetComponentsInParent<CircleCollider2D>()[2];                          // Gets the collider representing the right arm slam attack

        slam1Line = CreateSlamCircle("slam1Line", ringColor);                               // Creates the visual line of the slam attack for the left and right arm
        slam2Line = CreateSlamCircle("slame2Line", ringColor);

        move = StartCoroutine(GetMove());                                                  // Starts the Coroutine for the Final Boss to do attacks
    }

    void Update()
    {
        // Updates Boss Bar when damaged
        if(hs.IsHealthChanged())
        {
            UpdateBossBar();
            if (hs.GetCurrentHealth() <= 0)
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }

    // Makes the ring is still applied to the colliders
    private void LateUpdate()
    {
        if (slam1Line != null && slam1Line.enabled && shockwave1 != null)
        {
            UpdateRingFromCollider(slam1Line, shockwave1);
        }

        if (slam2Line != null && slam2Line.enabled && shockwave2 != null)
        {
            UpdateRingFromCollider(slam2Line, shockwave2);
        }
    }

    IEnumerator GetMove()
    {
        yield return new WaitForSeconds(1f);                            // Buffer before starting the routine

        while (true)
        {
            yield return new WaitForSeconds(delayBetweenMoves);         // Delay before picking a new attack

            int moveNumber = Random.Range(0, 3);                        // 0 = Shield, 1 = Fire Projectiles, 2 = Slam Attack
            switch (moveNumber)
            {
                case 0:
                    bossAudio.PlayShieldAudio();
                    animator.SetTrigger("isShield");
                    break;

                case 1:
                    animator.SetTrigger("isFiring");
                    break;

                case 2:
                    StartCoroutine(PlaySlammingAudio());
                    animator.SetTrigger("isSlamming");
                    break;
            }
            yield return new WaitForSeconds(postAttackBuffer);      // Delay for animation to finish before starting routine again
        }
    }

    IEnumerator PlaySlammingAudio()
    {
        yield return new WaitForSeconds(1.5f);
        bossAudio.PlaySlamAudio();
    }

    // Creates all projectiles and fires them
    public void FireProjectiles()
    {
        var p1 = Instantiate(projPrefab, fire1.position, Quaternion.identity).GetComponent<Projectile>();
        p1.Fire((player.position - fire1.position).normalized, projDMG, Projectile.ProjectileOwner.Enemy);

        var p2 = Instantiate(projPrefab, fire2.position, Quaternion.identity).GetComponent<Projectile>();
        p2.Fire((player.position - fire2.position).normalized, projDMG, Projectile.ProjectileOwner.Enemy);

        var p3 = Instantiate(projPrefab, fire3.position, Quaternion.identity).GetComponent<Projectile>();
        p3.Fire((player.position - fire3.position).normalized, projDMG, Projectile.ProjectileOwner.Enemy);

        var p4 = Instantiate(projPrefab, fire4.position, Quaternion.identity).GetComponent<Projectile>();
        p4.Fire((player.position - fire4.position).normalized, projDMG, Projectile.ProjectileOwner.Enemy);

        var p5 = Instantiate(projPrefab, fire5.position, Quaternion.identity).GetComponent<Projectile>();
        p5.Fire((player.position - fire5.position).normalized, projDMG, Projectile.ProjectileOwner.Enemy);

        var p6 = Instantiate(projPrefab, fire6.position, Quaternion.identity).GetComponent<Projectile>();
        p6.Fire((player.position - fire6.position).normalized, projDMG, Projectile.ProjectileOwner.Enemy);
    }

    // Enables/Disables the arms and head hitbox when boss is starting/ending shield animation
    public void EnableHitBoxes()
    {
        head.enabled = true;
        leftArm.enabled = true;
        rightArm.enabled = true;
    }
    public void DisableHitBoxes()
    {
        head.enabled = false;
        leftArm.enabled = false;
        rightArm.enabled = false;
    }

    // Enables/Disables the firing points when boss is starting/ending firing animation
    public void EnableFiringPoints()
    {
        fire1.gameObject.SetActive(true);
        fire2.gameObject.SetActive(true);
        fire3.gameObject.SetActive(true);
        fire4.gameObject.SetActive(true);
        fire5.gameObject.SetActive(true);
        fire6.gameObject.SetActive(true);
    }
    public void DisableFiringPoints()
    {
        fire1.gameObject.SetActive(false);
        fire2.gameObject.SetActive(false);
        fire3.gameObject.SetActive(false);
        fire4.gameObject.SetActive(false);
        fire5.gameObject.SetActive(false);
        fire6.gameObject.SetActive(false);
    }

    // Enables/Disables slam circle colliders when boss is starting/ending slam attack
    public void EnableSlamCircles()
    {
        shockwave1.enabled = true;
        shockwave2.enabled = true;

        if (slam1Line != null)
        {
            UpdateRingFromCollider(slam1Line, shockwave1);
            slam1Line.enabled = true;
        }
        if (slam2Line != null)
        {
            UpdateRingFromCollider(slam2Line, shockwave2);
            slam2Line.enabled = true;
        }
    }
    public void DisableSlamCircles()
    {
        shockwave1.enabled = false;
        shockwave2.enabled = false;

        if (slam1Line != null) slam1Line.enabled = false;
        if (slam2Line != null) slam2Line.enabled = false;
    }

    // Creates the visual for the slam attack
    private LineRenderer CreateSlamCircle(string name, Color c)
    {
        var gameOBJ = new GameObject(name);
        gameOBJ.transform.SetParent(transform, false);
        var lineRender = gameOBJ.AddComponent<LineRenderer>();
        lineRender.useWorldSpace = true;
        lineRender.loop = true;
        lineRender.positionCount = ringSegments;
        lineRender.startWidth = lineRender.endWidth = ringWidth;
        lineRender.material = new Material(Shader.Find("Sprites/Default"));
        lineRender.sortingLayerName = "Default";
        lineRender.sortingOrder = 10;
        lineRender.enabled = false;
        return lineRender;
    }

    // Makes sure the visual stays on the circle colliders of the slam attack
    private void UpdateRingFromCollider(LineRenderer lineRender, CircleCollider2D col)
    {
        // Sets the line renderer position and scale to the AOE Circle collider position and scale
        Vector3 center = col.transform.TransformPoint(col.offset);
        float scaleX = Mathf.Abs(col.transform.lossyScale.x);
        float scaleY = Mathf.Abs(col.transform.lossyScale.y);
        float radius = col.radius * Mathf.Max(scaleX, scaleY);

        if (lineRender.positionCount != ringSegments) lineRender.positionCount = ringSegments;

        for (int i = 0; i < ringSegments; i++)
        {
            float t = (i / (float)ringSegments) * Mathf.PI * 2f;
            Vector3 p = center + new Vector3(Mathf.Cos(t) * radius, Mathf.Sin(t) * radius, 0f);
            lineRender.SetPosition(i, p);
        }
    }

    // Updates the health counter of the health bar and if 0, destroy the enemy
    public void UpdateBossBar()
    {
        bossSlider.value = Mathf.Clamp01((float)hs.GetCurrentHealth() / hs.GetMaxHealth());
        if (hs.GetCurrentHealth() <= 0)
        {
            Destroy(gameObject);
        }
    }
}
