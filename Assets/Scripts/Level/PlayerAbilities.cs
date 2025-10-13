using System.Collections;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public UserInput userInput;
    [Header("Gravity Launch")]
    public float jumpForce = 20f;
    public float GLCooldown = 0.5f;
    private bool jumpReady;
    private float lastLaunch = -999f;

    [Header("Repulsor Wave")]
    public CircleCollider2D aoe;
    public LayerMask projectileLayer;
    public float repulsorDuration = 0.25f;
    public float repulsorCooldown = 1f;
    private float lastRepulse = -999f;

    [Header("RepulsorWave FX")]
    public LineRenderer repulseVisual;
    public int ringSegments = 48;
    public float ringWidth = 0.05f;
    public Color ringActiveColor = new Color(0f, 1f, 1f, 0.9f);

    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        userInput = GetComponent<UserInput>();
        aoe = GetComponent<CircleCollider2D>();
        aoe.isTrigger = true;
        aoe.enabled = false;

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
        if (Input.GetKeyDown(KeyCode.Z) && Time.time >= lastLaunch + GLCooldown)
        {
            jumpReady = true;
        }
        if (Input.GetKeyDown(KeyCode.X) && Time.time >= lastRepulse + repulsorCooldown)
        {
            StartCoroutine(RepulseWindow());
        }
    }

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

    void FixedUpdate()
    {
        GravityLaunch();
    }

    private void GravityLaunch()
    {
        if (jumpReady && userInput.CheckIsGrounded())
        {
            lastLaunch = Time.time;
            rb.AddForce(-Physics2D.gravity.normalized * jumpForce, ForceMode2D.Impulse);
        }
        jumpReady = false;
    }

    IEnumerator RepulseWindow()
    {
        lastRepulse = Time.time;
        aoe.enabled = true;
        yield return new WaitForSeconds(repulsorDuration);
        aoe.enabled = false;
    }

    void RebuildRingFromCollider()
    {
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!aoe || !aoe.enabled) return;
        if ((projectileLayer.value & (1 << collision.gameObject.layer) & projectileLayer) == 0) return;

        var rb = collision.attachedRigidbody;
        if (rb != null)
        {
            Vector2 projectileTarget = (rb.position - (Vector2)transform.position).normalized;
            rb.linearVelocity = Vector2.Reflect(rb.linearVelocity, projectileTarget);
        }
    }
}
