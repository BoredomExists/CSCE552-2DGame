using UnityEngine;


/// <summary>
/// Handles the regular door animations and colliders
/// </summary>
public class DoorAnimation : MonoBehaviour
{
    [Header("References")]
    public Animator animator;                                                           // Animator Component
    public BoxCollider2D colBox;                                                        // Non-Trigger collider box of the regular doors

    void Start()
    {
        animator = GetComponent<Animator>();                                            // Gets the Animator of the door
        animator.SetBool("doorClosed", true);                                           // Check to make sure the door is closed
        animator.SetBool("isOpening", false);                                           // Check to make sure the door will not open until Trigger event
        colBox.gameObject.SetActive(true);                                              // Check to make sure the player cannot walk through door before opening
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;                                    // Check to make sure nothing but the player triggers the animation
        if (collision.CompareTag("Player"))
        {
            animator.SetBool("isOpening", true);                                        // Sets the boolean for the door opening animation
            animator.SetBool("doorClosed", false);                                      // Makes sure the door does not go back to being closed when trigger event is happening
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return; ;                       // Check to make sure nothing but the player triggers the animation
        animator.SetBool("isOpening", false); ;                                   // Sets the boolean for the door closing animation and 
                                                                                  // stays until boolean for door opening animation is true again
    }

    public void EnableDoorCollider()
    {
        colBox.GetComponents<BoxCollider2D>()[1].enabled = true;            // Gets the Non-Trigger collider and enables it
    }

    public void DisableDoorCollider()
    {
        colBox.GetComponents<BoxCollider2D>()[1].enabled = false;           // Gets the Non-Trigger collider and disables it
    }
}
