using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public BoxCollider2D colBox;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("doorClosed", true);
        animator.SetBool("isOpening", false);
        colBox.gameObject.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (collision.CompareTag("Player"))
        {
            animator.SetBool("isOpening", true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        animator.SetBool("isOpening", false);
    }

    public void EnableDoorCollider()
    {
        colBox.GetComponents<BoxCollider2D>()[1].enabled = true;
    }

    public void DisableDoorCollider()
    {
        colBox.GetComponents<BoxCollider2D>()[1].enabled = false;
    }
}
