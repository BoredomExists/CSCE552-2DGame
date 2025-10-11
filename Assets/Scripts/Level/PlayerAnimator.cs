using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("Animation")]
    public Animator animator;
    public float walkThreshold = 0.1f;

    public UserInput userInput;

    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 vel = rb.linearVelocity;

        Vector2 grav = Physics2D.gravity.sqrMagnitude < 1e-6f ? Vector2.down : Physics2D.gravity.normalized;
        Vector2 lateral = new Vector2(-grav.y, grav.x);
        float lateralSpeed = Vector2.Dot(vel, lateral);

        bool isWalkingRight = lateralSpeed > walkThreshold;
        bool isWalkingLeft = lateralSpeed < -walkThreshold;

        bool isGrounded = userInput != null ? userInput.CheckIsGrounded() : true;


        animator.SetBool("isJumping", !isGrounded);
        if (!isGrounded)
        {
            animator.SetBool("isWalkingRight", false);
            animator.SetBool("isWalkingLeft", false);
        }
        else
        {
            animator.SetBool("isWalkingRight", isWalkingRight);
            animator.SetBool("isWalkingLeft", isWalkingLeft);
        }

    }
}
