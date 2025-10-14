using Unity.Cinemachine;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    // Checks the collisions of the player
    private CinemachineCamera cam;
    private CapsuleCollider2D playerCollider;

    void Awake()
    {
        cam = FindFirstObjectByType<CinemachineCamera>();
        playerCollider = GetComponent<CapsuleCollider2D>();
    }

    // If the player entered the room, disable the "cover" and enable all objects in the room
    void OnTriggerEnter2D(Collider2D collision)
    {

        if (!collision.IsTouching(playerCollider)) return;

        GameObject room = collision.gameObject;
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
        if (room.CompareTag("Cover"))
        {
            room.GetComponent<SpriteRenderer>().enabled = true;
            foreach (Transform child in room.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
