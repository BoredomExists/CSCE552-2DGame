using Unity.Cinemachine;
using UnityEngine;


/// <summary>
/// Manages the collisions that player will interact with
/// </summary>
public class PlayerCollisions : MonoBehaviour
{
    [Header("References")]
    private CinemachineCamera cam;                    // Camera that the level is using
    private CapsuleCollider2D playerCollider;         // Player Capsule Collider
    public GameObject secretText;                                                           // Secret Text Game Object in Canvas

    void Awake()
    {
        cam = FindFirstObjectByType<CinemachineCamera>();       // Gets the cinemachine camera
        playerCollider = GetComponent<CapsuleCollider2D>();     // Gets the player collider
    }

    // If the player entered the room, disable the "cover" and enable all objects in the room
    void OnTriggerEnter2D(Collider2D collision)
    {

        if (!collision.IsTouching(playerCollider)) return;

        GameObject room = collision.gameObject;

        // Disable all children of the Cover game object
        if (room.CompareTag("Cover"))
        {
            room.GetComponent<SpriteRenderer>().enabled = false;
            foreach (Transform child in room.transform)
            {
                child.gameObject.SetActive(true);
            }
            cam.transform.rotation = Quaternion.identity;
        }
    }

    // Checks if the player exited the room, if so, enable the "cover" while disabling all objects in the room
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.IsTouching(playerCollider)) return;

        GameObject room = collision.gameObject;

        // Disables all children of Cover Game Object
        if (room.CompareTag("Cover"))
        {
            if (secretText.activeSelf) secretText.SetActive(false);

            room.GetComponent<SpriteRenderer>().enabled = true;
            foreach (Transform child in room.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
