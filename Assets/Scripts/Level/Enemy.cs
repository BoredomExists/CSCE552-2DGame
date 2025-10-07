using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Gravity Settings")]
    public float gravityStr = 9.81f;

    private Rigidbody2D rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        Vector2 localDown = -transform.up;

        rb.AddForce(localDown * gravityStr * rb.mass, ForceMode2D.Force);
    }
}
