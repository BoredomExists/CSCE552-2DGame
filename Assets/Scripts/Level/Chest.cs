using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("References")]
    public UserInput userInput;
    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        userInput = GameObject.FindGameObjectWithTag("Player").GetComponent<UserInput>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player Damage Set to 100");
            userInput.SetDamage(100);
            animator.SetTrigger("OpenChest");
        }
    }
}
