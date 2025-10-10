using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("Animation")]
    public Animator animator;
    public float walkThreshold = 0.1f;

    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 vel = rb.linearVelocity;
        Vector2 grav = Physics2D.gravity.sqrMagnitude < 1e-6f ? Vector2.down : Physics2D.gravity.normalized;
        Vector2 lateral = new Vector2(-grav.y, grav.x);
        float lateralSpeed = Vector2.Dot(vel, lateral);

        bool isWalking = lateralSpeed > walkThreshold;

        animator.SetBool("isWalking", isWalking);
    }
}
